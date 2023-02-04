using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MilkyWare.MultiPartMapping.Models;
using Newtonsoft.Json;

namespace MilkyWare.MultiPartMapping.Functions
{
    public class DurableFunction
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public DurableFunction(ILogger<DurableFunction> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [FunctionName("DurableFunction")]
        [OpenApiOperation(operationId: "Post", tags: new[] { "DurableFunction" })]
        [OpenApiRequestBody(MediaTypeNames.Application.Json, typeof(Pupil))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, MediaTypeNames.Application.Json, typeof(PupilExport))]
        public async Task<PupilExport> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] Pupil pupil,
            [DurableClient] IDurableOrchestrationClient starter)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync(nameof(DurableFunctionOrchestrator), pupil);

            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            DurableOrchestrationStatus status = null;

            do
            {
                Thread.Sleep(1000);
                status = await starter.GetStatusAsync(instanceId);
            } while (status.RuntimeStatus != OrchestrationRuntimeStatus.Completed);
            _logger.LogInformation($"Completed orchestration with ID = '{instanceId}'.");

            var pupilExport = status.Output.ToObject<PupilExport>();
            return pupilExport;
        }

        [FunctionName(nameof(DurableFunctionOrchestrator))]
        public async Task<PupilExport> DurableFunctionOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var pupil = context.GetInput<Pupil>();
            var yearGroup = await context.CallActivityAsync<YearGroup>(nameof(DurableFunctionGetYearGroup), null);
            var school = await context.CallActivityAsync<School>(nameof(DurableFunctionGetSchool), null);
            var pupilExport = await context.CallActivityAsync<PupilExport>(nameof(DurableFunctionBuildPupilExport), (pupil, yearGroup, school));
            return pupilExport;
        }

        [FunctionName(nameof(DurableFunctionGetYearGroup))]
        public YearGroup DurableFunctionGetYearGroup([ActivityTrigger] IDurableActivityContext context)
        {
            _logger.LogInformation($"Getting {nameof(YearGroup)}");
            return new YearGroup()
            {
                Name = "Year 10"
            };
        }

        [FunctionName(nameof(DurableFunctionGetSchool))]
        public School DurableFunctionGetSchool([ActivityTrigger] IDurableActivityContext context)
        {
            _logger.LogInformation($"Getting {nameof(School)}");
            return new School()
            {
                Name = "High School"
            };
        }

        [FunctionName(nameof(DurableFunctionBuildPupilExport))]
        public PupilExport DurableFunctionBuildPupilExport([ActivityTrigger] Tuple<Pupil, YearGroup, School> tuple)
        {
            _logger.LogInformation($"Mapping tuple sources to {nameof(PupilExport)}");
            var pupilExport = _mapper.Map<PupilExport>(tuple);
            return pupilExport;
        }
    }
}