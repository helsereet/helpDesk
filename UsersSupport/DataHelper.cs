using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using UsersSupport.Data;

namespace UsersSupport
{
    public static class DataHelper
    {
        public static void BindDataGridView<T>(List<T> dataSource, DataGridView dataGridView)
        { dataGridView.DataSource = dataSource; }

        public static void MakeColumnsInvisible(DataGridView dataGridView, List<string> InvisibleColumns)
        {
            foreach (var column in InvisibleColumns)
                dataGridView.Columns[column].Visible = false;
        }

        public static void MakeAllColumnsInvisible(DataGridView dataGridView)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
                column.Visible = false;
        }

        public static void MakeColumnsVisible(DataGridView dataGridView, List<string> VisibleColumns)
        {
            foreach (var column in VisibleColumns)
                dataGridView.Columns[column].Visible = true;
        }

        public static void UpdateFontOfDataGridView(DataGridView dataGrid, Dictionary<string, object> dataGridStyle)
        {
            dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(
                    dataGridStyle["Font"] as string,
                    (float)dataGridStyle["FontSize"],
                    (FontStyle)dataGridStyle["FontStyle"]
                );
        }

        public static void DeleteSelectedRecord(DataGridView dataGrid, Action<string, int> dbMethod, string tableName)
        {
            if (dataGrid.CurrentCell != null)
            {
                var Id = Convert.ToInt32(dataGrid[dataGrid.Columns["Id"].Index,
                                                 dataGrid.CurrentRow.Index].Value);
                dbMethod(tableName, Id);
            }
        }

        public static void BindListBox<T>(List<T> dataSource, ListBox listBox, Dictionary<string, string> pairs)
        {
            listBox.DataSource = dataSource;
            listBox.DisplayMember = pairs["DisplayMember"];
            listBox.ValueMember = pairs["ValueMember"];
        }

        internal static void MakeColumnsOrder(DataGridView dataGridView, List<string> order)
        {
            for (int i = 0; i < order.Count; i++)
                dataGridView.Columns[order[i]].DisplayIndex = i;
        }

        public static void BindChartStatistics(Chart chart, List<PerformerStatistics> performers)
        {
            chart.DataSource = performers;

            chart.Series["Исполнители"].XValueMember = "FIO";
            chart.Series["Исполнители"].YValueMembers = "TotalPoints";
            chart.Series["Исполнители"].IsValueShownAsLabel = true;
        }
    }
}
