using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Country
{
    public int Id { get; set; }
    [Required]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Code must be exactly 2 characters.")]
    public string Code { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
