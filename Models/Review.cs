using System.ComponentModel.DataAnnotations;

namespace Website_Selling_Phones.Models;

public class Review
{
    public int Id { get; set; }

    [Required]
    public int PhoneId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required, Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}