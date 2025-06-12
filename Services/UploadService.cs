using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Interasian.API.Utilities;
using Microsoft.Extensions.Options;

namespace Interasian.API.Services
{
	public class UploadService : IUploadService
	{

		private readonly Cloudinary _cloudinary;

		public UploadService(IOptions<CloudinarySettings> config)
		{
			var acc = new Account(
				config.Value.CloudName,
				config.Value.ApiKey,
				config.Value.ApiSecret
				);
			_cloudinary = new Cloudinary(acc);
		}
		// upload single image
		public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
		{
			var uploadResult = new ImageUploadResult();

			if (file.Length > 0)
			{
				using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream)
				};
				uploadResult = await _cloudinary.UploadAsync(uploadParams);
			}
			return uploadResult;
		}

		// upload multiple images
		public async Task<List<ImageUploadResult>> AddMultiplePhotosAsync(IFormFileCollection files)
		{
			var uploadResults = new List<ImageUploadResult>();

			foreach (var file in files)
			{
				if (file.Length > 0)
				{
					using var stream = file.OpenReadStream();
					var uploadParams = new ImageUploadParams
					{
						File = new FileDescription(file.FileName, stream)
					};
					var uploadResult = await _cloudinary.UploadAsync(uploadParams);
					uploadResults.Add(uploadResult);
				}
			}

			return uploadResults;
		}

		public async Task<DeletionResult> DeletePhotoAsync(string imageId)
		{
			var deleteParams = new DeletionParams(imageId);
			var result = await _cloudinary.DestroyAsync(deleteParams);
			return result;
		}
	}
}
