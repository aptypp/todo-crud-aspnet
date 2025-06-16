namespace TestWebAPI.Dtos;

public record UpdateTaskRequest(string Name, string Description, bool IsCompleted);