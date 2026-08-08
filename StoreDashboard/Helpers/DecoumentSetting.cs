namespace StoreDashboard.Helpers
{
    public static class DecoumentSetting
    {
        public static string UploadFile(IFormFile File, string FolderName)
        {
            // 1. Get Folder Path
            var FolderPath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\imagess",FolderName);
            // 2. Set FileName UINQUE
            var FileName = Guid.NewGuid() + File.FileName;
            // 3. Get File Path
            var FilePath=Path.Combine(FolderPath,FileName);
            // 4. Save File as Streams
            var stream=new FileStream(FilePath, FileMode.Create);
            // 5. Copy File Into Streams
            File.CopyTo(stream);
            // 6. Retun FileName
            return Path.Combine("imagess\\products", FileName);
        }
        public static void DeleteFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imagess", folderName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
