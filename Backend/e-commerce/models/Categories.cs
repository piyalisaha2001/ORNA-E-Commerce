using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.models
{
    public class Categories
    {
        [Key]
        public int CategoriesId { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
