using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class VoteDto
    {
        [Required]
        public int TeamId { get; set; }
    }
}
