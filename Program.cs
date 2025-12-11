using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using ScheduleFinder.Data;
using ScheduleFinder.Models;

// Configuración inicial
Console.WriteLine("Cargando datos desde data.json...");

try
{
    // 1. Cargar el JSON
    string jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "data.json");
    if (!File.Exists(jsonPath))
    {
        Console.WriteLine($"ERROR: Archivo 'data.json' no encontrado en: {jsonPath}");
        return;
    }
    
    string jsonString = File.ReadAllText(jsonPath);
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var data = JsonSerializer.Deserialize<DataContainer>(jsonString, options);

    if (data == null || !data.Assignments.Any())
    {
        Console.WriteLine("No se pudo cargar la data o no hay asignaciones.");
        return;
    }

    // 2. Transformar Asignaciones en Schedules
    Console.WriteLine($"Procesando {data.Assignments.Count} asignaciones...");

    var allSchedules = data.Assignments
        .Select(assignment =>
        {
            // Encontrar la definición del turno base
            var shiftBase = data.Shifts.FirstOrDefault(s => s.ShiftId == assignment.ShiftId);
            if (shiftBase == null) return null;

            // Determinar las horas de inicio y fin (usando Custom si existe, sino Default)
            var start = assignment.CustomStartTime ?? shiftBase.DefaultStartTime;
            var end = assignment.CustomEndTime ?? shiftBase.DefaultEndTime;
            
            // Combinar la fecha de la asignación con la hora del turno
            var startDate = assignment.Date.Date.Add(start);
            var endDate = assignment.Date.Date.Add(end);
            
            // Manejar el caso de turnos nocturnos (que terminan al día siguiente)
            if (endDate < startDate)
            {
                endDate = endDate.AddDays(1);
            }

            return new Schedule
            {
                EmployeeId = assignment.EmployeeId,
                StartDate = startDate,
                EndDate = endDate
            };
        })
        .Where(s => s != null)
        .Select(s => s!)
        .ToList();

    Console.WriteLine($"Generados {allSchedules.Count} Schedules para análisis.");
    Console.WriteLine("---------------------------------------------");


    // 3. Ejecutar la lógica de solape
    var overlaps = OverlapFinder.FindOverlaps(allSchedules);

    // 4. Imprimir resultados
    Console.WriteLine("=== Resultados de Solapes ===");

    if (overlaps.Any())
    {
        foreach (var overlap in overlaps)
        {
            Console.WriteLine(overlap);
        }
    }
    else
    {
        Console.WriteLine("¡No se encontraron solapes de turnos en la lista proporcionada!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\n Ocurrió un error: {ex.Message}");
    // Console.WriteLine(ex.StackTrace); 
}


// --- LÓGICA DE SOLAPE ---

public static class OverlapFinder
{
    public static List<OverlapResult> FindOverlaps(List<Schedule> allSchedules)
    {
        var overlaps = new List<OverlapResult>();

        // 1. AGRUPAR: Agrupamos los turnos por EmployeeId
        var schedulesByEmployee = allSchedules
            .GroupBy(s => s.EmployeeId);

        // 2. ITERAR: Recorrer cada grupo de empleados
        foreach (var group in schedulesByEmployee)
        {
            var employeeSchedules = group.ToList();

            // 3. OPTIMIZACIÓN: Ordenar por hora de inicio para permitir el recorrido simple.
            var sortedSchedules = employeeSchedules.OrderBy(s => s.StartDate).ToList();

            // 4. BUCLE SIMPLE: Recorrer y comparar cada turno con su predecesor inmediato.
            for (int i = 1; i < sortedSchedules.Count; i++)
            {
                var scheduleB = sortedSchedules[i];     // El turno actual
                var scheduleA = sortedSchedules[i - 1]; // El turno anterior

                // 5. USAR MÉTODO ENCAPSULADO (DoesOverlap)
                if (scheduleA.DoesOverlap(scheduleB)) 
                {
                    // Calcular el periodo exacto del solape (la complejidad matemática)
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

    // Métodos auxiliares para la comparación de DateTime
    private static DateTime Max(DateTime a, DateTime b) => a > b ? a : b;
    private static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;
}