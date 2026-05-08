namespace Website_Selling_Phones.Models;

public class Newsletter
{
    public string Email { get; set; } = string.Empty;
    public DateTime SubscribedAt { get; set; } = DateTime.Now;
}
