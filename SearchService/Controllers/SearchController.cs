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

        // Buscar todas las calificaciones y restricciones de un estudiante por ID
        [HttpGet("student/{id}")]
        public async Task<ActionResult<Student>> GetStudentById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("El ID del estudiante es requerido");

            var student = await _mongoDBService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound(new { message = "Estudiante no encontrado" });

            return Ok(student);
        }

        // Buscar estudiantes que poseen una restricción específica (query parameter)
        [HttpGet("restriction")]
        public async Task<ActionResult<List<Student>>> GetStudentsByRestriction([FromQuery] string restrictionReason)
        {
            if (string.IsNullOrEmpty(restrictionReason))
                return BadRequest("La razón de la restricción es requerida");

            var students = await _mongoDBService.GetStudentsByRestriction(restrictionReason);
            if (students == null || students.Count == 0)
                return NotFound(new { message = "No se encontraron estudiantes con esa restricción" });

            return Ok(students);
        }

        // Buscar estudiantes por un rango de notas (query parameters)
        [HttpGet("grades")]
        public async Task<ActionResult<List<Student>>> GetStudentsByGradeRange([FromQuery] double min, [FromQuery] double max)
        {
            if (min < 0 || max < 0 || min > max)
                return BadRequest("Rango de notas no válido");

            var students = await _mongoDBService.GetStudentsByGradeRange(min, max);
            if (students == null || students.Count == 0)
                return NotFound(new { message = "No se encontraron estudiantes en ese rango de notas" });

            return Ok(students);
        }
    }
}
