using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace api.Interfaces
{
    public interface IPhotoServices
    {
         Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
         Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}