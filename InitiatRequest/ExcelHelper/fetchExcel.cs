using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.IO;

namespace InitiatRequest
{
    /// <summary>
    /// 读取Excel
    /// </summary>
    public class fetchExcel
    {
        public Dictionary<string, string> readexcel(string filename)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            if (!File.Exists(filename))
            {
                // 创建这个Excel
                using (new FileStream(filename, FileMode.OpenOrCreate))
                {  }
                return pairs;
            }

            FileStream fileStream = new FileStream(filename, FileMode.Open);
            Workbook workbook = new Workbook(fileStream);
            WorksheetCollection worksheets= workbook.Worksheets;
            foreach (Worksheet worksheet in worksheets)
            {
                //var datatable = worksheet.Cells.ExportDataTable(0,0, worksheet.Cells.MaxRow, worksheet.Cells.MaxColumn);//单元格转DataTable
                var cells = worksheet.Cells;  /*.ExportDataTable(0,0, worksheet.Cells.MaxRow, worksheet.Cells.MaxColumn);*/
                for (int r = 0; r < worksheet.Cells.MaxRow+1; r++)
                {
                    for (int c = 0; c < worksheet.Cells.MaxColumn + 1; c++)
                    {
                        int col = c;
                        try
                        {
                            string key = cells[r, col].Value.ToString();
                            col += 1;
                            string value = cells[r, col].Value.ToString();
                            pairs.Add(key, value);
                            break;
                        }
                        catch (Exception e)
                        {
                            log4helper<ReadExcel>.Info($"在第{r}行的第{col}列没有数据，引发异常！");
                            log4helper<ReadExcel>.Errror(e);
                            break;
                        }
                    }
                }
            }
            // keyi tong uo

            fileStream.Flush();
            fileStream.Close();
            worksheets.Clear();
            return pairs;
        }
    }
}
