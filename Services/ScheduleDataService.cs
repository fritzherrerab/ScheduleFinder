// Services/ScheduleDataService.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ScheduleFinder.Data;
using ScheduleFinder.Models;

namespace ScheduleFinder.Services
{
    public static class ScheduleDataService
    {
        public static List<Schedule>? LoadAndTransformData(string jsonPath)
        {
            // ... (Lógica de validación y carga JSON) ...
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"ERROR: Archivo 'data.json' no encontrado en: {jsonPath}");
                return null;
            }

            try
            {
                // ... (Lógica de deserialización JSON) ...
                string jsonString = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<DataContainer>(jsonString, options);

                if (data == null || !data.Assignments.Any())
                {
                    Console.WriteLine("No se pudo cargar la data o no hay asignaciones.");
                    return null;
                }
                
                Console.WriteLine($"Procesando {data.Assignments.Count} asignaciones...");

                // -------------------------------------------------------------------
                // 2. Transformar Asignaciones en Schedules (USANDO EL FACTORY METHOD)
                // -------------------------------------------------------------------
                var allSchedules = data.Assignments
                    .Select(assignment =>
                    {
                        var shiftBase = data.Shifts.FirstOrDefault(s => s.ShiftId == assignment.ShiftId);
                        if (shiftBase == null) return null;

                        // <--- DELEGACIÓN DE RESPONSABILIDAD --->
                        return Schedule.CreateFromAssignment(assignment, shiftBase);
                    })
                    .Where(s => s != null)
                    .Select(s => s!)
                    .ToList();

                Console.WriteLine($"Generados {allSchedules.Count} Schedules para análisis.");
                return allSchedules;
            }
            // ... (Manejo de excepciones) ...
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR al procesar datos: {ex.Message}");
                return null;
            }
        }
    }
}