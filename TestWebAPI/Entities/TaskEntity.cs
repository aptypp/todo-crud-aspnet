using Microsoft.AspNetCore.Identity;

namespace TestWebAPI.Entities;

public class TaskEntity
{
    public int Id { get; init; }
    public bool IsCompleted { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    
    public IdentityUser User { get; set; }
}