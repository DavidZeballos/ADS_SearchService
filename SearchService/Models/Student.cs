public class Student
{
    public string Id { get; set; } = string.Empty; // Evitar nulos
    public string Name { get; set; } = string.Empty; // Evitar nulos
    public string Email { get; set; } = string.Empty; // Evitar nulos
    public List<Grade> Grades { get; set; } = new(); // Inicializar lista vacía
    public List<Restriction> Restrictions { get; set; } = new(); // Inicializar lista vacía
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
