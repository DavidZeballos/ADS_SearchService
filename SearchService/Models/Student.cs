using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Student
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }  // Cambiar a ObjectId
     public string Name { get; set; } = string.Empty;
    public List<Grade> Grades { get; set; } = new();
    public List<Restriction> Restrictions { get; set; } = new();
}

public class Grade
{
    public string GradeId { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public double GradeValue { get; set; }
    public string Comment { get; set; } = string.Empty;
}

public class Restriction
{
    public string RestrictionId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;
}
