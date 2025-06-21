using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Experience.Application.Services
{
    /// <summary>
    /// Service for handling experience image uploads
    /// </summary>
    public class ImageUploadService
    {
        private readonly string _uploadDir;
        private readonly ILogger<ImageUploadService> _logger;
        private readonly string[] _allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

        public ImageUploadService(
            IConfiguration configuration,
            ILogger<ImageUploadService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uploadDir = configuration["Upload:Directory"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads", "experiences");
            
            // Ensure upload directory exists
            if (!Directory.Exists(_uploadDir))
            {
                Directory.CreateDirectory(_uploadDir);
                _logger.LogInformation("Created upload directory at {Path}", _uploadDir);
            }
        }

        /// <summary>
        /// Upload a single image for an experience
        /// </summary>
        /// <param name="file">The image file to upload</param>
        /// <param name="experienceId">ID of the experience</param>
        /// <returns>URL path to the uploaded image</returns>
        public async Task<string> UploadImageAsync(IFormFile file, string experienceId)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            _logger.LogInformation("Uploading image for experience {ExperienceId}", experienceId);
            
            // Validate the file
            ValidateFile(file);
            
            // Generate a unique filename
            string fileName = GenerateFileName(file, experienceId);
            string filePath = Path.Combine(_uploadDir, fileName);
            
            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            _logger.LogInformation("Successfully uploaded image {FileName} for experience {ExperienceId}", fileName, experienceId);
            
            // Return the relative URL to access the image
            return $"/uploads/experiences/{fileName}";
        }
        
        /// <summary>
        /// Upload multiple images for an experience
        /// </summary>
        /// <param name="files">Collection of image files</param>
        /// <param name="experienceId">ID of the experience</param>
        /// <returns>List of URL paths to the uploaded images</returns>
        public async Task<IEnumerable<string>> UploadMultipleImagesAsync(IFormFileCollection files, string experienceId)
        {
            if (files == null || !files.Any())
            {
                throw new ArgumentException("No files provided for upload", nameof(files));
            }
            
            _logger.LogInformation("Uploading {Count} images for experience {ExperienceId}", files.Count, experienceId);
            
            var tasks = new List<Task<string>>();
            
            foreach (var file in files)
            {
                tasks.Add(UploadImageAsync(file, experienceId));
            }
            
            return await Task.WhenAll(tasks);
        }
        
        /// <summary>
        /// Delete an image file
        /// </summary>
        /// <param name="imageUrl">URL path of the image to delete</param>
        /// <returns>Task representing the asynchronous operation</returns>
        public Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentException("Image URL cannot be null or empty", nameof(imageUrl));
            }
            
            // Extract the filename from the URL
            string fileName = imageUrl.Split('/').Last();
            string filePath = Path.Combine(_uploadDir, fileName);
            
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Attempted to delete non-existent image file {FilePath}", filePath);
                return Task.CompletedTask;
            }
            
            try
            {
                File.Delete(filePath);
                _logger.LogInformation("Successfully deleted image file {FilePath}", filePath);
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Error deleting image file {FilePath}", filePath);
                throw new ApplicationException($"Failed to delete image file: {ex.Message}", ex);
            }
            
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// Validates that the file meets size and type requirements
        /// </summary>
        /// <param name="file">The file to validate</param>
        private void ValidateFile(IFormFile file)
        {
            // Check file size
            if (file.Length > _maxFileSize)
            {
                _logger.LogWarning("File size {Size} exceeds maximum allowed size of {MaxSize}", 
                    file.Length, _maxFileSize);
                throw new ArgumentException($"File size exceeds maximum allowed size of {_maxFileSize / 1024 / 1024}MB");
            }
            
            // Check file type
            if (!_allowedMimeTypes.Contains(file.ContentType))
            {
                _logger.LogWarning("File type {ContentType} is not allowed", file.ContentType);
                throw new ArgumentException($"File type {file.ContentType} is not allowed. Allowed types: {string.Join(", ", _allowedMimeTypes)}");
            }
        }
        
        /// <summary>
        /// Generates a unique filename for the uploaded image
        /// </summary>
        /// <param name="file">The image file</param>
        /// <param name="experienceId">ID of the experience</param>
        /// <returns>Unique filename</returns>
        private string GenerateFileName(IFormFile file, string experienceId)
        {
            // Get file extension from the content type or original filename
            string extension = GetFileExtension(file);
            
            // Create a unique filename using experience ID, timestamp, and a GUID
            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            
            return $"exp-{experienceId}-{timestamp}-{uniqueId}{extension}";
        }
        
        /// <summary>
        /// Gets the appropriate file extension based on the file's content type
        /// </summary>
        /// <param name="file">The image file</param>
        /// <returns>File extension with dot prefix</returns>
        private string GetFileExtension(IFormFile file)
        {
            // Determine extension from content type
            switch (file.ContentType)
            {
                case "image/jpeg":
                    return ".jpg";
                case "image/png":
                    return ".png";
                case "image/webp":
                    return ".webp";
                default:
                    // Fall back to the original file extension if available
                    return Path.GetExtension(file.FileName);
            }
        }
    }
}