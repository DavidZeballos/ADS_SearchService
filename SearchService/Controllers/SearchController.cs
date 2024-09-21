using Microsoft.AspNetCore.Mvc;

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
            try
            {
                var students = await _mongoDBService.GetAllStudentsAsync();
                if (students.Count == 0)
                    return NotFound(new { message = "No se encontraron estudiantes" });

                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // Buscar todas las calificaciones y restricciones de un estudiante por UUID (GUID)
        [HttpGet("student/{id}")]
        public async Task<ActionResult<Student>> GetStudentById(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid studentId))
                    return BadRequest("El ID del estudiante es inválido");

                var student = await _mongoDBService.GetStudentByIdAsync(studentId);
                if (student == null)
                    return NotFound(new { message = "Estudiante no encontrado" });

                return Ok(student);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // Buscar estudiantes que poseen una restricción específica (query parameter)
        [HttpGet("restriction")]
        public async Task<ActionResult<List<Student>>> GetStudentsByRestriction([FromQuery] string restrictionReason)
        {
            try
            {
                if (string.IsNullOrEmpty(restrictionReason))
                    return BadRequest("La razón de la restricción es requerida");

                var students = await _mongoDBService.GetStudentsByRestriction(restrictionReason);
                if (students == null || students.Count == 0)
                    return NotFound(new { message = "No se encontraron estudiantes con esa restricción" });

                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // Buscar estudiantes por un rango de notas (query parameters)
        [HttpGet("grades")]
        public async Task<ActionResult<List<Student>>> GetStudentsByGradeRange([FromQuery] double min, [FromQuery] double max)
        {
            try
            {
                if (min < 0 || max < 0 || min > max)
                    return BadRequest("Rango de notas no válido");

                var students = await _mongoDBService.GetStudentsByGradeRange(min, max);
                if (students == null || students.Count == 0)
                    return NotFound(new { message = "No se encontraron estudiantes en ese rango de notas" });

                return Ok(students);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}
