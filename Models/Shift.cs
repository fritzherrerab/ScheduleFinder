using System;
namespace ScheduleFinder.Models
{
    public class Shift
    {
        public string ShiftId { get; set; } = null!;
        public string Name { get; set; } = null!;
        // En un sistema real, esto podría ser TimeSpan o DateTime para las horas
        public TimeSpan DefaultStartTime { get; set; } 
        public TimeSpan DefaultEndTime { get; set; }
    }
}