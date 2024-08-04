namespace ChefsFeed_backend.Data.Models;
public class Picture
{
    [Key]
    public long Id { get; set; }
    public byte[] ImageData { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
}