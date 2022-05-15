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
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public DirectoryService(IDirectoryRepository directoryRepository, IUserRepository userRepository, IConfiguration config)
        {
            _directoryRepository = directoryRepository;
            _userRepository = userRepository;
            _config = config;
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
                throw new Exception("Folder o podanej ścieżce już istnieje");
            }
        }

        public async Task<List<DirectorySelectBoxVM>> GetDirectoriesToSelectList(string username)
        {
            AppUser user = _userRepository.FirstOrDefault(x => x.Username == username) ?? throw new Exception("Użytkownik nie istnieje");
            return await _directoryRepository.GetDirectoriesToSelectList(user.Id, username);
        }

    }
}
