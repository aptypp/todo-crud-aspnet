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
        const string tasksGroup = "/api/tasks";

        var todosApi = app.MapGroup(tasksGroup)
            .WithTags("Tasks");

        todosApi.MapGet("/{id:int}", async (int id, AppDbContext context, IMapper mapper) =>
            {
                var entity = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

                return entity is null ? Results.NotFound(id) : Results.Ok(mapper.Map<TaskDto>(entity));
            })
            .WithOpenApi()
            .RequireAuthorization();

        todosApi.MapGet("/", async (AppDbContext context, IMapper mapper) =>
            {
                var entities = await context.Tasks.ToListAsync();

                var response = entities.Select(mapper.Map<TaskDto>);

                return Results.Ok(response);
            })
            .WithOpenApi()
            .RequireAuthorization();

        todosApi.MapPost("/", async (TaskPostDto taskPostDto, AppDbContext context, IMapper mapper) =>
            {
                var entity = new TaskEntity
                {
                    Name = taskPostDto.Name,
                    Description = taskPostDto.Description,
                    IsCompleted = taskPostDto.IsCompleted
                };

                var entry = await context.Tasks.AddAsync(entity);

                await context.SaveChangesAsync();

                var taskDto = mapper.Map<TaskDto>(entry.Entity);

                return Results.Created($"{tasksGroup}/{taskDto.Id}", taskDto);
            })
            .WithOpenApi()
            .RequireAuthorization();

        todosApi.MapPatch("/{id:int}",
                async (int id, TaskPatchDto patchDto, AppDbContext context, IMapper mapper) =>
                {
                    var entity = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

                    if (entity is null) return Results.NotFound(id);

                    if (patchDto.Name != null) entity.Name = patchDto.Name;
                    if (patchDto.Description != null) entity.Description = patchDto.Description;
                    if (patchDto.IsCompleted != null) entity.IsCompleted = patchDto.IsCompleted.Value;

                    context.Tasks.Update(entity);

                    await context.SaveChangesAsync();

                    return Results.Ok(mapper.Map<TaskDto>(entity));
                })
            .WithOpenApi()
            .RequireAuthorization();

        todosApi.MapDelete("/{id:int}", async (int id, AppDbContext context, IMapper mapper) =>
            {
                var entity = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is null) return Results.NotFound(id);

                context.Tasks.Remove(entity);

                await context.SaveChangesAsync();

                return Results.Ok(mapper.Map<TaskDto>(entity));
            })
            .WithOpenApi()
            .RequireAuthorization();
    }
}