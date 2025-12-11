// Services/OverlapFinderService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleFinder.Models; 

namespace ScheduleFinder.Services
{
    // Clase estática para la lógica central (Service/Business Logic)
    public static class OverlapFinder
    {
        public static List<OverlapResult> FindOverlaps(List<Schedule> allSchedules)
        {
            var overlaps = new List<OverlapResult>();

            // 1. AGRUPAR por EmployeeId
            var schedulesByEmployee = allSchedules.GroupBy(s => s.EmployeeId);

            // 2. ITERAR: Recorrer cada grupo de empleados
            foreach (var group in schedulesByEmployee)
            {
                var employeeSchedules = group.ToList();

                // 3. OPTIMIZACIÓN: Ordenar por hora de inicio.
                var sortedSchedules = employeeSchedules.OrderBy(s => s.StartDate).ToList();

                // 4. BUCLE SIMPLE: Recorrer y comparar cada turno con su predecesor inmediato.
                for (int i = 1; i < sortedSchedules.Count; i++)
                {
                    var scheduleB = sortedSchedules[i];     
                    var scheduleA = sortedSchedules[i - 1]; 

                    // 5. USAR MÉTODO ENCAPSULADO (DoesOverlap)
                    if (scheduleA.DoesOverlap(scheduleB)) 
                    {
                        var overlapStart = Max(scheduleA.StartDate, scheduleB.StartDate);
                        var overlapEnd = Min(scheduleA.EndDate, scheduleB.EndDate);

                        overlaps.Add(new OverlapResult
                        {
                            EmployeeId = group.Key,
                            ScheduleA = scheduleA,
                            ScheduleB = scheduleB,
                            OverlapStart = overlapStart,
                            OverlapEnd = overlapEnd
                        });
                    }
                }
            }
            
            return overlaps;
        }

        // Métodos auxiliares privados
        private static DateTime Max(DateTime a, DateTime b) => a > b ? a : b;
        private static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;
    }
}