
using System.Collections.Generic;
using ScheduleFinder.Models;

namespace ScheduleFinder.Data
{
    public class DataContainer
    {
        // Se añaden '= null!;' para resolver advertencias de Nulabilidad
        public List<Employee> Employees { get; set; } = null!;
        public List<Shift> Shifts { get; set; } = null!;
        public List<Assignment> Assignments { get; set; } = null!;
    }
}