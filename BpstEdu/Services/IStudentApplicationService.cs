using BpstEdu.DBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace BpstEdu.Services
{
    public interface IStudentApplicationService
    {
        Task<(bool Success, string? ErrorMessage)> ProcessStudentApplicationAsync(Application application, IFormFile photo, ModelStateDictionary modelState);
    }
}
