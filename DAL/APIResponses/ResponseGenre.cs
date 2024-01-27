using DAL.Models;
namespace DAL.APIResponse;
public class ResponseGenre
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public override string ToString() => $"{Name}, {Description}";
}
