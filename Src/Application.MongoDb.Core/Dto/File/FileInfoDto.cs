using System;

namespace KNN.NULLPrinter.Core.Dto.File
{
    public class FileInfoDto
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileAbsPath { get; set; }
        public string FileDescription { get; set; }
        public string Size { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
