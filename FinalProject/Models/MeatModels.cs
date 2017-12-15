using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class MeatModels
    {
        [Key]
        public int MeatID { get; set; }

        [Display (Name = "Meat Type")]
        [Required]
        public string MeatType { get; set; }

        public int TotalMeat { get; set; }
    }
}