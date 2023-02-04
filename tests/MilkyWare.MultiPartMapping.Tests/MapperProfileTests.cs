using Xunit;
using MilkyWare.MultiPartMapping.MapperProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MilkyWare.MultiPartMapping.Models;

namespace MilkyWare.MultiPartMapping.MapperProfiles.Tests
{
    public class MapperProfileTests
    {
        private readonly IMapper _mapper;

        public MapperProfileTests()
        {
            _mapper = new MapperConfiguration(configure =>
            {
                configure.AddMaps(typeof(Startup));
            }).CreateMapper();
        }

        [Fact()]
        public void PupilProfileTest()
        {
            var input = new Pupil
            {
                Forename = "Joe",
                Surname = "Bloggs",
                DOB = new DateTime(2000, 1, 1)
            };
            var expected = new PupilExport
            {
                PupilName = "Joe Bloggs",
                DOB = new DateTime(2000, 1, 1)
            };
            var actual = _mapper.Map<PupilExport>(input);
            Assert.Equivalent(expected, actual);
        }

        [Fact()]
        public void SchoolProfileTest()
        {
            var input = new School
            {
                Name = "High School"
            };
            var expected = new PupilExport
            {
                SchoolName = "High School",
            };
            var actual = _mapper.Map<PupilExport>(input);
            Assert.Equivalent(expected, actual);
        }

        [Fact()]
        public void YearGroupProfileTest()
        {
            var input = new YearGroup
            {
                Name = "Year 7"
            };
            var expected = new PupilExport
            {
                YearGroup = "Year 7",
            };
            var actual = _mapper.Map<PupilExport>(input);
            Assert.Equivalent(expected, actual);
        }

        [Fact()]
        public void PupilExportProfileTest()
        {
            var input = Tuple.Create(new Pupil
            {
                Forename = "Joe",
                Surname = "Bloggs",
                DOB = new DateTime(2000, 1, 1)
            },
            new YearGroup
            {
                Name = "Year 7"
            }, new School
            {
                Name = "High School"
            });
            var expected = new PupilExport
            {
                PupilName = "Joe Bloggs",
                DOB = new DateTime(2000, 1, 1),
                SchoolName = "High School",
                YearGroup = "Year 7"
            };
            var actual = _mapper.Map<PupilExport>(input);
            Assert.Equivalent(expected, actual);
        }
    }
}