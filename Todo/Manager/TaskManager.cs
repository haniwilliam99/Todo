using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Todo.Helper;
using Todo.Models;

namespace Todo.Manager
{
    public class TaskManager
    {

        public static List<Tasks> GetTasks( string length, string start, string sortColumn, string sortColumnDir, string searchValue, ref int recordsTotal, string currentUserId)
        {
            List<Tasks> res = new List<Tasks>();

            try
            {
                using (ApplicationDbContext _context = new ApplicationDbContext())
                {
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                 


                    // var user = idManager.FindByID(User.Identity.GetUserId());

                    // Getting all Customer data    
                  
                    var customerData = _context.Tasks.Where(o => o.TaskUser.Id == currentUserId);

                    ////Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        System.Reflection.PropertyInfo prop = typeof(Tasks).GetProperty(sortColumn);
                        if (sortColumnDir == "asc")
                        {
                            customerData = customerData.OrderBy(o => o.Title);
                        }
                        else
                        {
                            customerData = customerData.OrderByDescending(o => o.Title);
                        }
                    }
                    //Search    
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        customerData = customerData.Where(m => m.Title.Contains(searchValue));
                    }

                    //total number of rows count     
                    recordsTotal = customerData.Count();
                    //Paging     
                    res = customerData.Skip(skip).Take(pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                res = new List<Tasks>();
            }
            return res;
            //Returning Json Data    
        }
    
    
        public static Tasks GetByID(int Id)
        {
            Tasks task = null;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                var obj = _context.Tasks.FirstOrDefault(o => o.ID == Id);
                if (obj != null)
                {
                  
                    task = obj;
                }

            }
            return task;
        }

        public static ResponseDTO<Tasks> AddEdit( TaskViewModel model, string userId)
        {
            ResponseDTO<Tasks> response = new ResponseDTO<Tasks>();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (model.Id > 0)
                {
                    var obj = _context.Tasks.FirstOrDefault(o => o.ID == model.Id);
                    if (obj != null)
                    {
                        obj.Title = model.Title;
                        obj.DueDate = model.DueDate;

                    }
                }
                else
                {
                    Tasks tasks = new Tasks();
                    tasks.Title = model.Title;
                    tasks.DueDate = model.DueDate;
                    
                    ApplicationUser currentUser = _context.Users.FirstOrDefault(x => x.Id == userId);
                    tasks.TaskUser = currentUser;
                    tasks.Status = 1;
                    _context.Tasks.Add(tasks);
                }

                int res = _context.SaveChanges();
                if (res > 0)
                {
                    response.Code = StatusCode.OKAY;
                    response.Message = "Save successfully";
                }
                else
                {
                    response.Code = StatusCode.Error;
                    response.Message = "Something went wrong!!";
                }

            }
            return response;
        }
        public static ResponseDTO<Tasks> MarkComplete(int Id)
        {
            ResponseDTO<Tasks> response = new ResponseDTO<Tasks>();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {

                var obj = _context.Tasks.FirstOrDefault(o => o.ID == Id);
                if (obj != null)
                {
                    obj.Status = 2;

                }

                int res = _context.SaveChanges();
                if (res > 0)
                {
                    response.Code = StatusCode.OKAY;
                    response.Message = "Save successfully";
                }
                else
                {
                    response.Code = StatusCode.Error;
                    response.Message = "Something went wrong!!";
                }

            }
            return response;
        }


        public static ResponseDTO<Tasks> Delete(int Id)
        {
            ResponseDTO<Tasks> response = new ResponseDTO<Tasks>();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {

                var task = _context.Tasks.FirstOrDefault(o => o.ID == Id);

                if (task != null)
                {
                    _context.Tasks.Remove(task);
                    int res = _context.SaveChanges();
                    if (res > 0)
                    {
                        response.Code = StatusCode.OKAY;
                        response.Message = "Deleted successfully";
                    }
                    else
                    {
                        response.Code = StatusCode.Error;
                        response.Message = "Something went wrong!!";
                    }
                }


                else
                {
                    response.Code = StatusCode.Error;
                    response.Message = "Task not found !!";
                }


            }
            return response;
        }

    }
}