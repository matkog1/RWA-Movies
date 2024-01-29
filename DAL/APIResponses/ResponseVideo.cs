using DAL.Models;
namespace DAL.APIResponse;

public class ResponseVideo
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int GenreId { get; set; }

    public int TotalSeconds { get; set; }

    public string? StreamingUrl { get; set; }

    public int? ImageId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Image? Image { get; set; }
}
