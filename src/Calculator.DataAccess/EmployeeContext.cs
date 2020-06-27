using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Calculator.DataAccess
{
    public class EmployeeContext : DbContext, ISupportUser
    {

        public ClaimsPrincipal User { get; set; }
    }
}
