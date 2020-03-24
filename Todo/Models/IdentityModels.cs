using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity;
using System;

namespace Todo.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }


    public class Tasks
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }

        public DateTime DueDate { get; set; }

        public ApplicationUser TaskUser { get; set; }


    }

   



}