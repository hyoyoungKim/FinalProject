using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class TagsModels
    {
        [Key]
        public int TagID { get; set; }

        public string Tags { get; set; }
    }
}