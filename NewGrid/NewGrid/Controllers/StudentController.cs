﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewGrid.Models;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;

namespace NewGrid.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetStudents(string sidx, string sort, int page, int rows)
        {
            Models.StudentContext db = new Models.StudentContext();
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var StudentList = db.Students.Select(
                    t => new
                    {
                        t.ID,
                        t.Name,
                        t.FatherName,
                        t.Gender,
                        t.ClassName,
                        t.DateOfAdmission
                    });
            int totalRecords = StudentList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                StudentList = StudentList.OrderByDescending(t => t.Name);
                StudentList = StudentList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                StudentList = StudentList.OrderBy(t => t.Name);
                StudentList = StudentList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = StudentList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public string Create([Bind(Exclude = "Id")] Student Model)
        {
            Models.StudentContext db = new Models.StudentContext();
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    Model.ID = Guid.NewGuid().ToString();
                    db.Students.Add(Model);
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





        public string Edit(Student Model)
        {
            Models.StudentContext db = new Models.StudentContext();
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
    }
}