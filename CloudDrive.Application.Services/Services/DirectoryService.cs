using CloudDrive.Application.Abstraction;
using CloudDrive.Data.Abstraction;
using CloudDrive.Domain;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace CloudDrive.Application
{
    public class DirectoryService : IDirectoryService
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public DirectoryService(IDirectoryRepository directoryRepository, IFileRepository fileRepository, IUserRepository userRepository, IConfiguration config)
        {
            _directoryRepository = directoryRepository;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<List<UserDirectoryDTO>> GetUserDriveDataToTreeView(string username)
        {
            UserDirectory mainDirectory = _directoryRepository.FirstOrDefault(x => x.RelativePath == username) ?? throw new Exception("Uzytkownik nie posiada katalogu");
            List<UserDirectory> listDirectories = await _directoryRepository.GetUserDriveDataToTreeView(username, mainDirectory.Id);
            List<UserFile> listFiles = await _fileRepository.GetUserDriveFilesToTreeView(mainDirectory.Id);
            List<UserDirectoryDTO> listDTO = FromUserDirectoryToDTO(listDirectories);
            listDTO.AddRange(FromUserFileToDTO(listFiles));

            return listDTO;
        }

        private List<UserDirectoryDTO> FromUserDirectoryToDTO(List<UserDirectory> userDirectory)
        {
            return userDirectory.Select(x => new UserDirectoryDTO()
            {
                Id = x.Id,
                Name = x.Name,
                RelativePath = x.RelativePath,
                Children = (x.ChildDirectories != null ? FromUserDirectoryToDTO(x.ChildDirectories.ToList()) : null).Concat(x.Files != null ? FromUserFileToDTO(x.Files.ToList()) : null).ToList(),
                Icon = "fa-solid fa-folder",
                IsFile = false
            }).ToList();
        }

        private List<UserDirectoryDTO> FromUserFileToDTO(List<UserFile> userFile)
        {
            return userFile.Where(x => !x.IsDeleted).GroupBy(x => x.RelativePath).Select(y => y.OrderByDescending(z => z.FileVersion).First()).Select(x => new UserDirectoryDTO()
            {
                Id = x.Id,
                Name = x.Name,
                RelativePath = x.RelativePath,
                Icon = "fa-solid fa-file",
                IsFile = true
            }).ToList();
        }

        private string CreateDirectoryOnServer(string userChosenPath)
        {
            string mainPath = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>().SaveFilePath;
            string path = Path.Combine(mainPath, userChosenPath);

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return path;
        }

        public async Task AddDirectory(AddDirectoryVM model, string username)
        {
            if (!model.ParentDirectoryId.HasValue)
            {
                model.ParentDirectoryId = _directoryRepository.FirstOrDefault(x => x.RelativePath == username).Id;
            }

            UserDirectory parentDirectory = _directoryRepository.FirstOrDefault(x => x.Id == model.ParentDirectoryId) ?? throw new Exception("Wybrany folder nadrzedny nie istnieje");
            model.GeneratedPath = Path.Combine(parentDirectory.RelativePath, model.Name);

            AppUser user = _userRepository.FirstOrDefault(x => x.Username == username) ?? throw new Exception("Użytkownik nie istnieje");
            model.UserId = user.Id;

            if (_directoryRepository.IsDirectoryUnique(model.GeneratedPath))
            {
                CreateDirectoryOnServer(model.GeneratedPath);
                await _directoryRepository.AddDirectory(model);
            }
            else
            {
                return;
                //throw new Exception("Folder o podanej ścieżce już istnieje");
            }
        }

        public async Task<List<DirectorySelectBoxVM>> GetDirectoriesToSelectList(string username)
        {
            AppUser user = _userRepository.FirstOrDefault(x => x.Username == username) ?? throw new Exception("Użytkownik nie istnieje");
            return await _directoryRepository.GetDirectoriesToSelectList(user.Id, username);
        }

    }
}
