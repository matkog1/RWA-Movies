using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.APIRequests;
public class RequestVideo
{

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [Required]
    public int GenreId { get; set; }

    public int TotalSeconds { get; set; }
    [Required]
    public string? StreamingUrl { get; set; }

    public int? ImageId { get; set; }


    [Display(Name = "Tags")]
    public string NewTags { get; set; }

}
