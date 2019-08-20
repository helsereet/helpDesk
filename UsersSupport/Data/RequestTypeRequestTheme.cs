namespace UsersSupport.Data
{
    public class RequestTypeRequestTheme
    {
        public int Id { get; set; }

        public int RequestThemeId { get; set; }

        public int RequestTypeId { get; set; }

        public string ThemeName { get; set; }

        public string Description { get; set; }
    }
}
