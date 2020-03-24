using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Todo.Helper
{
    public class ResponseDTO<T>
    {
        public StatusCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

    }
    public enum StatusCode
    {
      
        OKAY = 0,

    
        Error = 2,

        AccessDenied = 3,

        DatabaseError = 4,

    }
}

