using MongoDB.Driver;
using SearchService.Models;
using Microsoft.Extensions.Options;
using System;

namespace SearchService.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Student> _students;

        public MongoDBService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _students = database.GetCollection<Student>("students");
        }

        // Obtener todos los estudiantes
        public async Task<List<Student>> GetAllStudentsAsync() =>
            await _students.Find(student => true).ToListAsync();

        // Obtener un estudiante por su Guid
        public async Task<Student> GetStudentByIdAsync(Guid id) =>
            await _students.Find(student => student.Id == id).FirstOrDefaultAsync();

        // Buscar estudiantes por nombre o parte de él
        public async Task<List<Student>> GetStudentsByNameAsync(string name) =>
            await _students.Find(student => student.Name.ToLower().Contains(name.ToLower())).ToListAsync();

        // Obtener estudiantes por una restricción específica
        public async Task<List<StudentRestrictionResult>> GetStudentsByRestriction(string restrictionReason) =>
            await _students.Find(student => student.Restrictions.Any(r => r.Reason.Contains(restrictionReason)))
                .Project(student => new StudentRestrictionResult
                {
                    StudentId = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    RestrictionId = student.Restrictions.First().RestrictionId,
                    RestrictionReason = student.Restrictions.First().Reason,
                    CreationDate = student.Restrictions.First().CreationDate
                }).ToListAsync();

        // Obtener estudiantes dentro de un rango de notas
        public async Task<List<Student>> GetStudentsByGradeRange(double min, double max) =>
            await _students.Find(student => student.Grades.Any(g => g.GradeValue >= min && g.GradeValue <= max)).ToListAsync();
    }
}
