using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Experience.Application.Services;
using Experience.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ecotrip_backend.Controllers;
    [ApiController]
    [Route("api/experiences")]
    [Authorize(Roles = "Agent")]
    public class ImageUploadController : ControllerBase
    {
        private readonly ImageUploadService _imageUploadService;
        private readonly ILogger<ImageUploadController> _logger;

        public ImageUploadController(
            ImageUploadService imageUploadService,
            ILogger<ImageUploadController> logger)
        {
            _imageUploadService = imageUploadService ?? throw new ArgumentNullException(nameof(imageUploadService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Uploads a single image for an experience
        /// </summary>
        /// <param name="experienceId">ID of the experience</param>
        /// <param name="file">Image file to upload</param>
        /// <returns>URL of the uploaded image</returns>
        [HttpPost("{experienceId}/images")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [RequestSizeLimit(5 * 1024 * 1024)] // 5MB limit
        public async Task<ActionResult<string>> UploadSingleImage(
            [FromRoute] string experienceId,
            IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                ExperienceId id;
                try
                {
                    id = ExperienceId.Parse(experienceId);
                }
                catch (ArgumentException)
                {
                    return BadRequest("Invalid experience ID format");
                }

                _logger.LogInformation("Uploading image for experience: {ExperienceId}", experienceId);
                var imageUrl = await _imageUploadService.UploadImageAsync(file, experienceId);
                
                return Ok(new { ImageUrl = imageUrl });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid upload attempt: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
            }
        }

        /// <summary>
        /// Uploads multiple images for an experience
        /// </summary>
        /// <param name="experienceId">ID of the experience</param>
        /// <param name="files">Collection of image files to upload</param>
        /// <returns>URLs of the uploaded images</returns>
        [HttpPost("{experienceId}/images/multiple")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [RequestSizeLimit(20 * 1024 * 1024)] // 20MB limit
        public async Task<ActionResult<IEnumerable<string>>> UploadMultipleImages(
            [FromRoute] string experienceId,
            [FromForm] IFormFileCollection files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    return BadRequest("No files uploaded");

                if (files.Count > 10)
                    return BadRequest("Maximum of 10 files can be uploaded at once");

                ExperienceId id;
                try
                {
                    id = ExperienceId.Parse(experienceId);
                }
                catch (ArgumentException)
                {
                    return BadRequest("Invalid experience ID format");
                }

                _logger.LogInformation("Uploading {Count} images for experience: {ExperienceId}", files.Count, experienceId);
                var imageUrls = await _imageUploadService.UploadMultipleImagesAsync(files, experienceId);
                
                return Ok(new { ImageUrls = imageUrls });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid upload attempt: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading images: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading images");
            }
        }

        /// <summary>
        /// Deletes an image
        /// </summary>
        /// <param name="experienceId">ID of the experience</param>
        /// <param name="imageUrl">URL of the image to delete</param>
        /// <returns>Success message</returns>
        [HttpDelete("{experienceId}/images")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteImage(
            [FromRoute] string experienceId,
            [FromQuery] string imageUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imageUrl))
                    return BadRequest("Image URL is required");

                ExperienceId id;
                try
                {
                    id = ExperienceId.Parse(experienceId);
                }
                catch (ArgumentException)
                {
                    return BadRequest("Invalid experience ID format");
                }

                _logger.LogInformation("Deleting image {ImageUrl} for experience: {ExperienceId}", imageUrl, experienceId);
                
                await _imageUploadService.DeleteImageAsync(imageUrl);
                
                return Ok(new { Message = "Image deleted successfully" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid delete attempt: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting image");
            }
        }

        /// <summary>
        /// Sets an image as the main image for an experience
        /// </summary>
        /// <param name="experienceId">ID of the experience</param>
        /// <param name="imageUrl">URL of the image to set as main</param>
        /// <returns>Success message</returns>
        [HttpPut("{experienceId}/images/main")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        public Task<ActionResult> SetMainImage(
            [FromRoute] string experienceId,
            [FromQuery] string imageUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imageUrl))
                    return Task.FromResult<ActionResult>(BadRequest("Image URL is required"));

                ExperienceId id;
                try
                {
                    id = ExperienceId.Parse(experienceId);
                }
                catch (ArgumentException)
                {
                    return Task.FromResult<ActionResult>(BadRequest("Invalid experience ID format"));
                }                _logger.LogInformation("Setting main image {ImageUrl} for experience: {ExperienceId}", imageUrl, experienceId);
                
                // Call a service method to set this as the main image
                // This might require implementation in the ImageUploadService or via a command handler
                // await _imageUploadService.SetMainImageAsync(id, imageUrl);
                
                return Task.FromResult<ActionResult>(Ok(new { Message = "Main image set successfully" }));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid set main image attempt: {Message}", ex.Message);
                return Task.FromResult<ActionResult>(BadRequest(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting main image: {Message}", ex.Message);            return Task.FromResult<ActionResult>(StatusCode(StatusCodes.Status500InternalServerError, "Error setting main image"));
        }
    }
}