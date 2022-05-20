using ExcelReporting.Client;
using ExcelReporting.Client.Pko;

namespace ExcelReporting.UI
{
    internal static class RemoteServerClient
    {
        //http://nsatin.ru
        private static readonly ExcelReportClient Client = new ExcelReportClient("http://nsatin.ru");

        public static IPkoExcelReportClient Pko => Client.Pko;
    }
}