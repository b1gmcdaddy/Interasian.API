using CloudinaryDotNet.Actions;

namespace Interasian.API.Services
{
	public interface IUploadService
	{
		Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
		Task<List<ImageUploadResult>> AddMultiplePhotosAsync(IFormFileCollection files);
		Task<DeletionResult> DeletePhotoAsync(string imageId);
	}
}
