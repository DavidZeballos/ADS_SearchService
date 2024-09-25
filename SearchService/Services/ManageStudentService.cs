using MongoDB.Bson;
using MongoDB.Driver;
using SearchService.Models;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace SearchService.Services.Manage
{
    public class ManageStudentService
    {
        private readonly IMongoCollection<Student> _students;

        public ManageStudentService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _students = database.GetCollection<Student>("students");
        }

        // --- CRUD para Estudiantes ---

        public async Task CreateStudentAsync(Student student)
        {
            // Verificar si el UUID del estudiante es válido
            if (student.Id == Guid.Empty)
            {
                throw new ArgumentException("El UUID del estudiante es obligatorio.");
            }

            // Verificar si ya existe un estudiante con el mismo UUID
            var existingStudent = await _students.Find(s => s.Id == student.Id).FirstOrDefaultAsync();
            if (existingStudent != null)
            {
                throw new ArgumentException($"Ya existe un estudiante con el UUID '{student.Id}'.");
            }

            await _students.InsertOneAsync(student);
        }
        
        public async Task<bool> UpdateStudentAsync(Guid id, Student updatedStudent)
        {
            var result = await _students.ReplaceOneAsync(s => s.Id == id, updatedStudent);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            var result = await _students.DeleteOneAsync(s => s.Id == id);
            return result.DeletedCount > 0;
        }

        // --- Gestión de Restricciones ---
        
        public async Task<bool> AddRestrictionToStudentAsync(Guid studentId, Restriction restriction)
        {
            var student = await _students.Find(s => s.Id == studentId).FirstOrDefaultAsync();
            if (student == null) return false;

            // Verificar si ya existe una restricción con el mismo UUID
            if (student.Restrictions.Any(r => r.RestrictionId == restriction.RestrictionId))
            {
                throw new ArgumentException("Ya existe una restricción con el mismo UUID.");
            }

            student.Restrictions.Add(restriction);
            var result = await _students.ReplaceOneAsync(s => s.Id == studentId, student);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateStudentRestrictionAsync(Guid studentId, Guid restrictionId, Restriction updatedRestriction)
        {
            var filter = Builders<Student>.Filter.And(
                Builders<Student>.Filter.Eq(s => s.Id, studentId),
                Builders<Student>.Filter.ElemMatch(s => s.Restrictions, r => r.RestrictionId == restrictionId)
            );

            var update = Builders<Student>.Update
                .Set("Restrictions.$.Reason", updatedRestriction.Reason)
                .Set("Restrictions.$.CreationDate", updatedRestriction.CreationDate);

            var result = await _students.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteStudentRestrictionAsync(Guid studentId, Guid restrictionId)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.Id, studentId);
            var update = Builders<Student>.Update.PullFilter(s => s.Restrictions, r => r.RestrictionId == restrictionId);
            var result = await _students.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // --- Gestión de Calificaciones ---

        public async Task<bool> AddGradeToStudentAsync(Guid studentId, Grade grade)
        {
            var student = await _students.Find(s => s.Id == studentId).FirstOrDefaultAsync();
            if (student == null) return false;

            // Verificar si ya existe una calificación con el mismo UUID
            if (student.Grades.Any(g => g.GradeId == grade.GradeId))
            {
                throw new ArgumentException("Ya existe una calificación con el mismo UUID.");
            }

            student.Grades.Add(grade);
            var result = await _students.ReplaceOneAsync(s => s.Id == studentId, student);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateStudentGradeAsync(Guid studentId, Guid gradeId, Grade updatedGrade)
        {
            var filter = Builders<Student>.Filter.And(
                Builders<Student>.Filter.Eq(s => s.Id, studentId),
                Builders<Student>.Filter.ElemMatch(s => s.Grades, g => g.GradeId == gradeId)
            );

            var update = Builders<Student>.Update
                .Set("Grades.$.CourseName", updatedGrade.CourseName)
                .Set("Grades.$.GradeName", updatedGrade.GradeName)
                .Set("Grades.$.GradeValue", updatedGrade.GradeValue)
                .Set("Grades.$.Comment", updatedGrade.Comment);

            var result = await _students.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteStudentGradeAsync(Guid studentId, Guid gradeId)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.Id, studentId);
            var update = Builders<Student>.Update.PullFilter(s => s.Grades, g => g.GradeId == gradeId);
            var result = await _students.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
