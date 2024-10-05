using System.Data;
using System.Xml;
using OfficeOpenXml;

namespace ScoreAnnouncementSoftware.Models.Process
{
    public class CDRCNTTCBProcess
    {
        public DataTable GetStudentTableDistinctFromCDRCNTTCB(DataTable originalDataTable)
        {
            //string[] columnNamesToClone = { "Column0", "Column2", "Column3", "Column4" };
            DataTable clonedTable = originalDataTable.Copy();
            clonedTable.Columns.RemoveAt(7);
            clonedTable.Columns.RemoveAt(6);
            clonedTable.Columns.RemoveAt(5);
            clonedTable.Columns.RemoveAt(4);
            clonedTable.Columns.RemoveAt(3);
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
