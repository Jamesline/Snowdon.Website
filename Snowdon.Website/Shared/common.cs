using BlazorInputFile;
using Snowdon.Website.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Snowdon.Website.Services.EPPlusBuilder;
using static Snowdon.Website.Services.EPPlusBuilder.TablesModel;

namespace Snowdon.Website.Shared
{
    public static class Common
    {
        
        public static string DeciString(string value)
        {
            return String.Format("{0:F}", Convert.ToDecimal(value));
        }
        public static RowModel LocateRow(TableModel table, string SearchWord)
        {
            foreach (RowModel row in table.Body)
            {
                if (row.Cells[0].Value == SearchWord)
                {
                    return row;
                }
            }
            return null;
        }
        public static TableModel FetchTableByName(TablesModel Tables, string tableName)
        {
            foreach (var table in Tables.Tables)
            {
                if (table.Name == tableName)
                {
                    return table;
                }
            }
            return null;
        }
        public static int FindRowByCellContent(TableModel table, string SearchWord, int Col, int Row)
        {
            for (int i = Row; i < table.Body.Count; i++)
            {
                string cellValue = FetchCellFromRow(table.Body[i], Col);
                if (cellValue == SearchWord)
                {
                    return i;
                }
            }
            return 0;
        }
        public static RowModel ReadRow(TableModel table, int Row)
        {
            for (int i = Row; i < table.Body.Count; i++)
            {
                if ((i == Row))
                // if ((i == Row) && (EmptyCellChecker(table, Row, 0) == false))
                {
                    return table.Body[Row];
                }
            }
            return null;
        }
        public static RowModel ReadRow(TableModel table, int Row,int Col)
        {
            for (int i = Row; i < table.Body.Count; i++)
            {
                if ((i == Row))
                // if ((i == Row) && (EmptyCellChecker(table, Row, 0) == false))
                {
                    return table.Body[Row];
                }
            }
            return null;
        }
        public static RowModel SizeRow(RowModel Row, int Size, int StartCol)
        {
            RowModel retRow = new RowModel();
            for (int i = StartCol; i<(StartCol+Size); i++)
            {
                string cellValue = Row.Cells[i].Value;
                CellModel cellModel = new CellModel();
                cellModel.Value = cellValue;
                retRow.Cells.Add(cellModel);
            }
            return retRow;
        }
        public static RowModel CellTyper(RowModel Row)
        {
            RowModel retRow = new RowModel();
            foreach (var cell in Row.Cells)
            {
                CellModel cellModel = new CellModel();
                if (IsNumeric(cell.Value.ToString()))
                {
                    cellModel.Value = DeciString(cell.Value.ToString());
                }
                else
                {
                    cellModel.Value = cell.Value;
                }
                retRow.Cells.Add(cellModel);
            }
            return retRow;
        }
        public static bool EmptyCellChecker(TableModel table, int Row, int Col)
        {
            bool result = string.IsNullOrEmpty(table.Body[Row].Cells[Col].Value);
            return result;
        }
        public static bool EmptyCellChecker(RowModel Row, int Col)
        {
            bool result = string.IsNullOrEmpty(Row.Cells[Col].Value);
            return result;
        }
        public static RowModel GetRow(TableModel table, int RowNumber)
        {
            int rowCount = 1;
            foreach (var row in table.Body)
            {
                if (rowCount == RowNumber)
                {
                    return row;
                }
                rowCount += 1;

            }
            return null;
        }
        public static CellModel GetCell(RowModel row, int cellNumber)
        {
            int cellCount = 1;
            foreach (var cell in row.Cells)
            {
                if (cellCount == cellNumber)
                {
                    return cell;
                }
                cellCount += 1;
            }
            return null;
        }
        public static bool IsNumeric(string cell)
        {
            decimal myDec;
            var Result = decimal.TryParse(cell, out myDec);
            return Result;
        }

        private static string FetchCellFromRow(RowModel row, int col)
        {
            for (int i = 0; i < row.Cells.Count; i++)
            {
                CellModel Cell = row.Cells[i];
                if (i == col)
                {
                    return Cell.Value;
                }
            }
            return " ";
        }
    }
    public class Commonasync
    {
        public async Task<MemoryStream> FileSelection(IFileListEntry[] files)
        {
            MemoryStream memoryStream = new MemoryStream();
            var fileloaded = false;
            var file = files.FirstOrDefault();
            if (file != null)
            {
                var ms = new MemoryStream();
                memoryStream = ms;
                await file.Data.CopyToAsync(ms);

                var status = $"Finished loading {file.Size} bytes from {file.Name}";
                fileloaded = true;
            }
            return memoryStream;
        }
    }



}
