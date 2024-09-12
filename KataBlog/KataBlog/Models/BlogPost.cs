
public class BlogPost
{
    public PostStatus Status { get; set; }
}

public enum PostStatus
{
    Pending,
    Approved,
    Rejected
}