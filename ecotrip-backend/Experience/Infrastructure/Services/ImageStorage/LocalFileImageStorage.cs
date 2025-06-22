using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Experience.Infrastructure.Services.ImageStorage
{
    public class LocalFileImageStorage
    {
        private readonly string _baseDirectory;
        private readonly string _baseUrl;
        private readonly ILogger<LocalFileImageStorage> _logger;
        private readonly string[] _allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

        public LocalFileImageStorage(
            IConfiguration configuration,
            ILogger<LocalFileImageStorage> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // Get configuration values or use defaults
            _baseDirectory = configuration["ImageStorage:LocalFile:BasePath"] 
                ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "experiences");
            
            _baseUrl = configuration["ImageStorage:LocalFile:BaseUrl"] 
                ?? "/uploads/experiences";
            
            // Ensure directory exists
            if (!Directory.Exists(_baseDirectory))
            {
                Directory.CreateDirectory(_baseDirectory);
                _logger.LogInformation("Created image storage directory at {Directory}", _baseDirectory);
            }
        }

        /// <summary>
        /// Save a single image file
        /// </summary>
        /// <param name="file">The image file</param>
        /// <param name="experienceId">ID of the associated experience</param>
        /// <returns>URL to access the saved image</returns>
        public async Task<string> SaveImageAsync(IFormFile file, string experienceId)
        {
            ValidateFile(file);
            
            // Create a directory for this specific experience if it doesn't exist
            var experienceDirectory = Path.Combine(_baseDirectory, experienceId);
            if (!Directory.Exists(experienceDirectory))
            {
                Directory.CreateDirectory(experienceDirectory);
            }
            
            // Generate a unique filename
            var fileName = GetUniqueFileName(file);
            var filePath = Path.Combine(experienceDirectory, fileName);
            
            _logger.LogInformation("Saving image for experience {ExperienceId} to {FilePath}", experienceId, filePath);
            
            // Save the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            // Return the URL path to access the image
            return $"{_baseUrl}/{experienceId}/{fileName}";
        }

        /// <summary>
        /// Save multiple image files
        /// </summary>
        /// <param name="files">Collection of image files</param>
        /// <param name="experienceId">ID of the associated experience</param>
        /// <returns>List of URLs to access the saved images</returns>
        public async Task<IEnumerable<string>> SaveMultipleImagesAsync(IEnumerable<IFormFile> files, string experienceId)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files));
            }
            
            var tasks = files.Select(file => SaveImageAsync(file, experienceId));
            return await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Delete an image by its URL
        /// </summary>
        /// <param name="imageUrl">URL of the image to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public Task<bool> DeleteImageAsync(string imageUrl)
        {
            try
            {
                // Extract file path from URL
                var relativePath = imageUrl.Replace(_baseUrl, string.Empty).TrimStart('/');
                var filePath = Path.Combine(_baseDirectory, relativePath);
                
                _logger.LogInformation("Attempting to delete image at {FilePath}", filePath);
                
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("Image file not found at {FilePath}", filePath);
                    return Task.FromResult(false);
                }
                
                File.Delete(filePath);
                _logger.LogInformation("Successfully deleted image at {FilePath}", filePath);
                  // Try to clean up empty directory if this was the last image
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    TryCleanupDirectory(directoryPath);
                }
                
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image at {ImageUrl}", imageUrl);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Validates that the file meets size and type requirements
        /// </summary>
        /// <param name="file">The file to validate</param>
        private void ValidateFile(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            
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
        /// Generates a unique filename for the image
        /// </summary>
        /// <param name="file">The image file</param>
        /// <returns>A unique filename</returns>
        private string GetUniqueFileName(IFormFile file)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
            var extension = GetFileExtension(file);
            
            return $"{timestamp}-{uniqueId}{extension}";
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

        /// <summary>
        /// Attempts to delete a directory if it's empty
        /// </summary>
        /// <param name="directory">Path to the directory</param>
        private void TryCleanupDirectory(string directory)
        {
            try
            {
                // Check if directory exists and is empty
                if (Directory.Exists(directory) && !Directory.EnumerateFileSystemEntries(directory).Any())
                {
                    Directory.Delete(directory);
                    _logger.LogInformation("Deleted empty directory {Directory}", directory);
                }
            }
            catch (Exception ex)
            {
                // Non-critical error, just log it
                _logger.LogWarning(ex, "Failed to clean up directory {Directory}", directory);
            }
        }
    }
}