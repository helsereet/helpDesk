namespace UsersSupport.Data
{
    public class User : Employee
    {
        public int CurrentRoleId { get; set; }

        public string CurrentRoleName { get; set; }

        public string GetFullName()
        {
            return $"{LastName} {FirstName} {SurName}";
        }
    }
}