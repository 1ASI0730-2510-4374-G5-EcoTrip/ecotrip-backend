using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Experience.Application.Commands.CreateExperience;
using Experience.Application.Commands.DeleteExperience;
using Experience.Application.Commands.PublishExperience;
using Experience.Application.Commands.UpdateExperience;
using Experience.Application.DTOs;
using Experience.Application.Queries.GetExperience;
using Experience.Application.Queries.GetExperiencesByAgent;
using Experience.Application.Queries.GetExperiencesByStatus;
using Experience.Application.Queries.ListExperiences;
using Experience.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Experience.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExperienceController> _logger;

        public ExperienceController(IMediator mediator, ILogger<ExperienceController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all experiences
        /// </summary>
        /// <returns>List of experiences</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ExperienceDTO>>> GetAll()
        {
            _logger.LogInformation("Getting all experiences");
            var query = new ListExperiencesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets an experience by ID
        /// </summary>
        /// <param name="id">Experience ID</param>
        /// <returns>Experience details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExperienceDetailsDTO>> GetById(string id)
        {
            _logger.LogInformation("Getting experience with ID: {ExperienceId}", id);
            var query = new GetExperienceQuery { Id = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound();
                
            return Ok(result);
        }

        /// <summary>
        /// Gets experiences by agent ID
        /// </summary>
        /// <param name="agentId">Agent ID</param>
        /// <returns>List of experiences for the agent</returns>
        [HttpGet("byAgent/{agentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Agent,Admin")]
        public async Task<ActionResult<IEnumerable<ExperienceDTO>>> GetByAgentId(string agentId)
        {
            _logger.LogInformation("Getting experiences for agent: {AgentId}", agentId);
            var query = new GetExperiencesByAgentQuery { AgentId = agentId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets experiences by status
        /// </summary>
        /// <param name="status">Experience status</param>
        /// <returns>List of experiences with the specified status</returns>
        [HttpGet("byStatus/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ExperienceDTO>>> GetByStatus(string status)
        {
            if (!Enum.TryParse<ExperienceStatus>(status, true, out var experienceStatus))
            {
                return BadRequest($"Invalid status value: {status}");
            }
            
            _logger.LogInformation("Getting experiences with status: {Status}", status);
            var query = new GetExperiencesByStatusQuery { Status = experienceStatus };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new experience
        /// </summary>
        /// <param name="command">Experience creation data</param>
        /// <returns>The created experience ID</returns>
        [HttpPost]
        [Authorize(Roles = "Agent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Create(CreateExperienceCommand command)
        {
            _logger.LogInformation("Creating new experience with title: {Title}", command.Title);
            
            // Get agent ID from current user claims
            command.AgentId = User.FindFirst("sub")?.Value;
            
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        /// <summary>
        /// Updates an existing experience
        /// </summary>
        /// <param name="id">Experience ID</param>
        /// <param name="command">Experience update data</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Agent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(string id, UpdateExperienceCommand command)
        {
            _logger.LogInformation("Updating experience with ID: {ExperienceId}", id);
            
            // Ensure the ID in the route matches the command
            if (id != command.Id)
                return BadRequest("ID in the route does not match ID in the request body");
                
            // Get agent ID from current user claims to ensure only the owner can update
            command.AgentId = User.FindFirst("sub")?.Value;
            
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Publishes an experience making it available for booking
        /// </summary>
        /// <param name="id">Experience ID</param>
        /// <returns>No content</returns>
        [HttpPut("{id}/publish")]
        [Authorize(Roles = "Agent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Publish(string id)
        {
            _logger.LogInformation("Publishing experience with ID: {ExperienceId}", id);
            
            var command = new PublishExperienceCommand
            {
                Id = id,
                AgentId = User.FindFirst("sub")?.Value
            };
            
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes an experience
        /// </summary>
        /// <param name="id">Experience ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Agent,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string id)
        {
            _logger.LogInformation("Deleting experience with ID: {ExperienceId}", id);
            
            var command = new DeleteExperienceCommand 
            { 
                Id = id,
                AgentId = User.FindFirst("sub")?.Value,
                IsAdmin = User.IsInRole("Admin")
            };
            
            await _mediator.Send(command);
            return NoContent();
        }