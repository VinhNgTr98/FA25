using AutoMapper;
using Payment_API.DTOs;
using Payment_API.Models;

namespace Payment_API.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentReadDTO>();
            CreateMap<PaymentCreateDTO, Payment>();
            CreateMap<PaymentUpdateDTO, Payment>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
