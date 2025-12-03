using System;

namespace ScheduleFinder.Models
{
    public class OverlapResult
    {
        public string EmployeeId { get; set; } = null!;
        public Schedule ScheduleA { get; set; } = null!;
        public Schedule ScheduleB { get; set; } = null!;
        public DateTime OverlapStart { get; set; }
        public DateTime OverlapEnd { get; set; }

        public override string ToString() => 
            $" Solape para {EmployeeId} | Periodo: {OverlapStart:HH:mm} - {OverlapEnd:HH:mm}\n" +
            $"   Turno A: {ScheduleA.StartDate:HH:mm} a {ScheduleA.EndDate:HH:mm}\n" +
            $"   Turno B: {ScheduleB.StartDate:HH:mm} a {ScheduleB.EndDate:HH:mm}";
    }
}