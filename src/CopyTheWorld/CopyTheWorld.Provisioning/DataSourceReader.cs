namespace CopyTheWorld.Provisioning
{
    using System.Data;
    using ExcelDataReader;

    internal sealed class DataSourceReader
    {
        public DataSet GetDataSet(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = dataReader => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            });

            return result;
        }
    }
}
