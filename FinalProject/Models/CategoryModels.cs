using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class CategoryModels
    {
        [Key]
        public int CatID { get; set; }

        [Display(Name = "Category")]
        [Required]
        public string Category { get; set; }

        [Display(Name = "Meal Course")]
        [Required]
        public string MealCourse { get; set; }

        [Display(Name = "Meat Type")]
        [Required]
        public string MeatType { get; set; }

        [Display(Name = "Cuisine")]
        [Required]
        public string Cuisine { get; set; }

        [Display(Name = "Country")]
        [Required]
        public string Country { get; set; }

        public int Difficulty { get; set; }



        public int CountryID { get; set; }

        public int CourseID { get; set; }

        public int MeatID { get; set; }

        public int CuisineID { get; set; }



    }
}