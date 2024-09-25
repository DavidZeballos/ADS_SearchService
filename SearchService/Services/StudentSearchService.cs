using MongoDB.Bson;
using MongoDB.Driver;
using SearchService.Models;
using Microsoft.Extensions.Options;

namespace SearchService.Services
{
    public class StudentSearchService
    {
        private readonly IMongoCollection<Student> _students;

        public StudentSearchService(IOptions<MongoDBSettings> settings)
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

                // Intentamos parsear el query como un UUID (Guid)
                if (Guid.TryParse(query, out Guid uuid))
                {
                    // Si es un UUID válido, buscamos por UUID
                    filter = Builders<Student>.Filter.Eq(s => s.Id, uuid);

                    // Intentamos encontrar un estudiante por UUID
                    var studentsById = await _students.Find(filter).ToListAsync();

                    if (studentsById.Count > 0)
                    {
                        return studentsById;  // Retornamos si encontramos coincidencias por UUID
                    }
                    else
                    {
                        throw new Exception($"No se encontraron estudiantes con el UUID '{query}' en la base de datos.");
                    }
                }

                // Si no es un UUID válido, intentamos buscar por coincidencias en el nombre usando regex
                filter = Builders<Student>.Filter.Regex(s => s.Name, new BsonRegularExpression(query, "i"));

                var studentsByName = await _students.Find(filter).ToListAsync();

                if (studentsByName.Count == 0)
                {
                    throw new Exception($"No se encontraron estudiantes con el nombre '{query}' en la base de datos.");
                }

                return studentsByName;  // Retornamos si encontramos coincidencias por nombre
            }
            catch (Exception ex)
            {
                throw new Exception($"Error buscando estudiante con UUID o nombre: {query}. Detalle del error: {ex.Message}", ex);
            }
        }

        // Método para obtener todos los estudiantes
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
                throw new Exception("Error obteniendo todos los estudiantes. Detalle del error: " + ex.Message, ex);
            }
        }

        // Obtener estudiantes por restricción
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
                throw new Exception($"Error buscando estudiantes con la restricción: '{restrictionReason}'. Detalle del error: {ex.Message}", ex);
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
                throw new Exception($"Error buscando estudiantes por rango de notas: {min} - {max}. Detalle del error: {ex.Message}", ex);
            }
        }
    }
}