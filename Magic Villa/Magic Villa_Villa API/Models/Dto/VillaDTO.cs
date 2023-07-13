using System.ComponentModel.DataAnnotations;

namespace Magic_Villa_VillaAPI.Models.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }

        //makes sure you enter a string for name and don't leave the field empty
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }    
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
    }
}
