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

        // Obtener estudiantes por UUID o parte del nombre
        public async Task<List<Student>> GetStudentByIdOrNameAsync(string query)
        {
            try
            {
                FilterDefinition<Student> filter;

                // Intentamos parsear el query como un Guid (UUID)
                if (Guid.TryParse(query, out Guid uuid))
                {
                    // Si es un UUID válido, filtramos por el campo Id
                    filter = Builders<Student>.Filter.Eq(s => s.Id, uuid);
                }
                else
                {
                    // Si no es un UUID, buscamos por coincidencias en el nombre usando regex
                    filter = Builders<Student>.Filter.Regex(s => s.Name, new MongoDB.Bson.BsonRegularExpression(query, "i"));
                }

                var students = await _students.Find(filter).ToListAsync();
                if (students == null || students.Count == 0)
                {
                    throw new Exception($"No se encontraron estudiantes con el identificador '{query}'");
                }

                return students;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching student by UUID or name: {query}. Detalle del error: {ex.Message}", ex);
            }
        }

        // Obtener todos los estudiantes
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            try
            {
                var students = await _students.Find(student => true).ToListAsync();
                if (students == null || students.Count == 0)
                {
                    throw new Exception("No se encontraron estudiantes en la base de datos.");
                }
                return students;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching all students. Detalle del error: " + ex.Message, ex);
            }
        }

        // Obtener estudiantes por una restricción específica
        public async Task<List<StudentRestrictionResult>> GetStudentsByRestriction(string restrictionReason)
        {
            try
            {
                var students = await _students.Find(student => student.Restrictions.Any(r => r.Reason.Contains(restrictionReason))).ToListAsync();

                if (students == null || students.Count == 0)
                {
                    throw new Exception($"No se encontraron estudiantes con la restricción '{restrictionReason}'");
                }

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
                throw new Exception($"Error fetching students by restriction: '{restrictionReason}'. Detalle del error: {ex.Message}", ex);
            }
        }

        // Obtener estudiantes dentro de un rango de notas
        public async Task<List<Student>> GetStudentsByGradeRange(double min, double max)
        {
            try
            {
                var students = await _students.Find(student => student.Grades.Any(g => g.GradeValue >= min && g.GradeValue <= max)).ToListAsync();
                
                if (students == null || students.Count == 0)
                {
                    throw new Exception($"No se encontraron estudiantes con notas entre {min} y {max}");
                }

                return students;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching students by grade range: {min} - {max}. Detalle del error: {ex.Message}", ex);
            }
        }
    }
}