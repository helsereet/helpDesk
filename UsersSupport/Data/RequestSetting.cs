namespace UsersSupport.Data
{
    public class RequestSetting
    {
        public int Id { get; set; }

        public string RequestTypeId { get; set; }

        public string RequestThemeId { get; set; }

        [System.ComponentModel.DisplayName("Тип заявки")]
        public string TypeName { get; set; }

        [System.ComponentModel.DisplayName("Тема заявки")]
        public string ThemeName { get; set; }

        [System.ComponentModel.DisplayName("Описание")]
        public string Description { get; set; }

        public long? NormExecutionTimeInSeconds { get; set; }

        [System.ComponentModel.DisplayName("Норма выполнения")]
        public string NormExecutionTimeReadable { get; set; }
    }
}
