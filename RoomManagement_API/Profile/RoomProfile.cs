using AutoMapper;
using RoomManagement_API.DTOs;
using RoomManagement_API.Models;

namespace RoomsManagement_API.Mapping
{
    public class RoomProfile : Profile  
    {
        public RoomProfile()
        {
            // Entity -> Read DTO
            CreateMap<Room, RoomReadDto>();

            // Create DTO -> Entity
            CreateMap<RoomCreateDto, Room>()
                .ForMember(d => d.RoomId, o => o.Ignore()); // RoomId set ở service

            // Update DTO -> Entity
            CreateMap<RoomUpdateDto, Room>();
        }
    }
}
