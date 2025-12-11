// Models/Schedule.cs

using System;
using ScheduleFinder.Models;

namespace ScheduleFinder.Models
{
    public class Schedule
    {
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // FACTORY METHOD para crear un Schedule desde una Assignment y su Shift base
        public static Schedule CreateFromAssignment(Assignment assignment, Shift shiftBase)
        {
            // Determinar las horas de inicio y fin (usando Custom si existe, sino Default)
            var start = assignment.CustomStartTime ?? shiftBase.DefaultStartTime;
            var end = assignment.CustomEndTime ?? shiftBase.DefaultEndTime;
            
            // Combinar la fecha de la asignación con la hora del turno
            var startDate = assignment.Date.Date.Add(start);
            var endDate = assignment.Date.Date.Add(end);
            
            // Manejar el caso CRÍTICO de turnos nocturnos (que terminan al día siguiente)
            if (endDate < startDate)
            {
                endDate = endDate.AddDays(1);
            }

            // Devolver el objeto Schedule completamente construido
            return new Schedule
            {
                EmployeeId = assignment.EmployeeId,
                StartDate = startDate,
                EndDate = endDate
            };
        }
    }
}