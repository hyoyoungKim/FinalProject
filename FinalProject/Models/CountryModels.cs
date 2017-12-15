using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class CountryModels
    {
        [Key]
        public int CountryID { get; set; }

        public string  Country { get; set; }
    }
}