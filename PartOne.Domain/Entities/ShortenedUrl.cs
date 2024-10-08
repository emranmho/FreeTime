using System.ComponentModel.DataAnnotations;

namespace PartOne.Domain.Entities;

public class ShortenedUrl
{
    [Key]
    public Guid Id { get; set; }

    [Required] public string LongUrl { get; set; } = string.Empty;

    [Required]
    public string ShortUrl { get; set; }= string.Empty;

    [Required]
    public DateTime CreatedTime { get; set; }

    public DateTime ExpireDate { get; set; }

    public int UsageCount { get; set; } 
    
}