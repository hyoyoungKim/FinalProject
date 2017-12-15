using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FinalProject.App_Code
{
    public class Helper
    {
        public static string GetCon()
        {
            return ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString;
        }
    }
}