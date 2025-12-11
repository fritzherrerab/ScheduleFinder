using System;
using System.IO;
using System.Linq;

using ScheduleFinder.Services;

// --- INICIADOR DE LA APLICACIÓN ---
Console.WriteLine("Iniciando Microservicio de Validación de Horarios...");
Console.WriteLine("---------------------------------------------");

try
{
    // 1. CARGA DE DATOS: Tarea delegada al Data Service
    string jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "data.json");
    var allSchedules = ScheduleDataService.LoadAndTransformData(jsonPath);

    if (allSchedules == null)
    {
        // El error ya fue reportado por ScheduleDataService
        return;
    }

    // 2. EJECUCIÓN DE LA LÓGICA: Tarea delegada al Overlap Finder Service
    var overlaps = OverlapFinder.FindOverlaps(allSchedules);

    // 3. IMPRESIÓN DE RESULTADOS (I/O): Tarea de Program.cs
    Console.WriteLine("\n=== Resultados de Solapes ===");

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
    // Cierre seguro y manejo de errores globales
    Console.WriteLine($"\n Ocurrió un error inesperado y crítico: {ex.Message}");
}
finally
{
    Console.WriteLine("\n---------------------------------------------");
    Console.WriteLine("Microservicio finalizado.");
}