using ParkyAPI.Models.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ParkyAPI.Models.Trail;

namespace ParkyAPI.Models
{
    public class TrailInsertsDto
    {


        [Required]
        public string Name { get; set; }

        [Required]
        public int Distance { get; set; }


        public DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }



    }
}
