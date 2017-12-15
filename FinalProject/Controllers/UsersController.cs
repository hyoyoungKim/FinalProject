using FinalProject.App_Code;
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            List<UsersModel> list = new List<UsersModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT UserID , Username, Password, FirstName, LastName, EmailAddress,
                                ContactNo,Image, Birthdate, DateCreated FROM Users";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new UsersModel
                            {
                                UserID = Convert.ToInt32(data["UserID"].ToString()),
                                Username = data["Username"].ToString(),
                                Password = data["Password"].ToString(),
                                FirstName = data["FirstName"].ToString(),
                                LastName = data["LastName"].ToString(),
                                EmailAddress = data["EmailAddress"].ToString(),
                                ContactNo = data["ContactNo"].ToString(),
                                Image = data["Image"].ToString(),
                                Birthdate = DateTime.Parse(data["Birthdate"].ToString()),
                                DateCreated = DateTime.Parse(data["DateCreated"].ToString())
                            });
                        }

                        return View(list);
                    }
                }
            }

        }

        //[HttpPost]
        //public ActionResult Add()
        //{
        //    UsersModel user = new UsersModel();

        //    return View(user);
        //}


        public ActionResult Add()
        {


            return View();
        }
        [HttpPost]
        public ActionResult Add(UsersModel user, HttpPostedFileBase image)
        {


            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO Users VALUES (@Username, @Password, @FirstName, @LastName, @EmailAddress,
                                @ContactNo,@Image,@Birthdate, @DateCreated) ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    string imageFile = user.Image;
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Firstname", user.FirstName);
                    cmd.Parameters.AddWithValue("@Lastname", user.LastName);
                    cmd.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                    cmd.Parameters.AddWithValue("@ContactNo", user.ContactNo);


                    double a;
                    string c = imageFile;



                    if (imageFile != null)
                    {
                        imageFile = DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName;
                        cmd.Parameters.AddWithValue("@Image", imageFile);
                        image.SaveAs(Server.MapPath("~/Images/Users/" + imageFile));
                    }
                    else
                    {
                        imageFile = "user-default.png";
                        cmd.Parameters.AddWithValue("@Image", imageFile);

                    }

                    cmd.Parameters.AddWithValue("@Birthdate", user.Birthdate);
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    UserLogin.UserID = user.UserID;
                    /*ADDED*/
                    UserLogin.FN = user.FirstName;
                    UserLogin.LN = user.LastName;

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("Prog3TeamFoodies@gmail.com");
                    mail.To.Add(user.EmailAddress);
                    mail.Subject = "Foodies Registration";
                    mail.Body = "Thank you " + user.LastName + ", " + user.FirstName + " for creating an account in Foodies.com. ";
                    mail.IsBodyHtml = true;

                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("Prog3TeamFoodies@gmail.com", "alec12345");
                    SmtpServer.Port = 587;
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);

                    return RedirectToAction("Index", "Recipe");

                }
            }
        }

        public ActionResult Details(int? UserID)
        {
            if (UserID == null)
            {
                return RedirectToAction("Index","Recipe");

            }
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT u.UserID, u.Username, u.Password, u.FirstName, u.LastName, u.EmailAddress,
                                u.ContactNo, u.Image, u.Birthdate, u.DateCreated, r.RecipeID, r.RecipeName,
                                d.Difficulty, cu.Cuisine, cr.MealCourse, m.MeatType, r.DateAdded
                                FROM Users u
                                INNER JOIN Recipes r ON  r.UserID= u.UserID
                                INNER JOIN Difficulty d ON r.DiffID = d.DiffID
                                INNER JOIN Cuisine cu ON r.CuisineID = cu.CuisineID
                                INNER JOIN Course cr ON r.CourseID = cr.CourseID
                                INNER JOIN Meat m ON r.MeatID = m.MeatID
                                WHERE u.UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            UsersModel user = new UsersModel();
                            while (data.Read())
                            {
                                user.UserID = Convert.ToInt32(data["UserID"].ToString());
                                user.Username = data["Username"].ToString();
                                user.Password = data["Password"].ToString();
                                user.FirstName = data["FirstName"].ToString();
                                user.LastName = data["LastName"].ToString();
                                user.EmailAddress = data["EmailAddress"].ToString();
                                user.ContactNo = data["ContactNo"].ToString();
                                user.Image = data["Image"].ToString();
                                user.Birthdate = DateTime.Parse(data["Birthdate"].ToString());
                                user.DateCreated = DateTime.Parse(data["DateCreated"].ToString());
                                user.RecipeID = Convert.ToInt32(data["RecipeID"].ToString());
                                user.RecipeName = data["RecipeName"].ToString();
                                user.Difficulty = data["Difficulty"].ToString();
                                user.Cuisine = data["Cuisine"].ToString();
                                user.MealCourse = data["MealCourse"].ToString();
                                user.MeatType = data["MeatType"].ToString();
                            }
                            return View(user);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
        }



        public List<CuisineModels> GetCuisines()
        {
            List<CuisineModels> list = new List<CuisineModels>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT c.CuisineID, c.Cuisine , (SELECT COUNT(r.CuisineID) FROM Recipes r WHERE r.CuisineID = c.CuisineID ) AS TotalCuisine
                                FROM Cuisine c ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            //ViewBag.TotalCuisine = data["TotalCuisine"].ToString();
                            list.Add(new CuisineModels
                            {
                                CuisineID = Convert.ToInt32(data["CuisineID"].ToString()),
                                Cuisine = data["Cuisine"].ToString(),
                                TotalCuisine = int.Parse(data["TotalCuisine"].ToString())
                            });
                        }

                        return list;
                    }
                }
            }
        }
        public List<CourseModels> GetCourses()
        {
            List<CourseModels> list = new List<CourseModels>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT CourseID, MealCourse , (SELECT COUNT(r.CourseID) FROM Recipes r WHERE r.CourseID = c.CourseID ) AS TotalCourse FROM Course c";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new CourseModels
                            {
                                CourseID = Convert.ToInt32(data["CourseID"].ToString()),
                                MealCourse = data["MealCourse"].ToString(),
                                TotalCourse = int.Parse(data["TotalCourse"].ToString())
                            });
                        }

                        return list;
                    }
                }
            }
        }
        public List<DifficultyModels> GetDifficulties()
        {
            List<DifficultyModels> list = new List<DifficultyModels>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT DiffID, Difficulty , (SELECT COUNT(r.DiffID) FROM Recipes r WHERE r.DiffID = d.DiffID ) AS TotalDiff  FROM Difficulty d ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new DifficultyModels
                            {
                                DiffID = Convert.ToInt32(data["DiffID"].ToString()),
                                Difficulty = Convert.ToInt32(data["Difficulty"].ToString()),
                                TotalDiff = Convert.ToInt32(data["TotalDiff"].ToString())

                            });
                        }

                        return list;
                    }
                }
            }
        }
        public List<MeatModels> GetMeat()
        {
            List<MeatModels> list = new List<MeatModels>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT MeatID, MeatType,(SELECT COUNT(r.MeatID) FROM Recipes r WHERE r.MeatID = m.MeatID ) AS TotalMeat FROM Meat m";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new MeatModels
                            {
                                MeatID = Convert.ToInt32(data["MeatID"].ToString()),
                                MeatType = data["MeatType"].ToString(),
                                TotalMeat = Convert.ToInt32(data["TotalMeat"].ToString())
                            });
                        }

                        return list;
                    }
                }
            }
        }


        public ActionResult LogIn()
        {
            //  ViewBag.Error = "<div class='alert alert-danger'>Invalid credentials!</div>";
            return View();

        }

        [HttpPost]
        public ActionResult LogIn(UsersModel user)
        {
            //return View();

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Username,UserID,FirstName,LastName, Password FROM Users WHERE Username = @Username
                    AND Password=@Password";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            UserLogin use = new UserLogin();
                         
                           // use.UserID = int.Parse(Session["userid"].ToString());
                            while (data.Read())
                            {
                                Session["username"] = data["username"].ToString();

                                UserLogin.UserID = int.Parse(data["userid"].ToString());
                /*ADDED*/       UserLogin.FN = data["FirstName"].ToString();
                                UserLogin.LN = data["LastName"].ToString();

                               
                            }
                            
                           
                            return RedirectToAction("Index", "Recipe");
                        }
                        else
                        {
                            ViewBag.Error = "<div class='alert alert-danger'>Invalid credentials!</div>";
                            return View();
                        }
                    }



                }
            }


        }

    }
}
