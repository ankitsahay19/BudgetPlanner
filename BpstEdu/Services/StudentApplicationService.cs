using BpstEdu.Data;
using BpstEdu.DBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BpstEdu.Services
{
    public class StudentApplicationService : IStudentApplicationService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StudentApplicationService> _logger;

        public StudentApplicationService(AppDbContext context, ILogger<StudentApplicationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(bool Success, string ErrorMessage)> ProcessStudentApplicationAsync(Application application, IFormFile photo, ModelStateDictionary modelState)
        {
            if (application.UniqueId.Equals(0))
            {
                // Check for duplicate mobile number
                var existingByMobile = _context.Applications.FirstOrDefault(a => a.MobileNumber == application.MobileNumber);
                if (existingByMobile != null)
                {
                    modelState.AddModelError("MobileNumber", "This mobile number is already registered. Please use a different mobile number or contact support.");
                    return (false, "Duplicate Mobile Number");
                }

                // Check for duplicate email
                if (!string.IsNullOrEmpty(application.EmailId))
                {
                    var existingByEmail = _context.Applications.FirstOrDefault(a => a.EmailId == application.EmailId);
                    if (existingByEmail != null)
                    {
                        modelState.AddModelError("EmailId", "This email is already registered. Please use a different email or contact support.");
                        return (false, "Duplicate Email");
                    }
                }

                application.CreatedDate = DateTime.UtcNow.AddMinutes(750);

                // Generate ApplicationId if not already set
                if (string.IsNullOrEmpty(application.ApplicationId))
                {
                    var count = _context.Applications.Count();
                    application.ApplicationId = "BPST" + (count + 1).ToString().PadLeft(5, '0');
                }

                // Handle uploaded photo
                if (photo != null && photo.Length > 0)
                {
                    var allowed = new[] { "image/jpeg", "image/png", "image/jpg" };
                    if (!allowed.Contains(photo.ContentType))
                    {
                        modelState.AddModelError("Photo", "Only JPG/PNG images are allowed.");
                        return (false, "Invalid Photo Type");
                    }
                    if (photo.Length > 2 * 1024 * 1024)
                    {
                        modelState.AddModelError("Photo", "Maximum file size is 2MB.");
                        return (false, "Photo Too Large");
                    }

                    // Create folder structure: wwwroot/applications/{MobileNumber}/
                    // Using application.MobileNumber instead of application.ApplicationId for folder name as per original logic.
                    var applicationsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "applications", application.MobileNumber);
                    if (!Directory.Exists(applicationsFolder))
                        Directory.CreateDirectory(applicationsFolder);

                    // Generate unique filename with extension
                    var fileExtension = Path.GetExtension(photo.FileName);
                    var fileName = $"photo_{DateTime.UtcNow.Ticks}{fileExtension}";
                    var filePath = Path.Combine(applicationsFolder, fileName);

                    // Save the file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    // Store relative path in database - Note: original code used ApplicationId for path, but MobileNumber for folder creation.
                    // This might need clarification if the path is intended to be different.
                    // For now, I'll keep the path consistent with the folder name (MobileNumber).
                    application.PhotoPath = $"/applications/{application.MobileNumber}/{fileName}";
                }

                _context.Add(application);
            }
            else
            {
                _context.Update(application);
            }

            try
            {
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving student application to database.");
                modelState.AddModelError("", "Something went wrong with Data, unable to save changes. Call To 82-9910-1616 for Registration.");
                return (false, "Database Save Error");
            }
        }
    }
}
