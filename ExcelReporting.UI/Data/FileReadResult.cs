namespace ExcelReporting.UI.Data
{
    public class FileReadResult
    {
        public string ErrorDescription { get; set; }

        public byte[] Content { get; set; }

        public bool HasError => !string.IsNullOrEmpty(ErrorDescription);

        public static FileReadResult Error(string error) => new FileReadResult
        {
            ErrorDescription = error
        };

        public static FileReadResult Ok(byte[] fileContent) => new FileReadResult
        {
            Content = fileContent
        };
    }
}