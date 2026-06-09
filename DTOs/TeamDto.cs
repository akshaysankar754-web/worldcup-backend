using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class TeamDto
    {
        public int Id { get; set; }
        [Required]
        public required string TeamName { get; set; }
    }
}
