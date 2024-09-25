using Microsoft.AspNetCore.Mvc;
using SearchService.Services.Manage;
using SearchService.Models;
using System.Threading.Tasks;
using System;

namespace SearchService.Controllers.Manage
{
    [ApiController]
    [Route("api/manage/student")]
    public class ManageStudentController : ControllerBase
    {
        private readonly ManageStudentService _manageStudentService;

        public ManageStudentController(ManageStudentService manageStudentService)
        {
            _manageStudentService = manageStudentService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudent([FromBody] Student student)
        {
            try
            {
                // Intentamos crear el estudiante
                await _manageStudentService.CreateStudentAsync(student);
                return Ok(new { message = "Estudiante creado exitosamente", id = student.Id });
            }
            catch (ArgumentException ex)
            {
                // Capturamos el error si el UUID es inválido o ya existe
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear estudiante", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStudent(Guid id, [FromBody] Student updatedStudent)
        {
            try
            {
                // No necesitamos la ID en el cuerpo, la tomamos de la URL
                updatedStudent.Id = id;

                var result = await _manageStudentService.UpdateStudentAsync(id, updatedStudent);
                if (!result) return NotFound(new { message = "Estudiante no encontrado" });
                return Ok(new { message = "Estudiante actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar estudiante", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(Guid id)
        {
            try
            {
                var result = await _manageStudentService.DeleteStudentAsync(id);
                if (!result) return NotFound(new { message = "Estudiante no encontrado" });
                return Ok(new { message = "Estudiante eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar estudiante", error = ex.Message });
            }
        }

        // --- Gestión de Restricciones ---
        [HttpPost("{studentId}/restriction")]
        public async Task<ActionResult> AddRestrictionToStudent(Guid studentId, [FromBody] Restriction restriction)
        {
            try
            {
                if (restriction.RestrictionId == Guid.Empty)
                {
                    return BadRequest(new { message = "El UUID de la restricción es obligatorio." });
                }

                var result = await _manageStudentService.AddRestrictionToStudentAsync(studentId, restriction);
                if (!result) return NotFound(new { message = "Estudiante no encontrado" });
                return Ok(new { message = "Restricción añadida exitosamente", restrictionId = restriction.RestrictionId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al añadir restricción", error = ex.Message });
            }
        }

        [HttpPut("{studentId}/restriction/{restrictionId}")]
        public async Task<ActionResult> UpdateStudentRestriction(Guid studentId, Guid restrictionId, [FromBody] Restriction restriction)
        {
            try
            {
                var result = await _manageStudentService.UpdateStudentRestrictionAsync(studentId, restrictionId, restriction);
                if (!result) return NotFound(new { message = "Estudiante o restricción no encontrados" });
                return Ok(new { message = "Restricción del estudiante actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar restricción", error = ex.Message });
            }
        }

        [HttpDelete("{studentId}/restriction/{restrictionId}")]
        public async Task<ActionResult> DeleteStudentRestriction(Guid studentId, Guid restrictionId)
        {
            try
            {
                var result = await _manageStudentService.DeleteStudentRestrictionAsync(studentId, restrictionId);
                if (!result) return NotFound(new { message = "Estudiante o restricción no encontrados" });
                return Ok(new { message = "Restricción eliminada del estudiante exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar restricción", error = ex.Message });
            }
        }

        // --- Gestión de Calificaciones ---
        [HttpPost("{studentId}/grade")]
        public async Task<ActionResult> AddGradeToStudent(Guid studentId, [FromBody] Grade grade)
        {
            try
            {
                if (grade.GradeId == Guid.Empty)
                {
                    return BadRequest(new { message = "El UUID de la calificación es obligatorio." });
                }

                var result = await _manageStudentService.AddGradeToStudentAsync(studentId, grade);
                if (!result) return NotFound(new { message = "Estudiante no encontrado" });
                return Ok(new { message = "Calificación añadida exitosamente", gradeId = grade.GradeId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al añadir calificación", error = ex.Message });
            }
        }

        [HttpPut("{studentId}/grade/{gradeId}")]
        public async Task<ActionResult> UpdateStudentGrade(Guid studentId, Guid gradeId, [FromBody] Grade grade)
        {
            try
            {
                var result = await _manageStudentService.UpdateStudentGradeAsync(studentId, gradeId, grade);
                if (!result) return NotFound(new { message = "Estudiante o calificación no encontrados" });
                return Ok(new { message = "Calificación del estudiante actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar calificación", error = ex.Message });
            }
        }

        [HttpDelete("{studentId}/grade/{gradeId}")]
        public async Task<ActionResult> DeleteStudentGrade(Guid studentId, Guid gradeId)
        {
            try
            {
                var result = await _manageStudentService.DeleteStudentGradeAsync(studentId, gradeId);
                if (!result) return NotFound(new { message = "Estudiante o calificación no encontrados" });
                return Ok(new { message = "Calificación eliminada del estudiante exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar calificación", error = ex.Message });
            }
        }
    }
}
