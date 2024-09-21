using Microsoft.AspNetCore.Mvc;
using SearchService.Services;
using SearchService.Models;
using System;
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

        // Buscar todos los estudiantes
        [HttpGet("students")]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            var students = await _mongoDBService.GetAllStudentsAsync();
            if (students.Count == 0)
                return NotFound(new { message = "No se encontraron estudiantes" });

            return Ok(students);
        }

        // Buscar todas las calificaciones y restricciones de un estudiante por ID o nombre
        [HttpGet("student/{idOrName}")]
        public async Task<ActionResult<List<Student>>> GetStudentByIdOrName(string idOrName)
        {
            if (Guid.TryParse(idOrName, out Guid guid))
            {
                var student = await _mongoDBService.GetStudentByIdAsync(guid);
                if (student == null)
                    return NotFound(new { message = "Estudiante no encontrado" });

                return Ok(student);
            }
            else
            {
                var students = await _mongoDBService.GetStudentsByNameAsync(idOrName);
                if (students == null || students.Count == 0)
                    return NotFound(new { message = "No se encontraron estudiantes con ese nombre" });

                return Ok(students);
            }
        }

        // Buscar estudiantes que poseen una restricción específica (query parameter)
        [HttpGet("restriction")]
        public async Task<ActionResult<List<StudentRestrictionResult>>> GetStudentsByRestriction([FromQuery] string restrictionReason)
        {
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
