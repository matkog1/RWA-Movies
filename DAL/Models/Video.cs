using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Video
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string? Description { get; set; }
    [Required]
    public int GenreId { get; set; }

    public int TotalSeconds { get; set; }
    [Required]
    public string? StreamingUrl { get; set; }

    public int? ImageId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Image? Image { get; set; }

    public virtual ICollection<VideoTag> VideoTags { get; } = new List<VideoTag>();
}
