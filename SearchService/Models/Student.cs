using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class Student
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

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
    public Guid GradeId { get; set; }

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
    public Guid RestrictionId { get; set; }

    [BsonElement("Reason")]
    public string Reason { get; set; } = string.Empty;

    [BsonElement("CreationDate")]
    public DateTime CreationDate { get; set; } = DateTime.Now;
}

public class StudentRestrictionResult
{
    [BsonElement("StudentId")]
    [BsonRepresentation(BsonType.String)]
    public Guid StudentId { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("RestrictionId")]
    [BsonRepresentation(BsonType.String)]
    public Guid RestrictionId { get; set; }

    [BsonElement("RestrictionReason")]
    public string RestrictionReason { get; set; } = string.Empty;

    [BsonElement("CreationDate")]
    public DateTime CreationDate { get; set; }
}
