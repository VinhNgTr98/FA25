using AutoMapper;
using ReportManagement_API.DTOs;
using ReportManagement_API.Models;

namespace ReportManagement_API.Profiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Report, ReportReadDTO>();
            CreateMap<ReportCreateDTO, Report>();
            CreateMap<ReportUpdateDTO, Report>();
        }
    }
}
