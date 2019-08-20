namespace UsersSupport.Data
{
    public class Employee
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public int PositionId { get; set; }

        public int RoleId { get; set; }

        public string Password { get; set; }

        [System.ComponentModel.DisplayName("Имя")]
        public string FirstName { get; set; }

        [System.ComponentModel.DisplayName("Фамилия")]
        public string LastName { get; set; }

        [System.ComponentModel.DisplayName("Отчество")]
        public string SurName { get; set; }

        [System.ComponentModel.DisplayName("Отдел")]
        public string DepartmentName { get; set; }

        [System.ComponentModel.DisplayName("Должность")]
        public string PositionName { get; set; }

        [System.ComponentModel.DisplayName("Роль")]
        public string RoleName { get; set; }

        [System.ComponentModel.DisplayName("Логин")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [System.ComponentModel.DisplayName("Телефон")]
        public string PhoneNumber { get; set; }

        [System.ComponentModel.DisplayName("Комната")]
        public string RoomNumber { get; set; }

        [System.ComponentModel.DisplayName("Адрес")]
        public string Address { get; set; }

        [System.ComponentModel.DisplayName("Работает")]
        public bool WorkingState { get; set; }

    }
}
