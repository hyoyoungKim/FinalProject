using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class DifficultyModels
    {
        [Key]
        public int DiffID { get; set; }

        [Required]
        public int  Difficulty { get; set; }

        public int TotalDiff { get; set; }


    }
}