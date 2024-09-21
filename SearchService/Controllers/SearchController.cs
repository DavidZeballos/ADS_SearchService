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
        private readonly MongoDBService _mongoDBService;

        public SearchController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        // Buscar todas las calificaciones y restricciones de un estudiante por UUID o parte del nombre
        [HttpGet("student/{query}")]
        public async Task<ActionResult<List<Student>>> GetStudentByIdOrName(string query)
        {
            try
            {
                var students = await _mongoDBService.GetStudentByIdOrNameAsync(query);

                if (students == null || students.Count == 0)
                    return NotFound(new { message = $"Estudiante no encontrado con el identificador o nombre '{query}'" });

                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor al buscar el estudiante con '{query}'", error = ex.Message });
            }
        }

        // Obtener todos los estudiantes
        [HttpGet("students")]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            try
            {
                var students = await _mongoDBService.GetAllStudentsAsync();
                if (students.Count == 0)
                    return NotFound(new { message = "No se encontraron estudiantes" });

                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al obtener todos los estudiantes", error = ex.Message });
            }
        }

        // Obtener estudiantes por una restricción específica
        [HttpGet("restriction")]
        public async Task<ActionResult<List<StudentRestrictionResult>>> GetStudentsByRestriction([FromQuery] string restrictionReason)
        {
            try
            {
                var students = await _mongoDBService.GetStudentsByRestriction(restrictionReason);
                if (students == null || students.Count == 0)
                    return NotFound(new { message = $"No se encontraron estudiantes con la restricción '{restrictionReason}'" });

                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor al buscar estudiantes con la restricción '{restrictionReason}'", error = ex.Message });
            }
        }

        // Obtener estudiantes por un rango de notas (query parameters)
        [HttpGet("grades")]
        public async Task<ActionResult<List<Student>>> GetStudentsByGradeRange([FromQuery] double min, [FromQuery] double max)
        {
            try
            {
                if (min < 0 || max < 0 || min > max)
                    return BadRequest("Rango de notas no válido");

                var students = await _mongoDBService.GetStudentsByGradeRange(min, max);
                if (students == null || students.Count == 0)
                    return NotFound(new { message = $"No se encontraron estudiantes en ese rango de notas ({min}-{max})" });

                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor al buscar estudiantes en el rango de notas {min} - {max}", error = ex.Message });
            }
        }
    }
}