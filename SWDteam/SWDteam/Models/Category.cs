using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace SWDteam.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string CategoryDescription { get; set; }

        [Display(Name ="Image")]
        [DefaultValue("default.png")]
        public string CategoryImage { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

    }
}
