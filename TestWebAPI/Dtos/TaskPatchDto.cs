namespace TestWebAPI.Dtos;

public record TaskPatchDto(
    string? Name,
    string? Description,
    bool? IsCompleted);