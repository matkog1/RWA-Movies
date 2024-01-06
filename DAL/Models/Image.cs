using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Image
{
    public string Content { get; set; } = null!;

    public int Id { get; set; }

    public virtual ICollection<Video> Videos { get; } = new List<Video>();
}
