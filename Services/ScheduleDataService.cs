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
            // 1. Validar y Cargar el JSON
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"ERROR: Archivo 'data.json' no encontrado en: {jsonPath}");
                return null;
            }

            try
            {
                string jsonString = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<DataContainer>(jsonString, options);

                if (data == null || !data.Assignments.Any())
                {
                    Console.WriteLine("No se pudo cargar la data o no hay asignaciones.");
                    return null;
                }
                
                Console.WriteLine($"Procesando {data.Assignments.Count} asignaciones...");

                // 2. Transformar Asignaciones en Schedules (Lógica de Negocio)
                var allSchedules = data.Assignments
                    .Select(assignment =>
                    {
                        var shiftBase = data.Shifts.FirstOrDefault(s => s.ShiftId == assignment.ShiftId);
                        if (shiftBase == null) return null;

                        var start = assignment.CustomStartTime ?? shiftBase.DefaultStartTime;
                        var end = assignment.CustomEndTime ?? shiftBase.DefaultEndTime;
                        
                        var startDate = assignment.Date.Date.Add(start);
                        var endDate = assignment.Date.Date.Add(end);
                        
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
                return allSchedules;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"ERROR de deserialización JSON: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR al procesar datos: {ex.Message}");
                return null;
            }
        }
    }
}