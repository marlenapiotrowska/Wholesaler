﻿using Wholesaler.Backend.Domain.Entities;
using Wholesaler.Core.Dto.ResponseModels;

namespace Wholesaler.Backend.Api.Factories
{
    public class WorkTasksFactory : IWorkTaskFactory
    {
        public WorkTaskDto Create(WorkTask workTask)
        {
            return new WorkTaskDto
            {
                Id = workTask.Id,
                Row = workTask.Row,
                UserId = workTask.Person?.Id,
                Start = workTask.Start,
                Stop = workTask.Stop,
            };
        }
    }
}