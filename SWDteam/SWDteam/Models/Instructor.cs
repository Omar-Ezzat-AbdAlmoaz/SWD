using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SWDteam.Models
{
    public class Instructor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InstructorId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string InstructorName { get; set; }

        [Display(Name = "Image")]
        [DefaultValue("default3.png")]
        public string InstrucrorImage { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]
        public string InstructorEmail { get; set; }

        [Required]
        [Display(Name = "biography")]
        public string Instructorbiography { get; set; }

        [Required]
        [Display(Name = "experience")]
        public string Instructorexperience { get; set; }

        [Required]
        [Display(Name = "Rate")]
        public int InstructorRate { get; set; }

        [Required]
        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        public virtual ICollection<Course> Courses { get; set; }


    }
}
