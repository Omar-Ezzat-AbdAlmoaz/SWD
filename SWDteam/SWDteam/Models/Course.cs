﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWDteam.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }

        [Required]
        [Display(Name ="Name")]
        public string CourseName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string CourseDescription { get; set; }

        [Display(Name = "Image")]
        [DefaultValue("default2.png")]
        public string CourseImage { get; set; }

        [Required]
        [Display(Name = "Instructor")]
        public string InstructorName { get; set; }

        [Required]
        [Display(Name = "Duration")]
        public int CourseDuration { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double CoursePrice { get; set; }

        [Required]
        [Display(Name = "Purchases")]
        public int CoursePurchases { get; set; }

        [Required]
        [Display(Name = "Rate")]
        public int CourseRate { get; set; }

        [Required]
        [Display(Name = "date")]
        public DateTime Coursedate { get; set; }

        [Required]
        public int DepartmentID { get; set; }
        [ForeignKey("DeprtmentId")]
        public Department Department { get; set; }


        [Required]
        public int InstructorID { get; set; }
        [ForeignKey("InstructorID")]
        public Instructor Instructor { get; set; }
    }
}
