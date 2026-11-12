using System.ComponentModel.DataAnnotations;

namespace NextPath.Platform.Tasks.Interfaces.Rest.Resources;

public record CreateStudentTaskResource(
    [property: Range(1, int.MaxValue, ErrorMessage = "StudentId must be a positive number")]
    int StudentId,
    [property: Required(ErrorMessage = "Title is required")]
    [property: MaxLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
    string Title,
    DateOnly? DueDate,
    [property: Range(0, int.MaxValue, ErrorMessage = "Order cannot be negative")]
    int? Order);
