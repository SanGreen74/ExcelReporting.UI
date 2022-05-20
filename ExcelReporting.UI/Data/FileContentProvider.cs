using System;
using System.IO;

namespace ExcelReporting.UI.Data
{
    public static class FileContentProvider
    {
        public static FileReadResult TryRead(string path)
        {
            if (!File.Exists(path))
            {
                return FileReadResult.Error($"Указанный файл не найден. Путь {path}");
            }

            try
            {
                var fileContent = File.ReadAllBytes(path);
                return FileReadResult.Ok(fileContent);
            }
            catch (IOException e)
            {
                return FileReadResult.Error($"Не удалось получить доступ к файлу {path}. Ошибка {e}");
            }
        }
    }
}