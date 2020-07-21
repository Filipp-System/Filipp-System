using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator.Server.Models;

namespace Calculator.Server.Data
{
    public class UserFilippSystem
    {
        public UserFilippSystem()
        {
            EventTime = DateTimeOffset.UtcNow;
        }

        public UserFilippSystem(string action, ApplicationUser user) : this()
        {
            UserId = user.Id;
            Username = user.UserName;
            Action = action;
        }

        public int Id { get; set; }

        public string UserId { get; set; }
        public DateTimeOffset EventTime { get; set; }
        public string Action { get; set; }
        public string Username { get; set; }
    }
}
