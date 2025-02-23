using Glow_Up.Core.Services.Files;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Glow_Up.Services.Files
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{folderName}/{fileName}";
        }

        public async Task<TimeSpan> GetVideoDurationAsync(string filePath)
        {
            var mediaInfo = await FFmpeg.GetMediaInfo(filePath);
            return mediaInfo.Duration;
        }
    }
}
