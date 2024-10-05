using System.Data;
using System.Xml;
using OfficeOpenXml;

namespace ScoreAnnouncementSoftware.Models.Process
{
    public class CCCNTTCBProcess
    {
        public DataTable GetStudentTableDistinctFromCCCNTTCB(DataTable originalDataTable)
        {
            //string[] columnNamesToClone = { "Column0", "Column2", "Column3", "Column4" };
            DataTable clonedTable = originalDataTable.Copy();
            clonedTable.Columns.RemoveAt(10);
            clonedTable.Columns.RemoveAt(9);
            clonedTable.Columns.RemoveAt(8);
            clonedTable.Columns.RemoveAt(0);
            var distinctDataTable = clonedTable.Clone();
            try
            {
                var uniqueRows = new HashSet<string>();
                foreach (DataRow row in clonedTable.Rows)
                {
                    if (uniqueRows.Add(row[0].ToString()))
                    {
                        distinctDataTable.ImportRow(row);
                    }
                }
            }
            catch
            {
            }
            return distinctDataTable;
        }

    }
}
