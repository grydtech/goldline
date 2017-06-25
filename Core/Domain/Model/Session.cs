using Core.Domain.Model.Employees;
using Core.Domain.Security;

namespace Core.Domain.Model
{
    public class Session
    {
        public const string DefaultPassword = "dEf@ult";
        public static User CurrentUser { get; set; }
        public static Clearance Clearance { get; set; }
    }
}
