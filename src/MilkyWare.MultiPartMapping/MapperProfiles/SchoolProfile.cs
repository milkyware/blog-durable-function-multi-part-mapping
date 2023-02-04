using AutoMapper;
using MilkyWare.MultiPartMapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkyWare.MultiPartMapping.MapperProfiles
{
    public class SchoolProfile : Profile
    {
        public SchoolProfile()
        {
            CreateMap<School, PupilExport>()
                .ForMember(d => d.SchoolName,
                o => o.MapFrom(s => s.Name));
        }
    }
}
