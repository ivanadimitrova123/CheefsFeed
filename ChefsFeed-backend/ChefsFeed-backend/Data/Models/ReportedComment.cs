namespace ChefsFeed_backend.Data.Models;

public class ReportedComment
{
    public long UserId { get; set; }
    public int CommentId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
    [ForeignKey("CommentId")]
    public Comment Comment { get; set; }
}