using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Model.Employees;

namespace Core.Domain.Model
{
    public class Session
    {
        public const string DefaultPassword = "dEf@ult";
        public static User CurrentUser { get; set; }
    }
}
