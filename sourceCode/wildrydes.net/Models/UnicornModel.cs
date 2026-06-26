using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wildrydes.net.Models;

[Table("Unicorns")]
public class UnicornModel
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Color { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public int Rating { get; set; }
}
