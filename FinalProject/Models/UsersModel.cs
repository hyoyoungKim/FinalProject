using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class UsersModel
    {
        [Key]
        public int UserID { get; set; }

        [Display(Name = "User Name")]
        [Required]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(200)]
        public string EmailAddress { get; set; }

        [Display(Name = "Contact Number")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string ContactNo { get; set; }

        [Display(Name = "Image")]
        
        public string Image { get; set; }

        [Display(Name = "Birthdate")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }


        [Display(Name = "Date Created")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }


        public int RecipeID { get; set; }

        [Display(Name = "Recipe Name")]
        [Required]
        public string RecipeName { get; set; }

        [Display(Name = "Difficulty")]
        [Required]
        public string Difficulty { get; set; }

        [Display(Name = "Cuisine")]
        [Required]
        public string Cuisine { get; set; }

        [Display(Name = "Course")]
        [Required]
        public string MealCourse { get; set; }

        [Display(Name = "Meat Type")]
        [Required]
        public string MeatType { get; set; }

        [Display(Name = "Date Added")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
    }
}