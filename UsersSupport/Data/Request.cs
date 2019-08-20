using System;

namespace UsersSupport.Data
{
    public class Request
    {
        public int Id { get; set; }

        [System.ComponentModel.DisplayName("Тип заявки")]
        public string TypeName { get; set; }

        [System.ComponentModel.DisplayName("Тема заявки")]
        public string ThemeName { get; set; }

        public string Description { get; set; }

        [System.ComponentModel.DisplayName("Заказчик")]
        public string CustomerFullName { get; set; }

        [System.ComponentModel.DisplayName("Исполнитель")]
        public string PerformerFullName { get; set; }

        public int RequestTypeRequestThemeId { get; set; }

        [System.ComponentModel.DisplayName("Краткое описание")]
        public string ShortDescription { get; set; }

        [System.ComponentModel.DisplayName("Полное описание")]
        public string FullDescription { get; set; }

        [System.ComponentModel.DisplayName("Дата создания")]
        public DateTime? CreatedAt { get; set; }

        public int CreatedByEmployeeId { get; set; }

        [System.ComponentModel.DisplayName("В работе")]
        public bool Taken { get; set; }

        [System.ComponentModel.DisplayName("Дата принятия в работу")]
        public DateTime? TakenAt { get; set; }

        public int? LastTakenByEmployeeId { get; set; }

        [System.ComponentModel.DisplayName("Решена")]
        public bool Solved { get; set; }

        [System.ComponentModel.DisplayName("Дата решения")]
        public DateTime? SolvedAt { get; set; }

        public int? SolvedByEmployeeId { get; set; }

        [System.ComponentModel.DisplayName("Решение")]
        public string Solution { get; set; }

        [System.ComponentModel.DisplayName("Закрыта")]
        public bool Closed { get; set; }

        [System.ComponentModel.DisplayName("Дата закрытия")]
        public DateTime? ClosedAt { get; set; }

        [System.ComponentModel.DisplayName("Оценка заказчика")]
        public byte? CustomerMark { get; set; }

        [System.ComponentModel.DisplayName("Оценка исполнителя")]
        public byte? PerformerMark { get; set; }

        [System.ComponentModel.DisplayName("Время, потраченное на решение")]
        public long? SpentOnSolutionInSeconds { get; set; }

        public long? NormExecutionTimeInSeconds { get; set; }
    }
}
