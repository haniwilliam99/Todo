using Todo.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Todo.Helper;
using Todo.Manager;


namespace Todo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

      

        public ActionResult LoadData()
        {
            try
            {
                //Creating instance of DatabaseContext class  

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int recordsTotal = 0;
                string currentUserId = User.Identity.GetUserId();
                var data = TaskManager.GetTasks(length, start, sortColumn, sortColumnDir, searchValue, ref recordsTotal, currentUserId);

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult Get(int Id)
        {
            ResponseDTO<Tasks> response = new ResponseDTO<Tasks>();
            try
            {
              var task=  TaskManager.GetByID(Id);
                if(task !=null)
                {
                    response.Code = StatusCode.OKAY;
                    response.Data = task;

                }
                else
                {

                    response.Code = StatusCode.Error;
                    response.Data = null;
                }
              
            }
            catch (Exception ex)
            {
                response.Code = StatusCode.Error;
                response.Data = null;
            }
            return Json(response);
        }
        public ActionResult Edit(TaskViewModel model)
        {
            ResponseDTO<Tasks> response = new ResponseDTO<Tasks>();
            try
            {
               string userId = User.Identity.GetUserId();
                response = TaskManager.AddEdit(model, userId);
            }
            catch (Exception ex)
            {
                response.Code = StatusCode.Error;
                TempData["Error"] = "Something went wrong!!";
            }
            return Json(response);
        }

        public ActionResult MarkComplete(int Id)
        {
            ResponseDTO<Tasks> response = new ResponseDTO<Tasks>();
            try
            {
                response = TaskManager.MarkComplete(Id);
                if(response.Code==StatusCode.Error)
                {
                    TempData["Error"] = response.Message;
                }
                else
                {
                    TempData["Success"] = response.Message;
                }
            }
            catch (Exception ex)
            {
                response.Code = StatusCode.Error;
                TempData["Error"] = "Something went wrong!!";
            }
            return Json(response);
        }


        public ActionResult Delete(int Id)
        {
            ResponseDTO<Tasks> response = new ResponseDTO<Tasks>();
            try
            {

                response = TaskManager.Delete(Id);
                if (response.Code == StatusCode.Error)
                {
                    TempData["Error"] = response.Message;
                }
                else
                {
                    TempData["Success"] = response.Message;
                }
            }
            catch (Exception ex)
            {
                response.Code = StatusCode.Error;
                TempData["Error"] = "Something went wrong!!";
            }

            return Json(response);
        }

    }


}
