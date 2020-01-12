using Snowdon.Website.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Snowdon.Website.Services.EPPlusBuilder.TablesModel;

namespace Snowdon.Website.Services
{
    public static class CogeBuilder
    {
        private static int _customerRowStart = 0;
        private static int _customerRowEnd = 0;
        public static TableModel BuildCoge(TableModel table)
        {
            TableModel cogeTable = new TableModel();
            _customerRowStart = Common.FindRowByCellContent(table, "CUSTOMERS", 0, 0);
            _customerRowEnd = Common.FindRowByCellContent(table, "TOTAL", 0, _customerRowStart);

            for (int i = _customerRowStart; i < _customerRowEnd + 1; i++)
            {
                var CogeRow = Common.ReadRow(table, i);
                CogeRow = Common.SizeRow(CogeRow, 14, 0);
                CogeRow = Common.CellTyper(CogeRow);
                if ((Common.EmptyCellChecker(CogeRow, 0) == true)) //|| (common.EmptyCellChecker(CogeRow, 13) == true)|| (common.IsNumeric(CogeRow.Cells[0].Value.ToString())==false))
                {
                }
                else
                {
                    cogeTable.Body.Add(CogeRow);
                }
            }
            return cogeTable;
        }
    }
    //public class CogeAsyncBuilder
    //{
    //    private EPPlusBuilder.TablesModel Tables = new EPPlusBuilder.TablesModel();
    //    private readonly RowModel rowModel = new RowModel();
    //    public async Task<CogeReturn> DecodeCogeFile(MemoryStream memoryStream)
    //    {
    //        CogeReturn cogeReturn = new CogeReturn();
    //        try
    //        {
    //            var fileInRam = memoryStream;
    //            using (var package = new OfficeOpenXml.ExcelPackage(fileInRam))
    //            {
    //                if (package != null)
    //                {
    //                    Tables = EPPlusBuilder.Parse(package, false);
    //                }
    //            }
    //            foreach (var table in Tables.Tables)
    //            {
    //                if (table.Name == "REV_CUST")
    //                {
    //                    cogeReturn.cogeRowModel = CogeBuilder.BuildCoge(table);
    //                }
    //            }
    //            return cogeReturn;
    //        }
    //        catch (Exception ex)
    //        {
    //            cogeReturn.Status = "File failed decoding: " + ex.Message;
    //            return cogeReturn;

    //        }
    //    }

    //}
    public class CogeReturn
    {
        public RowModel cogeRowModel { get; set; }
        public string Status { get; set; }

    }
}
