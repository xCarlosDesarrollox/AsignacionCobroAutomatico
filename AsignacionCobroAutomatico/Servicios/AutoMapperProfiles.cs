using AsignacionCobroAutomatico.Models;
using AutoMapper;

namespace AsignacionCobroAutomatico.Servicios
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        { 
            CreateMap<Cliente, ClienteViewModel>().ReverseMap();
        }
    }
}
