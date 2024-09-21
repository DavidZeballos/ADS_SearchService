
using Microsoft.AspNetCore.Mvc;
using SearchService.Services;
using SearchService.Models;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public SearchController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        // Buscar todas las calificaciones y restricciones de un estudiante
        [HttpGet("student/{id}")]
        public async Task<ActionResult<Student>> GetStudentById(string id)
        {
            var student = await _mongoDBService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        // Buscar estudiantes que poseen una restricción específica
        [HttpGet("restriction")]
        public async Task<ActionResult<List<Student>>> GetStudentsByRestriction(string restrictionReason)
        {
            var students = await _mongoDBService.GetStudentsByRestriction(restrictionReason);
            return Ok(students);
        }

        // Buscar estudiantes por un rango de notas
        [HttpGet("grades")]
        public async Task<ActionResult<List<Student>>> GetStudentsByGradeRange(double min, double max)
        {
            var students = await _mongoDBService.GetStudentsByGradeRange(min, max);
            return Ok(students);
        }
    }
}