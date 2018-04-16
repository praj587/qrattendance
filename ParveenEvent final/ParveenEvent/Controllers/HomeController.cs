using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParveenEvent.Models;
using System.IO;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Data.Entity;

namespace ParveenEvent.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "DashBoard";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [ValidateInput(true)]
        [HttpPost]
        public ActionResult Create1(tbl_Reg regdata)
        {
            if (ModelState.IsValid)
            {
                //var Mobileno = Convert.ToString(Request["txtAmount"].ToString());
                using (var context = new regmodelcontext())
                {
                    var user = context.regusers.Where(u => u.Cus_Id == regdata.Cus_Id).FirstOrDefault();

                    // regdata.Cus_Id = Convert.ToInt16(context.regusers.Max(x => x.Cus_Id)) + 1;
                    if (user == null)
                    {

                        context.regusers.Add(regdata);
                        context.SaveChanges();
                        ModelState.Clear();
                        // return View("Success");
                        ViewBag.successMessage = "Success";
                    }
                    else
                    {


                        user.Cus_Name = regdata.Cus_Name;
                        user.Cus_Mobile = regdata.Cus_Name;
                        user.Cus_Company = regdata.Cus_Name;
                        user.Cus_Occupation = regdata.Cus_Name;

                        // etc
                        context.Entry(user).State = EntityState.Modified;
                        context.SaveChanges();
                        ModelState.Clear();
                        // return View("Success");
                        // return RedirectToAction("Success", new { Qrimput = regdata.Cus_Mobile });
                        ViewBag.successMessage = "Success";
                    }
                }
            }
            return View("Register");
        }




        public JsonResult GetUsers(string sidx, string sort, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            regmodelcontext db = new regmodelcontext();
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var UserList = db.regusers.Select(
                    t => new
                    {
                        t.Cus_Id,
                        t.Cus_Name,
                        t.Cus_Occupation,
                        t.Cus_Mobile,
                        t.Cus_Company
                    });
            if (_search)
            {
                switch (searchField)
                {
                    //case "Cus_Id":
                    //    UserList = UserList.Where(t => t.Cus_Id.Contains(searchString));
                    //    break;
                    case "Cus_Name":
                        UserList = UserList.Where(t => t.Cus_Name.Contains(searchString));
                        break;
                    case "Cus_Occupation":
                        UserList = UserList.Where(t => t.Cus_Occupation.Contains(searchString));
                        break;
                    case "Cus_Mobile":
                        UserList = UserList.Where(t => t.Cus_Mobile.Contains(searchString));
                        break;
                    case "Cus_Company":
                        UserList = UserList.Where(t => t.Cus_Company.Contains(searchString));
                        break;
                }
            }
            int totalRecords = UserList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                UserList = UserList.OrderByDescending(t => t.Cus_Name);
                UserList = UserList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                UserList = UserList.OrderBy(t => t.Cus_Name);
                UserList = UserList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = UserList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Create([Bind(Exclude = "Id")] tbl_Reg Model)
        {
            regmodelcontext db = new regmodelcontext();
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    Model.Cus_Id = 1;// Guid.NewGuid().ToString();
                    db.regusers.Add(Model);
                    db.SaveChanges();
                    msg = "Saved Successfully";
                }
                else
                {
                    msg = "Validation data not successfully";
                }
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
            }
            return msg;
        }
        public string Edit(tbl_Reg Model)
        {
            regmodelcontext db = new regmodelcontext();
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(Model).State = EntityState.Modified;
                    db.SaveChanges();
                    msg = "Saved Successfully";
                }
                else
                {
                    msg = "Validation data not successfully";
                }
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
            }
            return msg;
        }
        public string Delete(string Id)
        {
            regmodelcontext db = new regmodelcontext();
            tbl_Reg User = db.regusers.Find(Id);
            db.regusers.Remove(User);
            db.SaveChanges();
            return "Deleted successfully";
        }

    }


}
