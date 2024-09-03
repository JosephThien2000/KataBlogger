using AspNetCoreHero.ToastNotification.Abstractions;
using KataBlogger.Models;
using KataBlogger.Utilities;
using KataBlogger.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KataBlogger.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly INotyfService _notification;
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                INotyfService notyfService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notification = notyfService;
        }

        [Authorize(Roles="Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var vm = users.Select(x => new UserVM()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                Email = x.Email,
            }).ToList();
            // assigning roles
            foreach (var user in vm)
            {
                var singleUser = await _userManager.FindByIdAsync(user.Id);
                var role = await _userManager.GetRolesAsync(singleUser);
                user.Role = role.FirstOrDefault();
            }


            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                _notification.Error("User does not exists!");
                return View();
            }
            var vm = new ResetPasswordVM()
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName
            };
            return View(vm);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if(!ModelState.IsValid) { return View(vm); }
            var existingUser = await _userManager.FindByIdAsync(vm.Id);
            if(existingUser == null)
            {
                _notification.Error("User not found");
                return View(vm);

            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var result = await _userManager.ResetPasswordAsync(existingUser, token, vm.NewPassword);
            if(result.Succeeded)
            {
                _notification.Success("Password reset successfully");
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }
        

        [Authorize(Roles ="Admin")]     
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if(!ModelState.IsValid) { return View(vm); }
            var checkUserByEmail = await _userManager.FindByEmailAsync(vm.Email);
            if(checkUserByEmail != null)
            {
                _notification.Error("Email already exists");
                return View(vm);
            }
            var  checkUserByUsername = await _userManager.FindByEmailAsync(vm.UserName);
            if(checkUserByUsername != null)
            {
                _notification.Error("Username already exists");
                return View(vm);
            }

            var applicationUser = new ApplicationUser()
            {
                Email = vm.Email,
                UserName = vm.UserName,
                FirstName = vm.FirstName,
                LastName = vm.LastName
                
            };

            var result = await _userManager.CreateAsync(applicationUser, vm.Password);
            if(result.Succeeded)
            {
                if(vm.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAdmin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAuthor);
                }

                _notification.Success("User registered successfully");
                return RedirectToAction("Index", "User", new {area="Admin"});
            }

            return View(vm);
        }

        // [Authorize(Roles ="Admin")]     
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View(new RegisterVM());
        }

        // // [Authorize(Roles = "Admin")]
        // [HttpPost]
        // public async Task<IActionResult> RegisterUser(RegisterVM vm)
        // {
        //     if(!ModelState.IsValid) { return View(vm); }
        //     var checkUserByEmail = await _userManager.FindByEmailAsync(vm.Email);
        //     if(checkUserByEmail != null)
        //     {
        //         _notification.Error("Email already exists");
        //         return View(vm);
        //     }
        //     var  checkUserByUsername = await _userManager.FindByEmailAsync(vm.UserName);
        //     if(checkUserByUsername != null)
        //     {
        //         _notification.Error("Username already exists");
        //         return View(vm);
        //     }

        //     var applicationUser = new ApplicationUser()
        //     {
        //         Email = vm.Email,
        //         UserName = vm.UserName,
        //         FirstName = vm.FirstName,
        //         LastName = vm.LastName
                
        //     };

        //     // Làm lại để khi đăng ký xong thì 

        //     var result = await _userManager.CreateAsync(applicationUser, vm.Password);
        //     if(result.Succeeded)
        //     {
        //         if(vm.IsAdmin)
        //         {
        //             await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAdmin);
        //         }
        //         else
        //         {
        //             await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAuthor);
        //         }

        //         _notification.Success("User registered successfully");
        //         return RedirectToAction("Login", "User", new {area="Admin"});
        //     }

        //     return View(vm);
        // }
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser account = new ApplicationUser();
                account.Email = viewModel.Email;
                account.FirstName = viewModel.FirstName;
                account.LastName = viewModel.LastName;
                account.Password = viewModel.Password;
                account.UserName = viewModel.UserName;
                
                try
                {
                    _context.UserAccounts.Add(account);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName} registered successfully. Please Login.";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Please enter unique Email or Password.");
                    return View(viewModel);
                }
                return View(viewModel);//???
            }
            return View(viewModel);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if(!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new LoginVM());
            }
            return RedirectToAction("Index", "Post", new {area="Admin"});
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if(!ModelState.IsValid) { return View(vm);}
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == vm.Username);
            if(existingUser == null)
            {
                _notification.Error("Username does not exist");
                return View(vm);
            }
            
            var verifyPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);
            if(!verifyPassword)
            {
                _notification.Error("Incorrect password");
                return View(vm);
            }

            await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, true);
            _notification.Success("Login Successful");

            return RedirectToAction("Index", "User", new {area="Admin"});
        }

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            _notification.Success("Logout Successful");
            return RedirectToAction("Index", "Home", new {area=""});
        }

        [HttpGet("AccessDenied")]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}