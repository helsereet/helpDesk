namespace UsersSupport.Data
{
    public class RequestTheme
    {
        public int Id { get; set; }

        [System.ComponentModel.DisplayName("Название темы заявки")]
        public string ThemeName { get; set; }
    }
}
