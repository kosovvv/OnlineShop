﻿using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.Infrastructure;
using OnlineShop.Web.ViewModels.Image;

namespace OnlineShop.WebAPI.Controllers
{
    public class ImagesController : BaseApiController
    {
        private readonly IImageService imageService;

        public ImagesController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpGet("{folderName}/{imageName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Cached(600)]
        public async Task<ActionResult> GetImage(string folderName, string imageName)
        {
            var imageData = await imageService.GetImageAsync(imageName);

            if (imageData == null)
            {
                return NotFound();
            }

            var contentType = GetContentType(imageName);

            return File(imageData, contentType);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage(ImageUploadDto model)
        {
            using var stream = model.Image.OpenReadStream();
            var imageId = await imageService.UploadImageAsync(model.Image.FileName, stream);
            return Ok(new { ImageId = imageId });
        }

        private static string GetContentType(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName)?.ToLowerInvariant();
            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                default:
                    return "application/octet-stream";
            }
        }

    }
}
