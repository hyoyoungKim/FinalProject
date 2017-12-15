using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class CourseModels
    {
        [Key]
        public int CourseID { get; set; }

        public string MealCourse { get; set; }

        public int TotalCourse { get; set; }


    }
}