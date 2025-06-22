using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Experience.API.Models;
using Experience.Application.Commands.CreateExperience;
using Experience.Application.Commands.DeleteExperience;
using Experience.Application.Commands.UpdateExperience;
using Experience.Application.DTOs;
using Experience.Application.Queries.GetExperience;
using Experience.Application.Queries.ListExperience;
using Experience.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ecotrip_backend.Controllers;
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController : ControllerBase
    {        private readonly ILogger<ExperienceController> _logger;

        public ExperienceController(ILogger<ExperienceController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<ActionResult<IEnumerable<ExperienceDTO>>> GetAll()
        {
            _logger.LogInformation("Getting all experiences");
            return Task.FromResult<ActionResult<IEnumerable<ExperienceDTO>>>(Ok(new List<ExperienceDTO>()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<ExperienceDetailsDTO>> GetById(string id)
        {
            _logger.LogInformation("Getting experience with ID: {ExperienceId}", id);
            return Task.FromResult<ActionResult<ExperienceDetailsDTO>>(NotFound());
        }

        [HttpPost]
        [Authorize(Roles = "Agent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<ActionResult<string>> Create(CreateExperienceCommand command)
        {
            _logger.LogInformation("Creating new experience with title: {Title}", command.Title);

            var agentId = User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException("Agent ID not found in claims");
            command.AgentId = agentId;

            var experienceId = Guid.NewGuid().ToString();
            return Task.FromResult<ActionResult<string>>(CreatedAtAction(nameof(GetById), new { id = experienceId }, experienceId));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Agent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult> Update(string id, UpdateExperienceCommand command)
        {
            _logger.LogInformation("Updating experience with ID: {ExperienceId}", id);
            
            if (id != command.Id)
                return Task.FromResult<ActionResult>(BadRequest("ID in the route does not match ID in the request body"));
                
            var agentId = User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException("Agent ID not found in claims");
            command.AgentId = agentId;

            return Task.FromResult<ActionResult>(NoContent());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Agent,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult> Delete(string id)
        {
            _logger.LogInformation("Deleting experience with ID: {ExperienceId}", id);
            
            var agentId = User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException("Agent ID not found in claims");
            var isAdmin = User.IsInRole("Admin");
            
            return Task.FromResult<ActionResult>(NoContent());
        }
    }