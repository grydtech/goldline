namespace Core.Security
{
    public class User
    {
        public const string DefaultPassword = "dEf@ult";
        private static User _currentUser;

        public User(uint employeeId, UserType userType, string username, string password = DefaultPassword)
        {
            EmployeeId = employeeId;
            Username = username;
            Password = password;
            UserType = userType;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public User()
        {
        }

        /// <summary>
        ///     Stores the current user during a session. All payments and orders use this property
        /// </summary>
        /// private static User _currentUser;
        public static User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                UserPermissions.SetUserType(_currentUser.UserType);
            }
        }

        public uint EmployeeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }

        /// <summary>
        ///     Returns whether the current user password is the default password
        /// </summary>
        /// <returns></returns>
        public static bool IsDefaultPassword()
        {
            return CurrentUser?.Password == DefaultPassword;
        }
    }
}