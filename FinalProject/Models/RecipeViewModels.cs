using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class RecipeViewModels
    {
        public List<UsersModel> AllUsers { get; set; }
        public List<RecipesModels> AllRecipes { get; set; }
        public List<CommentsModels> AllComments { get; set; }
        public List<CategoryModels> AllCategories { get; set; }
        public List<CuisineModels> AllCuisines { get; set; }
        public List<CourseModels> AllCourses { get; set; }
        public List<DifficultyModels> AllDifficulties { get; set; }
        public List<MeatModels> AllMeat { get; set; }
        public int  User { get; set; }





    }
}