using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherWinForms
{
    internal class FileDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public long FileVersion { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string RelativePath { get; set; }
        public Guid? DirectoryId { get; set; }
        public Guid? ParentDictoryId { get; set; }
    }
}
