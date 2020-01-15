using Snowdon.Website.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Snowdon.Website.Services.EPPlusBuilder.TablesModel;

namespace Snowdon.Website.Services
{


    public static class GecoBuilder
    {
        private static readonly int _GecoStartCol = 5;
        private static readonly int _GecoStartRow = 20;
        private static readonly int _GecoMonthHeader = 18;
        private static int _CustomerRowEnd = 0;
        //private static Models.GecoPosition gecoPositions;
        public static TableModel BuildGeco(TableModel table)
        {
            List<Models.GecoPosition> PostionsGeco = DetermineCustomerTotalPositions(table);
            //Build a display table
            TableModel GecoGatherTable = GecoGather(PostionsGeco,table);
            

            return GecoGatherTable;
        }

        private static List<Models.GecoPosition> DetermineCustomerTotalPositions(TableModel table)
        {
            List<Models.GecoPosition> GecoPostions = new List<Models.GecoPosition>();
            _CustomerRowEnd = Common.FindRowByCellContent(table, "Total", 0, _GecoStartRow);
            Console.WriteLine("End row is :" + _CustomerRowEnd);
            for (int i = _GecoStartRow; i < _CustomerRowEnd + 1; i++)
            {
                Models.GecoPosition gecoPosition = new Models.GecoPosition();
                var GecoRow = Common.ReadRow(table, i);
                GecoRow = Common.CellTyper(GecoRow);
                if (Common.EmptyCellChecker(GecoRow, 0))
                {

                }
                else
                {
                    Console.WriteLine("Customer: " + GecoRow.Cells[0].Value);
                    Console.WriteLine("Line # :" + i);
                    gecoPosition.CustomerRow = i;
                    gecoPosition.CustomerName = GecoRow.Cells[0].Value;
                    gecoPosition.TotalRow = Common.FindRowByCellContent(table, "Tot.Customer", 1, i);
                    GecoPostions.Add(gecoPosition);
                    Console.Write(gecoPosition.CustomerName);
                }

            }
           return GecoPostions;
        }
        private static TableModel GecoGather(List<Models.GecoPosition> gecoPositions, TableModel table)
        {
            TableModel displayTable = new TableModel(); 
            for (int i = 0; i < gecoPositions.Count; i++)
            {
                var GecoRow = new RowModel();
                GecoRow = ParseGecoRow(table, gecoPositions[i].CustomerName, gecoPositions[i].TotalRow);
                displayTable.Body.Add(GecoRow);

            }

                return displayTable;
        }
        private static RowModel ParseGecoRow(TableModel table,string CustomerName, int TotalRow)
        {
            int counter = 1;
            RowModel displayRow = new RowModel();
            CellModel cellName = new CellModel();
            cellName.Value = CustomerName;
            displayRow.Cells.Add(cellName);
            for (int i = 6; i < 45; i += 3)
            {
                var Row = Common.ReadRow(table, TotalRow);
                CellModel cell = new CellModel();
                if (Common.EmptyCellChecker(Row,i))
                {
                    cell.Value = "0";
                }
                else
                {
                    cell = Common.GetCell(Row, i);
                    Console.WriteLine(cell.Value);
                }
                counter++;
                displayRow.Cells.Add(cell);
            }
            

            return displayRow;
        }

        public static RowModel BuildGecoHeader(TableModel table)
        {
            RowModel rowModel = GetHeaderRow(table);
            return rowModel != null ? rowModel : null;
        }
        public static RowModel BuildGecoRow(TableModel table)
        {
            RowModel rowModel = GetRowData(table);
            return rowModel != null ? rowModel : null;
        }
        private static RowModel GetHeaderRow(TableModel table)
        {
            var row = Common.GetRow(table, _GecoMonthHeader);
            return row != null ? GetRowCommon(row, true) : null;
        }
        private static RowModel GetRowData(TableModel table)
        {
            var row = Common.GetRow(table, _GecoStartRow);
            return row != null ? GetRowCommon(row, false) : null;
        }
        private static RowModel GetRowCommon(RowModel row, bool Header)
        {
            int intervalCount = 0;
            var gecoRow = new RowModel();
            for (int i = 0; i < row.Cells.Count; i++)
            {
                CellModel cell = row.Cells[i];
                if (i > _GecoStartCol)
                {
                    intervalCount += 1;
                    if (intervalCount == 1)
                    {
                        var pandlCell = new CellModel();
                        var tempCell = cell.Value;
                        if (tempCell == string.Empty)
                        {
                            tempCell = Header == false ? "0.00" : i == 42 ? "Total" : " ";
                        }
                        else
                        {
                            if (Header == false)
                            {
                                tempCell = Common.DeciString(tempCell);
                            }
                            else
                            {
                            }
                        }
                        pandlCell.Value = tempCell;
                        gecoRow.Cells.Add(pandlCell);
                    }
                    if (intervalCount == 3)
                    {
                        intervalCount = 0;
                    }
                }
                //colCount += 1;
                if (i == 44)
                {
                    return gecoRow;
                }
            }
            return gecoRow;
        }
    }

    public class GecoReturns
    {
        private EPPlusBuilder.TablesModel Tables = new EPPlusBuilder.TablesModel();
        private readonly RowModel rowModel = new RowModel();
        public RowModel GecoHeaderRowModel { get; set; }
        public RowModel GecoDataRowModel { get; set; }
        public string Status { get; set; }
    }
}
