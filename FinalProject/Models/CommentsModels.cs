using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class CommentsModels
    {
        [Key]
        public int CommentID { get; set; }

        public string Comment { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

    }
}