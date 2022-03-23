using CloudDrive.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace CloudDrive.WebAPI
{
    public class FileController : AppController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile()
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file.Length > 0)
            {
                AddUserFileVM userFile = new()
                {
                    File = file
                };

                await _fileService.AddFile(userFile);
            }

            return Ok();
        }
    }
}
