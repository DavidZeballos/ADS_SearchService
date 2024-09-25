using Microsoft.AspNetCore.Mvc;
using SearchService.Services;
using SearchService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly StudentSearchService _studentSearchService;

        public SearchController(StudentSearchService studentSearchService)
        {
            _studentSearchService = studentSearchService;
        }

        // Obtener todos los estudiantes
        [HttpGet("students")]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            try
            {
                var students = await _studentSearchService.GetAllStudentsAsync();
                if (students.Count == 0)
                    return NotFound(new { message = "No se encontraron estudiantes" });

                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al obtener todos los estudiantes", error = ex.Message });
            }
        }

        // Buscar todas las calificaciones y restricciones de un estudiante por UUID o parte del nombre
        [HttpGet("student/{query}")]
        public async Task<ActionResult<List<Student>>> GetStudentByIdOrName(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                    return BadRequest(new { message = "Se requiere un UUID o un nombre para la búsqueda" });

                var students = await _studentSearchService.GetStudentByIdOrNameAsync(query);
                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor al buscar el estudiante con '{query}'", error = ex.Message });
            }
        }

        // Buscar estudiantes que poseen una restricción específica (query parameter)
        [HttpGet("restriction")]
        public async Task<ActionResult<List<StudentRestrictionResult>>> GetStudentsByRestriction([FromQuery] string restrictionReason)
        {
            try
            {
                if (string.IsNullOrEmpty(restrictionReason))
                    return BadRequest(new { message = "Se requiere especificar una razón de restricción" });

                var students = await _studentSearchService.GetStudentsByRestriction(restrictionReason);
                if (students == null || students.Count == 0)
                    return NotFound(new { message = $"No se encontraron estudiantes con la restricción '{restrictionReason}'" });

                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor al buscar estudiantes con la restricción '{restrictionReason}'", error = ex.Message });
            }
        }

        // Buscar estudiantes por un rango de notas (query parameters)
        [HttpGet("grades")]
        public async Task<ActionResult<List<Student>>> GetStudentsByGradeRange([FromQuery] double min, [FromQuery] double max)
        {
            try
            {
                if (min < 0 || max < 0 || min > max)
                    return BadRequest(new { message = "Rango de notas no válido" });

                var students = await _studentSearchService.GetStudentsByGradeRange(min, max);
                if (students == null || students.Count == 0)
                    return NotFound(new { message = $"No se encontraron estudiantes con notas entre {min} y {max}" });

                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor al buscar estudiantes con notas entre {min} y {max}", error = ex.Message });
            }
        }
    }
}
