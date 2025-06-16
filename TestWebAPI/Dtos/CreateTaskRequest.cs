namespace TestWebAPI.Dtos;

public record CreateTaskRequest(string Name, string Description, bool IsCompleted);