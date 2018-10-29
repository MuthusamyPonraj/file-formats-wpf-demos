#region Copyright Syncfusion Inc. 2001 - 2012
// Copyright Syncfusion Inc. 2001 - 2012. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Windows;
using System.Windows.Media;
using Syncfusion.XlsIO;
using System.ComponentModel;
using Syncfusion.Windows.Shared;

namespace ConditionalFormatting
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : ChromelessWindow
    {
        # region Constructor
        /// <summary>
        /// Window constructor
        /// </summary>
        public Window1()
        {
            InitializeComponent();
            ImageSourceConverter img = new ImageSourceConverter();
            string file = @"..\..\..\..\..\..\..\Common\Images\XlsIO\xlsio_header.png";
            this.image1.Source = (ImageSource)img.ConvertFromString(file);
        }
        # endregion

        # region Events
        /// <summary>
        /// Creates spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            string OutputFileName = "";            

            IWorkbook myWorkbook = excelEngine.Excel.Workbooks.Open(@"..\..\..\..\..\..\..\Common\Data\XlsIO\CFTemplate.xlsx");
            IWorksheet sheet = myWorkbook.Worksheets[0];

            //Set the Default Version as Excel 97to2003
            if (this.rdButtonxls.IsChecked.Value)
            {
                myWorkbook.Version = ExcelVersion.Excel97to2003;
                OutputFileName = "AdvancedCF.xls";
            }
            //Set the Default Version as Excel 2007
            else if (this.rdButtonxlsx.IsChecked.Value)
            {
                myWorkbook.Version = ExcelVersion.Excel2007;
                OutputFileName = "AdvancedCF.xlsx";
            }
            //Set the Default Version as Excel 2010
            else if (this.rdButtonexcel2010.IsChecked.Value)
            {
                myWorkbook.Version = ExcelVersion.Excel2010;
                OutputFileName = "AdvancedCF.xlsx";
            }
            //Set the Default Version as Excel 2013
            else if (this.rdButtonexcel2013.IsChecked.Value)
            {
                myWorkbook.Version = ExcelVersion.Excel2013;
                OutputFileName = "AdvancedCF.xlsx";
            }

            #region Databar
            //Add condition for the range
            IConditionalFormats formats = sheet.Range["C7:C46"].ConditionalFormats;
            IConditionalFormat format = formats.AddCondition();

            //Set Data bar and icon set for the same cell
            //Set the format type
            format.FormatType = ExcelCFType.DataBar;
            IDataBar dataBar = format.DataBar;

            //Set the constraint
            dataBar.MinPoint.Type = ConditionValueType.LowestValue;
            dataBar.MinPoint.Value = "0";
            dataBar.MaxPoint.Type = ConditionValueType.HighestValue;
            dataBar.MaxPoint.Value = "0";

            //Set color for Bar
            dataBar.BarColor = System.Drawing.Color.FromArgb(156, 208, 243);

            //Hide the value in data bar
            dataBar.ShowValue = false;
            #endregion

            #region Iconset
            //Add another condition in the same range
            format = formats.AddCondition();

            //Set Icon format type
            format.FormatType = ExcelCFType.IconSet;
            IIconSet iconSet = format.IconSet;
            iconSet.IconSet = ExcelIconSetType.FourRating;
            iconSet.IconCriteria[0].Type = ConditionValueType.LowestValue;
            iconSet.IconCriteria[0].Value = "0";
            iconSet.IconCriteria[1].Type = ConditionValueType.HighestValue;
            iconSet.IconCriteria[1].Value = "0";
            iconSet.ShowIconOnly = true;

            //Sets Icon sets for another range
            formats = sheet.Range["E7:E46"].ConditionalFormats;
            format = formats.AddCondition();
            format.FormatType = ExcelCFType.IconSet;
            iconSet = format.IconSet;
            iconSet.IconSet = ExcelIconSetType.ThreeSymbols;
            iconSet.IconCriteria[0].Type = ConditionValueType.LowestValue;
            iconSet.IconCriteria[0].Value = "0";
            iconSet.IconCriteria[1].Type = ConditionValueType.HighestValue;
            iconSet.IconCriteria[1].Value = "0";
            iconSet.ShowIconOnly = true;
            #endregion

            #region Databar Negative value settings
            //Add condition for the range
            IConditionalFormats conditionalFormats1 = sheet.Range["E7:E46"].ConditionalFormats;
            IConditionalFormat conditionalFormat1 = conditionalFormats1.AddCondition();

            //Set Data bar and icon set for the same cell
            //Set the conditionalFormat type
            conditionalFormat1.FormatType = ExcelCFType.DataBar;
            IDataBar dataBar1 = conditionalFormat1.DataBar;

            //Set the constraint
            dataBar1.BarColor = System.Drawing.Color.YellowGreen;
            dataBar1.NegativeFillColor = System.Drawing.Color.Pink;
            dataBar1.NegativeBorderColor = System.Drawing.Color.WhiteSmoke;
            dataBar1.BarAxisColor = System.Drawing.Color.Yellow;
            dataBar1.BorderColor = System.Drawing.Color.WhiteSmoke;
            dataBar1.DataBarDirection = DataBarDirection.context;
            dataBar1.DataBarAxisPosition = DataBarAxisPosition.middle;
            dataBar1.HasGradientFill = true;

            //Hide the value in data bar
            dataBar1.ShowValue = false;

            #endregion

            #region Color Scale
            formats = sheet.Range["D7:D46"].ConditionalFormats;
            format = formats.AddCondition();
            format.FormatType = ExcelCFType.ColorScale;
            IColorScale colorScale = format.ColorScale;

            //Sets 3 - color scale.
            colorScale.SetConditionCount(3);

            colorScale.Criteria[0].FormatColorRGB = System.Drawing.Color.FromArgb(230, 197, 218);
            colorScale.Criteria[0].Type = ConditionValueType.LowestValue;
            colorScale.Criteria[0].Value = "0";

            colorScale.Criteria[1].FormatColorRGB = System.Drawing.Color.FromArgb(244, 210, 178);
            colorScale.Criteria[1].Type = ConditionValueType.Percentile;
            colorScale.Criteria[1].Value = "50";

            colorScale.Criteria[2].FormatColorRGB = System.Drawing.Color.FromArgb(245, 247, 171);
            colorScale.Criteria[2].Type = ConditionValueType.HighestValue;
            colorScale.Criteria[2].Value = "0";
            #endregion

            try
            {
                myWorkbook.SaveAs(OutputFileName);

                //Close the workbook.
                myWorkbook.Close();

                //No exception will be thrown if there are unsaved workbooks.
                excelEngine.ThrowNotSavedOnDestroy = false;
                excelEngine.Dispose();

                //Message box confirmation to view the created spreadsheet.
                if (MessageBox.Show("Do you want to view the workbook?", "Workbook has been created",
                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    try
                    {
                        //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                        System.Diagnostics.Process.Start(OutputFileName);
                        //Exit
                        this.Close();
                    }
                    catch (Win32Exception ex)
                    {
                        MessageBox.Show("Excel 2007 is not installed in this system");
                        Console.WriteLine(ex.ToString());
                    }
                }
                else
                    // Exit
                    this.Close();
            }
            catch
            {
                MessageBox.Show("Sorry, Excel can't open two workbooks with the same name at the same time.\nPlease close the workbook and try again.", "File is already open", MessageBoxButton.OK);
            }
        }
        # endregion
    }
}
