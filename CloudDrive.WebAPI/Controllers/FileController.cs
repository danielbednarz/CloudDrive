using CloudDrive.Application;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
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
