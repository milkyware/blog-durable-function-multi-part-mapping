using AutoMapper;
using MilkyWare.MultiPartMapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkyWare.MultiPartMapping.MapperProfiles
{
    public class PupilExportProfile : Profile
    {
        public PupilExportProfile()
        {
            CreateMap<Tuple<Pupil, YearGroup, School>, PupilExport>()
                .ConvertUsing((s, d, c) =>
                {
                    d = c.Mapper.Map(s.Item1, d);
                    d = c.Mapper.Map(s.Item2, d);
                    d = c.Mapper.Map(s.Item3, d);
                    return d;
                });
        }
    }
}
