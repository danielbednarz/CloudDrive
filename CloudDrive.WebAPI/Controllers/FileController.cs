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

        [HttpPost]
        public ActionResult Post()
        {
            _fileService.AddFile();

            return Ok();
        }
    }
}
