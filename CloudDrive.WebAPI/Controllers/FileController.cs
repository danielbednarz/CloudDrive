using CloudDrive.Application;
using CloudDrive.Domain;
using CloudDrive.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CloudDrive.WebAPI
{
    [Authorize]
    public class FileController : AppController
    {
        private readonly IFileService _fileService;
        private readonly IDirectoryService _directoryService;
        private readonly IHubContext<FileHub> _hubContext;

        public FileController(IFileService fileService, IDirectoryService directoryService, IHubContext<FileHub> hubContext)
        {
            _fileService = fileService;
            _directoryService = directoryService;
            _hubContext = hubContext;
        }

        #region Files

        [HttpGet("getUserFiles")]
        public async Task<IActionResult> GetUserFiles()
        {
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            List<FileDataDTO> list = await _fileService.GetUserFiles(loggedUsername);

            return Ok(list);
        }

        [HttpGet("getUserDriveDataToTreeView")]
        public async Task<IActionResult> GetUserDriveDataToTreeView(bool showDeleted)
        {
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            List<UserItemDTO> list = await _directoryService.GetUserDriveDataToTreeView(loggedUsername, showDeleted);
            return Ok(list);
        }

        [RequestSizeLimit(2000 * 1024 * 1024)]
        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile()
        {
            var files = Request.Form.Files;
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var dictionaryId = Request.Form.FirstOrDefault(x => x.Key == "DirectoryId");
                    AddUserFileVM userFile = new()
                    {
                        File = file,
                        Username = loggedUsername,
                        DirectoryId = !string.IsNullOrEmpty(dictionaryId.Value) ? Guid.Parse(dictionaryId.Value) : null
                    };

                    UserFile addedFile = await _fileService.AddFile(userFile);
                    await _hubContext.Clients.Group(loggedUsername).SendAsync("FileAdded", default, addedFile.Name);
                }
            }

            return Ok();
        }

        [HttpPost("uploadFileByFileWatcher")]
        public async Task<IActionResult> UploadFileByFileWathcer(string relativePath)
        {
            var file = Request.Form.Files.FirstOrDefault();
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            if (file.Length > 0)
            {
                AddUserFileVM userFile = new()
                {
                    File = file,
                    Username = loggedUsername,
                };

                UserFile addedFile = await _fileService.AddFileByFileWatcher(userFile, relativePath);
                await _hubContext.Clients.Group(loggedUsername).SendAsync("FileAdded", default, addedFile.Name);
            }

            return Ok();
        }

        [HttpDelete("deleteFile")]
        public async Task<IActionResult> DeleteFile(string relativePath)
        {
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            await _fileService.DeleteFile(relativePath, loggedUsername);

            return Ok();
        }

        [HttpGet("getFileVersions")]
        public async Task<IActionResult> GetFileVersions(Guid fileId)
        {
            List<FileVersionDTO> fileVersions = await _fileService.GetFileVersions(fileId);
            return Ok(fileVersions);
        }

        [HttpPost("selectFileVersion")]
        public async Task<IActionResult> SelectFileVersion(Guid id)
        {
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            await _fileService.SelectFileVersion(id, loggedUsername);
            return Ok();
        }

        [HttpGet("downloadFile")]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            DownloadFileDTO downloadedFile = await _fileService.DownloadFile(fileId, loggedUsername);

            if (downloadedFile == null)
            {
                return NotFound("Brak pliku do pobrania");
            }

            return File(downloadedFile.Bytes, downloadedFile.UserFile.ContentType, downloadedFile.UserFile.Name);
        }

        #endregion Files

        #region Directories

        [HttpPost("addDirectory")]
        public async Task<IActionResult> AddDirectory(AddDirectoryVM model)
        {
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            await _directoryService.AddDirectory(model, loggedUsername);

            return Ok();
        }
        
        [HttpGet("getDirectoriesToSelectList")]
        public async Task<IActionResult> GetDirectoriesToSelectList()
        {
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            List<DirectorySelectBoxVM> list = await _directoryService.GetDirectoriesToSelectList(loggedUsername);

            return Ok(list);
        }

        [HttpGet("downloadSelectedFiles")]
        public async Task<IActionResult> DownloadSelectedFiles([FromQuery]List<Guid> fileIds)
        {
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            DownloadDirectoryDTO downloadDirectoryDTO = await _directoryService.CreateSelectedFilesDirectory(fileIds, loggedUsername);

            return File(downloadDirectoryDTO.Bytes, "application/zip", downloadDirectoryDTO.DirectoryName);
        }

        [HttpGet("downloadDirectory")]
        public async Task<IActionResult> DownloadDirectory(Guid directoryId)
        {
            var loggedUsername = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (loggedUsername == null)
            {
                return NotFound("Błąd przy próbie znalezienia użytkownika");
            }

            DownloadDirectoryDTO downloadDirectoryDTO = await _directoryService.CreateCompressedDirectory(directoryId);

            if (downloadDirectoryDTO == null)
            {
                return NotFound("Błąd przy próbie pobrania folderu");
            }

            return File(downloadDirectoryDTO.Bytes, "application/zip", downloadDirectoryDTO.DirectoryName);
        }

        #endregion Directories
    }
}