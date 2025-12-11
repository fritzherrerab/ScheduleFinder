// Models/ScheduleExtensions.cs
using System;

namespace ScheduleFinder.Models
{
    // Una clase estática para métodos de extensión de Schedule
    public static class ScheduleExtensions
    {
        // Método encapsulado que comprueba si dos turnos solapan
        public static bool DoesOverlap(this Schedule scheduleA, Schedule scheduleB)
        {
            // Usamos la lógica estándar de intervalo: A comienza antes de que B termine,
            // Y B comienza antes de que A termine.
            return scheduleA.StartDate < scheduleB.EndDate && 
                   scheduleB.StartDate < scheduleA.EndDate;
        }
    }
}