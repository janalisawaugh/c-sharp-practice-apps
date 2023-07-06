using System.ComponentModel.DataAnnotations;
namespace RazorPagesMovie.Models
{
    public class Movie
    {   //primary key
        public int Id { get; set; }
        //? after string means that the property is nullable
        public string? Title { get; set; }
        //specifies that time info isn't required by the user or displayed
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public decimal Price { get; set; }
    }
}
