namespace TestWebAPI.Dtos;

public record TaskPostDto(
    string Name,
    string Description,
    bool IsCompleted);