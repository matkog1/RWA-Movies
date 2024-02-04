using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Genre
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string? Description { get; set; }

    public virtual ICollection<Video> Videos { get; } = new List<Video>();
}
