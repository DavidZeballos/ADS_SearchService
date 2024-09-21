using MongoDB.Driver;
using Microsoft.Extensions.Options;
using SearchService.Models;

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
        
        public async Task<List<Student>> GetStudentsAsync() =>
            await _students.Find(student => true).ToListAsync();

        // Obtener un estudiante por su ID
        public async Task<Student> GetStudentByIdAsync(string id) =>
            await _students.Find(student => student.Id == id).FirstOrDefaultAsync();

        // Obtener estudiantes dentro de un rango de notas
        public async Task<List<Student>> GetStudentsByGradeRange(double min, double max) =>
            await _students.Find(student => student.Grades.Any(g => g.GradeValue >= min && g.GradeValue <= max)).ToListAsync();

        // Obtener estudiantes por una restricción específica
        public async Task<List<Student>> GetStudentsByRestriction(string restrictionReason) =>
            await _students.Find(student => student.Restrictions.Any(r => r.Reason.Contains(restrictionReason))).ToListAsync();
    }
}
