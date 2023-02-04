using AutoMapper;
using MilkyWare.MultiPartMapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkyWare.MultiPartMapping.MapperProfiles
{
    public class PupilProfile : Profile
    {
        public PupilProfile()
        {
            CreateMap<Pupil, PupilExport>()
                .ForMember(d => d.PupilName,
                o => o.MapFrom(s => $"{s.Forename} {s.Surname}".Trim()))
                .ForMember(d => d.DOB,
                o => o.MapFrom(s => s.DOB));
        }
    }
}
