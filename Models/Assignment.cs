using System;
namespace ScheduleFinder.Models
{
    public class Assignment
    {
        public string AssignmentId { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
        public string ShiftId { get; set; } = null!;
        public DateTime Date { get; set; } // Fecha específica de la asignación
        // Opcional: Para manejar excepciones al turno base
        public TimeSpan? CustomStartTime { get; set; } 
        public TimeSpan? CustomEndTime { get; set; } 
    }
}