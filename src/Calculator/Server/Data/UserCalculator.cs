using System;
using Calculator.Server.Models;

namespace Calculator.Server.Data
{
    public class UserCalculator
    {
        public UserCalculator()
        {
            EventTime = DateTimeOffset.UtcNow;
        }

        public UserCalculator(string action, ApplicationUser user) : this()
        {
            UserId = user.Id;
            UserName = user.UserName;
            Action = action;
        }

        public int Id { get; set; }

        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public DateTimeOffset EventTime { get; set; }
    }
}
