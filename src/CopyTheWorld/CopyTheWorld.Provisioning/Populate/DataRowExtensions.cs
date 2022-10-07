namespace DigitalTwin.Provisioning
{
    using System;
    using System.Data;

    internal static class DataRowExtensions
    {
        public static string GetStringValue(this DataRow dataRow, string columnName)
        {
            var value = dataRow[columnName];
            return value != DBNull.Value ? value.ToString()! : string.Empty;
        }

        public static int GetIntValue(this DataRow dataRow, string columnName) => Convert.ToInt32(dataRow.Field<double>(columnName));
    }
}
