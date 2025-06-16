namespace TestWebAPI.Dtos;

public record TaskPatch(string? Name, string? Description, bool? IsCompleted);