using AutoMapper;
using MilkyWare.MultiPartMapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkyWare.MultiPartMapping.MapperProfiles
{
    public class YearGroupProfile : Profile
    {
        public YearGroupProfile()
        {
            CreateMap<YearGroup, PupilExport>()
                .ForMember(d => d.YearGroup,
                o => o.MapFrom(s => s.Name));
        }
    }
}
