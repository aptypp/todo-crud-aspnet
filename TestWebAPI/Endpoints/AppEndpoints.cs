using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Dtos;
using TestWebAPI.Entities;

namespace TestWebAPI.Endpoints;

public static class AppEndpoints
{
    public static void MapAppEndpoints(this WebApplication app)
    {
        var todosApi = app.MapGroup("/api/tasks")
            .WithTags("Tasks");

        todosApi.MapGet("/{id:int}", async (int id, AppDbContext context, IMapper mapper) =>
            {
                var entity = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

                return entity is null ? Results.NotFound(id) : Results.Ok(mapper.Map<TaskDto>(entity));
            })
            .WithOpenApi();

        todosApi.MapGet("/", async (AppDbContext context, IMapper mapper) =>
            {
                var entities = await context.Tasks.ToListAsync();

                var response = entities.Select(mapper.Map<TaskDto>);

                return Results.Ok(response);
            })
            .WithOpenApi();

        todosApi.MapPost("/", async (CreateTaskRequest request, AppDbContext context, IMapper mapper) =>
            {
                var entity = new TaskEntity
                {
                    Name = request.Name,
                    Description = request.Description,
                    IsCompleted = request.IsCompleted,
                };

                var entry = await context.Tasks.AddAsync(entity);

                await context.SaveChangesAsync();

                var dto = mapper.Map<TaskDto>(entry.Entity);

                return Results.Created($"/api/tasks/{dto.Id}", dto);
            })
            .WithOpenApi();

        todosApi.MapPut("/{id:int}", async (int id, UpdateTaskRequest request, AppDbContext context, IMapper mapper) =>
            {
                var entity = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is null) return Results.NotFound(id);

                entity.Name = request.Name;
                entity.Description = request.Description;
                entity.IsCompleted = request.IsCompleted;

                context.Tasks.Update(entity);

                await context.SaveChangesAsync();

                return Results.Ok(mapper.Map<TaskDto>(entity));
            })
            .WithOpenApi();

        todosApi.MapDelete("/{id:int}", async (int id, AppDbContext context, IMapper mapper) =>
            {
                var entity = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is null) return Results.NotFound(id);

                context.Tasks.Remove(entity);

                await context.SaveChangesAsync();

                return Results.Ok(mapper.Map<TaskDto>(entity));
            })
            .WithOpenApi();
    }
}