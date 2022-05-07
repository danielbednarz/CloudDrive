using CloudDrive.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace CloudDrive.WebAPI
{
    public class FileController : AppController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [Authorize]
        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile()
        {
            var file = Request.Form.Files.FirstOrDefault();
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (file.Length > 0)
            {
                AddUserFileVM userFile = new()
                {
                    File = file,
                    Username = loggedUsername,
                };

                await _fileService.AddFile(userFile);
            }

            return Ok();
        }
    }
}
