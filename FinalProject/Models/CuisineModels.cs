using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class CuisineModels
    {
        [Key]
        public int CuisineID { get; set; }

        [Display(Name = "Cuisine")]
        [Required]
        public string Cuisine { get; set; }

        [Display(Name = "Country")]
        [Required]
        public string Country { get; set; }

        public int TotalCuisine { get; set; }
    }
}