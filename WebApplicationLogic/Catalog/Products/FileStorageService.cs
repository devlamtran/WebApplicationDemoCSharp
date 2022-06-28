using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace WebApplicationLogic.Catalog.Products
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private readonly string _userContentProfileFolder;
        private const string USER_CONTENT_FOLDER_NAME = "image-product";
        private const string USER_CONTENT_Profile_FOLDER_NAME = "image-user";

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);
            _userContentProfileFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_Profile_FOLDER_NAME);
        }



        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
        public async Task SaveFileUserAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentProfileFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
    }
}
