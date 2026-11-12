using System.ComponentModel.DataAnnotations;

namespace VocaFy.NextPath.Platform.Tasks.Interfaces.Rest.Resources;

public record ChangeStudentTaskStatusResource(
    [property: Required(ErrorMessage = "Status is required")]
    string Status);
