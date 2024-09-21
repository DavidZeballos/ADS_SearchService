using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class Student
{
    // Usa Id como el identificador único y elimina el uso de _id
    [BsonId]  // Esto indica que este será el identificador principal en MongoDB
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("Grades")]
    public List<Grade> Grades { get; set; } = new();

    [BsonElement("Restrictions")]
    public List<Restriction> Restrictions { get; set; } = new();
}

public class Grade
{
    [BsonElement("GradeId")]
    [BsonRepresentation(BsonType.String)]
    public Guid GradeId { get; set; } = Guid.NewGuid();

    [BsonElement("Course")]
    public string CourseName { get; set; } = string.Empty;

    [BsonElement("GradeName")]
    public string GradeName { get; set; } = string.Empty;

    [BsonElement("GradeValue")]
    public double GradeValue { get; set; }

    [BsonElement("Comment")]
    public string Comment { get; set; } = string.Empty;
}

public class Restriction
{
    [BsonElement("RestrictionId")]
    [BsonRepresentation(BsonType.String)]
    public Guid RestrictionId { get; set; } = Guid.NewGuid();

    [BsonElement("Reason")]
    public string Reason { get; set; } = string.Empty;

    [BsonElement("CreationDate")]
    public DateTime CreationDate { get; set; } = DateTime.Now;
}

public class StudentRestrictionResult
{
    [BsonElement("StudentId")]
    [BsonRepresentation(BsonType.String)]
    public Guid StudentId { get; set; } = Guid.NewGuid();

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("RestrictionId")]
    [BsonRepresentation(BsonType.String)]
    public Guid RestrictionId { get; set; } = Guid.NewGuid();

    [BsonElement("RestrictionReason")]
    public string RestrictionReason { get; set; } = string.Empty;

    [BsonElement("CreationDate")]
    public DateTime CreationDate { get; set; }
}
