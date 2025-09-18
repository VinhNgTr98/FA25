using AutoMapper;
using FeedbackManagement_API.DTOs;
using FeedbackManagement_API.Model;

namespace FeedbackManagement_API.Profiles
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedbacks, FeedbackReadDTO>()
                .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.IsReply ? (int?)null : s.Rating))
                .ForMember(d => d.RepliesCount, opt => opt.MapFrom(s => s.Replies.Count));
        }
    }
}