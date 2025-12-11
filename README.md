# 🛡️ ScheduleFinder: Microservicio de Validación de Solapamiento de Horarios

**Autor:** Fritz Herrera Bernedo

Este proyecto de C# simula un Microservicio enfocado en la validación algorítmica de horarios de personal, diseñado específicamente para detectar conflictos o solapamientos de turnos, un punto crítico en la gestión de guardias y seguridad.

La solución está construida como una **aplicación de consola (.NET Core)** para ejecutarse en línea de comandos (`dotnet run`), demostrando un diseño modular, eficiencia algorítmica y principios de arquitectura de software.

---

## 🏗️ Arquitectura y Diseño de Software

El proyecto se estructura en capas para asegurar la limpieza del código, la escalabilidad y el cumplimiento del **Principio de Responsabilidad Única (SRP)**:

| Capa / Módulo | Rol | Diseño de Código |
| :--- | :--- | :--- |
| **`Program.cs`** | **Orquestador (I/O)** | Se limita a iniciar la aplicación, manejar errores globales y gestionar la entrada/salida (impresión en consola). |
| **`Services/`** | **Lógica de Negocio** | Contiene toda la lógica de la aplicación, separada en servicios dedicados: `ScheduleDataService` (carga y transformación de datos) y `OverlapFinder` (algoritmo central).  |
| **`Models/`** | **Contratos de Datos** | Define los DTOs (Data Transfer Objects) y la estructura de datos interna (`Schedule`, `OverlapResult`). |
| **`Data/`** | **Simulación de Fuente de Datos** | Contiene `data.json`, simulando la respuesta consolidada de múltiples microservicios (Empleados, Turnos, Asignaciones). |

## ✨ Puntos Clave de Ingeniería (Optimization)

Este proyecto destaca por la aplicación de técnicas avanzadas para garantizar la eficiencia y la robustez del sistema:

### 1. Optimización Algorítmica (Rendimiento O(N log N))

El corazón del sistema, el `OverlapFinder`, no utiliza el ineficiente bucle anidado $O(N^2)$. En su lugar, aplica un algoritmo basado en la técnica de **Barrido de Línea (Sweep Line)**:

* **Proceso:** Agrupa las asignaciones por guardia, las ordena por tiempo de inicio ($O(N \log N)$), y luego las recorre en un único pase para detectar solapes.
* **Ventaja:** Garantiza la máxima eficiencia para la detección de conflictos, ideal para sistemas que manejan un gran volumen de turnos.

### 2. Modularidad y Encapsulación Avanzada (Patrón Factory)

* **Patrón Factory Estático:** La lógica compleja de cálculo de fechas (manejo de turnos nocturnos, uso de valores personalizados, etc.) está encapsulada en el método estático `Schedule.CreateFromAssignment()`.  Esto aísla el código de cálculo y mejora la limpieza de la capa de servicios.
* **Métodos de Extensión:** La lógica de la condición de solapamiento se abstrae en el método de extensión `Schedule.DoesOverlap()`, haciendo que el código del algoritmo principal sea extremadamente legible.

---

## ▶️ Cómo Ejecutar (Línea de Comandos)

1.  Clonar el repositorio.
2.  Navegar a la carpeta raíz del proyecto (`ScheduleFinder`) usando la terminal.
3.  Asegúrese de tener instalado el .NET Runtime o SDK necesario.
4.  Ejecutar el proyecto con el comando:
    ```bash
    dotnet run
    ```

El sistema cargará los datos de `Data/data.json` y reportará todos los solapamientos encontrados en la consola.