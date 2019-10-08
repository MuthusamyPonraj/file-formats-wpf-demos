#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System.Windows.Input;
using System.Data;
using ImportDataTable.Model;
using Syncfusion.UI.Xaml.Spreadsheet;

namespace ImportDataTable.Commands
{
    public static class ImportCommand
    {
        static ImportCommand()
        {
            CommandManager.RegisterClassCommandBinding(typeof(SfSpreadsheet), new CommandBinding(ImportFromDataTable, OnExecuteImportFromDataTable, OnCanExecuteImportFromDataTable));
        }

        public static RoutedCommand ImportFromDataTable = new RoutedCommand("ImportFromDataTable", typeof(SfSpreadsheet));

        private static void OnExecuteImportFromDataTable(object sender, ExecutedRoutedEventArgs args)
        {
            SfSpreadsheet Spreadsheet = args.Source as SfSpreadsheet;
            if (Spreadsheet != null && args.Parameter != null)
            {
                Spreadsheet.Create(3);
                string SelectedItem = args.Parameter.ToString();
                DataTable datatable;
                if (SelectedItem.Contains("Products"))
                    datatable = Data.GetDataTable("Products");
                else if (SelectedItem.Contains("Orders"))
                    datatable = Data.GetDataTable("Orders");
                else
                    datatable = Data.GetDataTable("Customers");

                Spreadsheet.ActiveSheet.ImportDataTable(datatable, true, 1, 1);
                for (int i = 1; i <= Spreadsheet.ActiveSheet.UsedRange.LastColumn; i++)
                {
                    Spreadsheet.ActiveSheet.AutofitColumn(i);
                    Spreadsheet.ActiveGrid.SetColumnWidth(i, i, Spreadsheet.ActiveSheet.GetColumnWidthInPixels(i));
                }
                Spreadsheet.ActiveGrid.InvalidateCells();
            }
        }

        private static void OnCanExecuteImportFromDataTable(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }
    }
}
