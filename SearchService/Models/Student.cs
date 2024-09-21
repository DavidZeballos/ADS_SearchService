using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


public class Student
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Email")]
    public string Email { get; set; } = string.Empty;  // Campo Email agregado

    [BsonElement("Grades")]
    public List<Grade> Grades { get; set; } = new();

    [BsonElement("Restrictions")]
    public List<Restriction> Restrictions { get; set; } = new();
}

public class Grade
{
    [BsonElement("GradeId")]
    public string GradeId { get; set; } = string.Empty;

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
    public string RestrictionId { get; set; } = string.Empty;

    [BsonElement("Reason")]
    public string Reason { get; set; } = string.Empty;

    [BsonElement("CreationDate")]
    public DateTime CreationDate { get; set; } = DateTime.Now;
}

public class StudentRestrictionResult
{
    [BsonElement("StudentId")]
    public string StudentId { get; set; } = string.Empty;

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("RestrictionId")]
    public string RestrictionId { get; set; } = string.Empty;

    [BsonElement("RestrictionReason")]
    public string RestrictionReason { get; set; } = string.Empty;

    [BsonElement("CreationDate")]
    public DateTime CreationDate { get; set; }
}