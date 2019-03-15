namespace Application.Core.Dto.Setting
{
    public class FileDirectorySettingDto
    {
        public string Id { get; set; }    
        public string DefaultDirectoryPath { get; set; }
        public string DirectoryPath { get; set; }
        public string ArchiveDirectoryPath { get; set; }
    }
}
