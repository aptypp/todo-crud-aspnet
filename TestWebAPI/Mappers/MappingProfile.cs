using AutoMapper;
using TestWebAPI.Dtos;
using TestWebAPI.Entities;

namespace TestWebAPI.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskEntity, TaskDto>();
        CreateMap<TaskDto, TaskEntity>();
    }
}