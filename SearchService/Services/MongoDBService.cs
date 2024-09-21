using MongoDB.Bson;
using MongoDB.Driver;
using SearchService.Models;
using Microsoft.Extensions.Options;

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
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            try
            {
                return await _students.Find(student => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching all students", ex);
            }
        }

        // Obtener un estudiante por su ObjectId
        public async Task<Student> GetStudentByIdAsync(Guid id)
        {
            try
            {
                return await _students.Find(student => student.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching student by ID", ex);
            }
        }

        // Obtener estudiantes por una restricción específica
        public async Task<List<StudentRestrictionResult>> GetStudentsByRestriction(string restrictionReason)
        {
            try
            {
                var students = await _students.Find(student => student.Restrictions.Any(r => r.Reason.Contains(restrictionReason))).ToListAsync();

                return students.Select(student => new StudentRestrictionResult
                {
                    StudentId = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    RestrictionId = student.Restrictions.FirstOrDefault()?.RestrictionId ?? Guid.Empty,
                    RestrictionReason = student.Restrictions.FirstOrDefault()?.Reason ?? string.Empty,
                    CreationDate = student.Restrictions.FirstOrDefault()?.CreationDate ?? DateTime.MinValue
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching students by restriction", ex);
            }
        }

        // Obtener estudiantes dentro de un rango de notas
        public async Task<List<Student>> GetStudentsByGradeRange(double min, double max)
        {
            try
            {
                return await _students.Find(student => student.Grades.Any(g => g.GradeValue >= min && g.GradeValue <= max)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching students by grade range", ex);
            }
        }
    }
}
