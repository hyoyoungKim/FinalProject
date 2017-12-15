using FinalProject.App_Code;
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace FinalProject.Controllers
{
    public class RecipeController : Controller
    {
        // GET: Recipe
        public ActionResult Index()
        {
            RecipeViewModels rvm = new RecipeViewModels();
            rvm.AllRecipes = GetRecipesIndex();
            rvm.AllCategories = GetCategories2();
            rvm.AllCuisines = GetCuisines();
            rvm.AllCourses = GetCourses();
            rvm.AllDifficulties = GetDifficulties();
            rvm.AllMeat = GetMeat();

            ViewBag.User = UserLogin.UserID;
            rvm.User = UserLogin.UserID;
            ViewBag.FN = UserLogin.FN;
            ViewBag.LN = UserLogin.LN;


            return View(rvm);
        }


        public ActionResult Add()
        {
            RecipesModels recipe = new RecipesModels();
            recipe.Cuisines = GetCuisinesAdd();
            recipe.Courses = GetCoursesAdd();
            recipe.Meats = GetMeatsAdd();
            recipe.Difficulties = GetDifficultiesAdd();
            return View(recipe);
        }

        public ActionResult Edit(int? id)
        {


            if (id == null)
            {
                return RedirectToAction("Index");
            }
            
            List<RecipesModels> list = new List<RecipesModels>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT r.RecipeName, r.Description, r.Ingredients, r.Instructions, d.Difficulty,
                                 cu.Cuisine, cr.MealCourse, m.MeatType, u.UserID, r.image, r.DateAdded, r.DateModified 
                                 FROM Recipes r
                                    INNER JOIN Difficulty d ON d.DiffID = r.DiffID
                                    INNER JOIN Cuisine cu ON cu.CuisineID = r.CuisineID
                                    INNER JOIN Course cr ON cr.CourseID = r.CourseID
                                    INNER JOIN Meat m ON m.MeatID = r.MeatID
                                    INNER JOIN Users u ON u.UserID = r.UserID 
									WHERE r.RecipeID = @RecipeID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RecipeID", id);
                                using (SqlDataReader data = cmd.ExecuteReader())
                                {
                                    while (data.Read())
                                    {
                                        list.Add(new RecipesModels
                                        {
                                            RecipeName = data["RecipeName"].ToString(),
                                            Description = data["Description"].ToString(),
                                            Ingredients = data["Ingredients"].ToString(),
                                            Instructions = data["Instructions"].ToString(),
                                            Difficulty = Convert.ToInt32(data["Difficulty"].ToString()),
                                            MealCourse = data["MealCourse"].ToString(),
                                            MeatType = data["MeatType"].ToString(),
                                            FirstName = data["FirstName"].ToString(),
                                            Lastname = data["LastName"].ToString(),
                                            Image = data["Image"].ToString(),
                                            DateAdded = DateTime.Parse(data["DateAdded"].ToString()),
                                            DateModified = DateTime.Parse(data["DateModified"].ToString()),
                                        });
                                    }

                                    return View(list);
                                }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(RecipesModels recipe, UsersModel user, HttpPostedFileBase image)
        {

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"UPDATE Recipes SET RecipeName=@RecipeName, Description=@Description, Ingredients=@Ingredients, Instructions=@Instructions, DiffID=@DiffID,
                                 CuisineID=@CuisineID, CourseID=@CourseID, MeatID=@MeatID, @UserID, image=@image, @DateAdded, @DateModified) ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    string imageFile = recipe.Image;
                    cmd.Parameters.AddWithValue("@RecipeName", recipe.RecipeName);
                    cmd.Parameters.AddWithValue("@Description", recipe.Description);
                    cmd.Parameters.AddWithValue("@Ingredients", recipe.Ingredients);
                    cmd.Parameters.AddWithValue("@Instructions", recipe.Instructions);
                    cmd.Parameters.AddWithValue("@DiffID", recipe.DiffID);
                    cmd.Parameters.AddWithValue("@CuisineID", recipe.CuisineID);
                    cmd.Parameters.AddWithValue("@CourseID", recipe.CourseID);
                    cmd.Parameters.AddWithValue("@MeatID", recipe.MeatID);
                    cmd.Parameters.AddWithValue("@UserID", UserLogin.UserID);
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);


                    double a;
                    string c = imageFile;

                    if (imageFile != null)
                    {
                        imageFile = DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName;
                        cmd.Parameters.AddWithValue("@image", imageFile);
                        image.SaveAs(Server.MapPath("~/Images/Users/" + imageFile));
                    }
                    else
                    {
                        imageFile = "user-default.png";
                        cmd.Parameters.AddWithValue("@image", imageFile);

                    }
                    cmd.ExecuteNonQuery();

                    return RedirectToAction("Index", "Recipe");

                }
            }
        }

        [HttpPost]
        public ActionResult Add(RecipesModels recipe, UsersModel user, HttpPostedFileBase image)
        {


            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO Recipes VALUES (@RecipeName, @Description, @Ingredients, @Instructions, @DiffID,
                                @CuisineID, @CourseID ,@MeatID,@UserID, @image,@DateAdded,@DateModified) ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    string imageFile = recipe.Image;
                    cmd.Parameters.AddWithValue("@RecipeName", recipe.RecipeName);
                    cmd.Parameters.AddWithValue("@Description", recipe.Description);
                    cmd.Parameters.AddWithValue("@Ingredients", recipe.Ingredients);
                    cmd.Parameters.AddWithValue("@Instructions", recipe.Instructions);
                    cmd.Parameters.AddWithValue("@DiffID", recipe.DiffID);
                    cmd.Parameters.AddWithValue("@CuisineID", recipe.CuisineID);
                    cmd.Parameters.AddWithValue("@CourseID", recipe.CourseID);
                    cmd.Parameters.AddWithValue("@MeatID", recipe.MeatID);
                    cmd.Parameters.AddWithValue("@UserID", UserLogin.UserID);

                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);


                    double a;
                    string c = imageFile;

                    if (imageFile != null)
                    {
                        imageFile = DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName;
                        cmd.Parameters.AddWithValue("@image", imageFile);
                        image.SaveAs(Server.MapPath("~/Images/Users/" + imageFile));
                    }
                    else
                    {
                        imageFile = "user-default.png";
                        cmd.Parameters.AddWithValue("@image", imageFile);

                    }
                    cmd.ExecuteNonQuery();

                    return RedirectToAction("Index", "Recipe");

                }
            }
        }



        //public List<UsersModel> GetUserDetails()
        //{
        //    List<UsersModel> list = new List<UsersModel>();
        //    using (SqlConnection con = new SqlConnection(Helper.GetCon()))
        //    {
        //        con.Open();
        //        string query = @"SELECT u.UserID, u.FirstName, u.LastName, r.Recipe
        //                         FROM User u
        //                        INNER JOIN Recipes r ON u.RecipeID = r.RecipeID";
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            using (SqlDataReader data = cmd.ExecuteReader())
        //            {
        //                while (data.Read())
        //                {
        //                    list.Add(new UsersModel
        //                    {
        //                        //RecipeID = Convert.ToInt32(data["RecipeID"].ToString()),
        //                        //RecipeName = data["RecipeName"].ToString(),
        //                        //Description = data["Description"].ToString(),
        //                        //Ingredients = data["Ingredients"].ToString(),
        //                        //Instructions = data["Instructions"].ToString(),
        //                        //Category = data["Category"].ToString(),
        //                        //Difficulty = Convert.ToInt32(data["Difficulty"].ToString()),
        //                        //Country = data["Country"].ToString(),
        //                        //MealCourse = data["MealCourse"].ToString(),
        //                        //MeatType = data["MeatType"].ToString(),
        //                        //FirstName = data["FirstName"].ToString(),
        //                        //Lastname = data["LastName"].ToString(),
        //                        //Image = data["Image"].ToString(),
        //                        //DateAdded = DateTime.Parse(data["DateAdded"].ToString()),
        //                        //DateModified = DateTime.Parse(data["DateModified"].ToString()),
        //                    });
        //                }

        //                return list;
        //            }
        //        }
        //    }
        //}

        /* SELECT u.UserID, u.Username, u.Password, u.FirstName, u.LastName, u.EmailAddress
                                u.ContactNo, u.Image, u.Birthdate, u.DateCreated
                                FROM Users u
                                WHERE UserID = @UserID */

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
        public List<CategoryModels> GetCategories2()
        {
            List<CategoryModels> list = new List<CategoryModels>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT c.CatID, c.Category, cu.Cuisine, d.Difficulty, cr.MealCourse, m.MeatType 
                                FROM Category c
                               INNER JOIN Cuisine cu ON c.CuisineID = cu.CuisineID
                               INNER JOIN Difficulty d ON c.DiffID = d.DiffID
                               INNER JOIN Course cr ON c.CourseID = cr.CourseID
                               INNER JOIN Meat m ON c.MeatID = m.MeatID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new CategoryModels
                            {
                                CatID = Convert.ToInt32(data["CatID"].ToString()),
                                Category = data["Category"].ToString(),
                                Difficulty = Convert.ToInt32(data["Difficulty"].ToString()),
                                Cuisine = data["Cuisine"].ToString(),
                                MealCourse = data["MealCourse"].ToString(),
                                MeatType = data["MeatType"].ToString()
                            });
                        }

                        return list;
                    }
                }
            }
        }
        public List<RecipesModels> GetRecipesDetails()
        {
            List<RecipesModels> list = new List<RecipesModels>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT r.RecipeID, r.RecipeName, r.Description, r.Ingredients, r.Instructions, 
                                d.Difficulty, cu.Cuisine, cr.MealCourse, m.MeatType, cu.Cuisine, u.FirstName, u.LastName ,r.Image, r.DateAdded, r.DateModified
                                FROM Recipes r
                                INNER JOIN Difficulty d ON r.DiffID = d.DiffID
                                INNER JOIN Cuisine cu ON r.CountryID = cu.CuisineID
                                INNER JOIN Course cr ON r.CourseID = cr.CourseID
                                INNER JOIN Meat m ON r.MeatID = m.MeatID
                                INNER JOIN Cuisine cu ON r.CuisineID = cu.CuisineID
                                INNER JOIN Users u ON r.UserID = u.userID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new RecipesModels
                            {
                                RecipeID = Convert.ToInt32(data["RecipeID"].ToString()),
                                RecipeName = data["RecipeName"].ToString(),
                                Description = data["Description"].ToString(),
                                Ingredients = data["Ingredients"].ToString(),
                                Instructions = data["Instructions"].ToString(),
                                Difficulty = Convert.ToInt32(data["Difficulty"].ToString()),
                                MealCourse = data["MealCourse"].ToString(),
                                MeatType = data["MeatType"].ToString(),
                                FirstName = data["FirstName"].ToString(),
                                Lastname = data["LastName"].ToString(),
                                Image = data["Image"].ToString(),
                                DateAdded = DateTime.Parse(data["DateAdded"].ToString()),
                                DateModified = DateTime.Parse(data["DateModified"].ToString()),
                            });
                        }

                        return list;
                    }
                }
            }
        }
        public List<RecipesModels> GetRecipesIndex()
        {
            List<RecipesModels> list = new List<RecipesModels>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT r.RecipeID, r.RecipeName, r.Description, r.Ingredients, r.Instructions, r.UserID, 
                                d.Difficulty, cu.Cuisine, cr.MealCourse, m.MeatType, u.FirstName, 
                                u.LastName, r.Image, r.DateAdded, r.DateModified
                                FROM Recipes r
                                INNER JOIN Difficulty d ON r.DiffID = d.DiffID
                                INNER JOIN Cuisine cu ON r.CuisineID = cu.CuisineID
                                INNER JOIN Course cr ON r.CourseID = cr.CourseID
                                INNER JOIN Meat m ON r.MeatID = m.MeatID
                                INNER JOIN Users u ON r.UserID = u.userID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new RecipesModels
                            {
                                RecipeID = Convert.ToInt32(data["RecipeID"].ToString()),
                                RecipeName = data["RecipeName"].ToString(),
                                Description = data["Description"].ToString(),
                                Difficulty = Convert.ToInt32(data["Difficulty"].ToString()),
                                MealCourse = data["MealCourse"].ToString(),
                                MeatType = data["MeatType"].ToString(),
                                /*ADDED*/
                                Cuisine = data["Cuisine"].ToString(),
                                UserID = Convert.ToInt32(data["UserID"].ToString()),
                                FirstName = data["FirstName"].ToString(),
                                Lastname = data["LastName"].ToString(),
                                Image = data["Image"].ToString(),
                                DateAdded = DateTime.Parse(data["DateAdded"].ToString()),
                                DateModified = DateTime.Parse(data["DateModified"].ToString()),
                            });
                        }

                        return list;
                    }
                }
            }
        }

        public List<SelectListItem> GetCuisinesAdd()
        {
            List<SelectListItem> list = new List<SelectListItem>();
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
                            list.Add(new SelectListItem
                            {
                                Text = data["Cuisine"].ToString(),
                                Value = data["CuisineID"].ToString()
                            });
                        }

                        return list;
                    }
                }
            }
        }
        public List<SelectListItem> GetCoursesAdd()
        {
            List<SelectListItem> list = new List<SelectListItem>();
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
                            //ViewBag.TotalCuisine = data["TotalCuisine"].ToString();
                            list.Add(new SelectListItem
                            {
                                Text = data["MealCourse"].ToString(),
                                Value = data["CourseID"].ToString()
                            });
                        }

                        return list;
                    }
                }
            }
        }
        public List<SelectListItem> GetMeatsAdd()
        {
            List<SelectListItem> list = new List<SelectListItem>();
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
                            //ViewBag.TotalCuisine = data["TotalCuisine"].ToString();
                            list.Add(new SelectListItem
                            {
                                Text = data["MeatType"].ToString(),
                                Value = data["MeatID"].ToString()
                            });
                        }

                        return list;
                    }
                }
            }
        }
        public List<SelectListItem> GetDifficultiesAdd()
        {
            List<SelectListItem> list = new List<SelectListItem>();
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
                            //ViewBag.TotalCuisine = data["TotalCuisine"].ToString();
                            list.Add(new SelectListItem
                            {
                                Text = data["Difficulty"].ToString(),
                                Value = data["DiffID"].ToString()
                            });
                        }

                        return list;
                    }
                }
            }
        }

        public ActionResult Details(int? RecipeID)
        {
            if (RecipeID == null)
            {
                return RedirectToAction("Index");

            }

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                /*Added*/
                string query = @"SELECT r.RecipeID, r.RecipeName, r.Description, r.Ingredients, r.Instructions, 
                        d.Difficulty, c.Cuisine , cr.MealCourse, m.MeatType, u.FirstName, u.LastName , r.Image, r.DateAdded, r.DateModified
                                   FROM Recipes r
                                INNER JOIN Cuisine c ON r.CuisineID= c.CuisineID
                                INNER JOIN Difficulty d ON r.DiffID = d.DiffID
                                INNER JOIN Course cr ON r.CourseID = cr.CourseID
                                INNER JOIN Meat m ON r.MeatID = m.MeatID
                                INNER JOIN Users u ON r.UserID = u.userID WHERE r.RecipeID = @RecipeID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RecipeID", RecipeID);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            RecipesModels recipe = new RecipesModels();
                            while (data.Read())
                            {
                                recipe.RecipeID = Convert.ToInt32(data["RecipeID"].ToString());
                                recipe.RecipeName = data["RecipeName"].ToString();
                                recipe.MealCourse = data["MealCourse"].ToString();
                                recipe.MeatType = data["MeatType"].ToString();
                                recipe.Difficulty = int.Parse(data["Difficulty"].ToString());
                                recipe.FirstName = data["FirstName"].ToString();
                                recipe.Lastname = data["LastName"].ToString();
                                recipe.Cuisine = data["Cuisine"].ToString();

                                recipe.Description = data["Description"].ToString();
                                recipe.Ingredients = data["Ingredients"].ToString();
                                recipe.Instructions = data["Instructions"].ToString();
                                recipe.Difficulty = Convert.ToInt32(data["Difficulty"].ToString());
                                recipe.DateAdded = DateTime.Parse(data["DateAdded"].ToString());
                                recipe.DateModified = DateTime.Parse(data["DateModified"].ToString());
                                recipe.Image = data["Image"].ToString();
                            }
                            return View(recipe);
                            // ViewBag.Pic = " <a href='Index'><img src='../Images/foodies2.png' width='125'/></a>";
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
        }

            public ActionResult GenerateReport(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("~/Reports/rptRecipe.rpt"));
            rd.SetDatabaseLogon("sa", "benilde", "DESKTOP-8HE1LD0", "FinalProj");
            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, true, "Recipe #" + id.ToString() + " Report");
            return View();
        }

    }

    }
