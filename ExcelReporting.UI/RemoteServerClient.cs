using ExcelReporting.Client;
using ExcelReporting.Client.Pko;

namespace ExcelReporting.UI
{
    internal static class RemoteServerClient
    {
        private static readonly ExcelReportClient Client = new ExcelReportClient("http://localhost:5003");

        public static IPkoExcelReportClient Pko => Client.Pko;
    }
}