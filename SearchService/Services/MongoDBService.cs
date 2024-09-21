using MongoDB.Bson;
using MongoDB.Driver;
using SearchService.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // 1. Buscar estudiante por ID o parte del nombre
        public async Task<List<Student>> GetStudentByIdOrNameAsync(string idOrName)
        {
            var filter = Builders<Student>.Filter.Or(
                Builders<Student>.Filter.Eq(s => s.Id, ObjectId.TryParse(idOrName, out ObjectId objectId) ? objectId : ObjectId.Empty),
                Builders<Student>.Filter.Regex(s => s.Name, new BsonRegularExpression(idOrName, "i"))
            );

            return await _students.Find(filter).ToListAsync();
        }

        // 2. Buscar estudiantes que poseen una restricción específica
        public async Task<List<StudentRestrictionResult>> GetStudentsByRestriction(string restrictionReason)
        {
            var filter = Builders<Student>.Filter.ElemMatch(s => s.Restrictions,
                r => r.Reason.ToLower().Contains(restrictionReason.ToLower()));

            var students = await _students.Find(filter).ToListAsync();

            var result = new List<StudentRestrictionResult>();
            foreach (var student in students)
            {
                foreach (var restriction in student.Restrictions)
                {
                    if (restriction.Reason.ToLower().Contains(restrictionReason.ToLower()))
                    {
                        result.Add(new StudentRestrictionResult
                        {
                            StudentId = student.Id.ToString(),
                            Name = student.Name,
                            Email = student.Email,
                            RestrictionId = restriction.RestrictionId,
                            RestrictionReason = restriction.Reason,
                            CreationDate = restriction.CreationDate
                        });
                    }
                }
            }
            return result;
        }

        // 3. Buscar estudiantes por rango de notas
        public async Task<List<Student>> GetStudentsByGradeRange(double min, double max)
        {
            var filter = Builders<Student>.Filter.ElemMatch(s => s.Grades,
                g => g.GradeValue >= min && g.GradeValue <= max);

            return await _students.Find(filter).ToListAsync();
        }
    }
}
