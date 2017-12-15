using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Models
{
    public class RecipesModels
    {
        [Key]
        public int RecipeID { get; set; }

        [Display(Name = "RecipeName")]
        [Required]
        public string RecipeName { get; set; }

        [Display(Name = "Description")]
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Ingredients")]
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(1000)]
        public string Ingredients { get; set; }

        [Display(Name = "Instructions")]
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(1000)]
        public string Instructions { get; set; }

        [Display(Name = "Difficulty")]
        [Required]
        public int Difficulty { get; set; }

        [Required]
        public int CatID { get; set; }

        [Required]
        public int CountryID { get; set; }

        [Required]
        public int CuisineID { get; set; }

        [Required]
        public int DiffID { get; set; }

        [Required]
        public int TagID { get; set; }

        [Required]
        public int UserID { get; set; }


        [Display(Name = "Image")]
        [Required]
        public string Image { get; set; }

        //Additional

         //Addional (Dec. 13, 2017   5:10pm)

        [Display(Name = "FirstName")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Lastname")]
        [Required]
        public string Lastname { get; set; }

        [Display(Name = "Meal Course")]
        [Required]
        public string MealCourse { get; set; }

        [Display(Name = "Meat Type")]
        [Required]
        public string MeatType { get; set; }

        [Display(Name = "Cuisine")]
        [Required]
        public string Cuisine { get; set; }

        public int CourseID { get; set; }

        public int MeatID { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public List<SelectListItem> Cuisines { get; set; }
        public List<SelectListItem> Courses { get; set; }
        public List<SelectListItem> Meats { get; set; }
        public List<SelectListItem> Difficulties { get; set; }


        public List<SelectListItem> AllRecipes { get; set; }

    }
}