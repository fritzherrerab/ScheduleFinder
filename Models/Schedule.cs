using System;
namespace ScheduleFinder.Models
{
    public class Schedule
    {
        public string EmployeeId { get; set; }  = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Método para imprimir el turno de forma legible
        public override string ToString() => 
            $"{EmployeeId}: {StartDate:HH:mm} - {EndDate:HH:mm}";
    }
}