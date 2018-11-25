#region Copyright Syncfusion Inc. 2001 - 2015
// Copyright Syncfusion Inc. 2001 - 2015. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Syncfusion.XlsIO;
using Syncfusion.Windows.Shared;

namespace TemplateMarker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : ChromelessWindow
    {
        #region Constants
        private const string DEFAULTPATH = @"..\..\..\..\..\..\..\Common\Data\XlsIO\{0}";
        #endregion

        # region Private Members
        private DataTable northwindDt;
        private DataTable numbersDt;
        private string fileName;
        private string inputPath;
        IList<Customer> _customers = new List<Customer>();
        # endregion

        # region Constructor and Load
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

        /// <summary>
        /// Window Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Load Data
            InitDataSet();
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
            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.
            //Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();

            excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2007;
            //Open an existing spreadsheet which will be used as a template for generating the new spreadsheet.
            //After opening, the workbook object represents the complete in-memory object model of the template spreadsheet.
            IWorkbook workbook;
            if (rdImagewtSize.IsChecked.Value==true)
                fileName="TemplateMarkerImageWithSize.xlsx";
            else if (rdImageOnly.IsChecked.Value == true)
                fileName="TemplateMarkerImageOnly.xlsx";
            else if (rdImagewtPosition.IsChecked.Value == true)
                fileName="TemplateMarkerImageWithPosition.xlsx";
            else if (rdImagewtSizeAndPosition.IsChecked.Value == true)
                fileName="TemplateMarkerImageWithSize&Position.xlsx";
            else if (rdImageFitToCell.IsChecked.Value == true)
                 fileName="TemplateMarkerImageFitToCell.xlsx";
            inputPath = GetFullTemplatePath(fileName);
            workbook = excelEngine.Excel.Workbooks.Open(inputPath);
            //The first worksheet object in the worksheets collection is accessed.
            IWorksheet sheet1 = workbook.Worksheets[0];
            IWorksheet sheet2 = workbook.Worksheets[1];

            //Create Template Marker Processor
            ITemplateMarkersProcessor marker = workbook.CreateTemplateMarkersProcessor();

            IConditionalFormats conditions = marker.CreateConditionalFormats(sheet1["C5"]);
            #region Data Bar
            //Apply markers using Formula

            IConditionalFormat condition = conditions.AddCondition();

            //Set Data bar and icon set for the same cell
            //Set the format type
            condition.FormatType = ExcelCFType.DataBar;
            IDataBar dataBar = condition.DataBar;

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

            #region IconSet
            condition = conditions.AddCondition();
            condition.FormatType = ExcelCFType.IconSet;
            IIconSet iconSet = condition.IconSet;
            iconSet.IconSet = ExcelIconSetType.FourRating;
            iconSet.IconCriteria[0].Type = ConditionValueType.LowestValue;
            iconSet.IconCriteria[0].Value = "0";
            iconSet.IconCriteria[1].Type = ConditionValueType.HighestValue;
            iconSet.IconCriteria[1].Value = "0";
            iconSet.ShowIconOnly = true;
            #endregion

            conditions = marker.CreateConditionalFormats(sheet1["D5"]);
            #region Color Scale

            condition = conditions.AddCondition();

            condition.FormatType = ExcelCFType.ColorScale;
            IColorScale colorScale = condition.ColorScale;

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

            conditions = marker.CreateConditionalFormats(sheet1["E5"]);
            #region Iconset
            condition = conditions.AddCondition();
            condition.FormatType = ExcelCFType.IconSet;
            iconSet = condition.IconSet;
            iconSet.IconSet = ExcelIconSetType.ThreeSymbols;
            iconSet.IconCriteria[0].Type = ConditionValueType.LowestValue;
            iconSet.IconCriteria[0].Value = "0";
            iconSet.IconCriteria[1].Type = ConditionValueType.HighestValue;
            iconSet.IconCriteria[1].Value = "0";
            iconSet.ShowIconOnly = false;

            #endregion



           
            if (rdbDataTable.IsChecked.Value == true)
            {
                if (northwindDt == null)
                    InitDataSet();
                //Northwind customers table			
                marker.AddVariable("Customers", northwindDt);
            }
            else
            {
                //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
                //The instantiation process consists of two steps.
                if (this._customers.Count == 0)
                {
                    this._customers = GetCustomerAsObjects();
                }
                marker.AddVariable("Customers", _customers);
            }

            //Stretch Formula. This shows the data getting replaced in the marker specified in another worksheet.
            marker.AddVariable("NumbersTable", numbersDt);

            //Process the markers in the template.
            marker.ApplyMarkers();

            workbook.Version = ExcelVersion.Excel2007;

            try
            {
                //Saving the workbook to disk. This spreadsheet is the result of opening and modifying
                //an existing spreadsheet and then saving the result to a new workbook.
                workbook.SaveAs(fileName);

                //Close the workbook.
                workbook.Close();

                //Message box confirmation to view the created spreadsheet.
                if (MessageBox.Show("Do you want to view the workbook?", "Workbook has been created",
                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                    System.Diagnostics.Process.Start(fileName);
                }
            }
            catch
            {
                MessageBox.Show("Sorry, Excel can't open two workbooks with the same name at the same time.\nPlease close the workbook and try again.", "File is already open", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Opens input template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Input_Click(object sender, RoutedEventArgs e)
        {
            if (rdImagewtSize.IsChecked.Value==true)
                inputPath = GetFullTemplatePath("TemplateMarkerImageWithSize.xlsx");
            else if (rdImageOnly.IsChecked.Value==true)
                inputPath = GetFullTemplatePath("TemplateMarkerImageOnly.xlsx");
            else if (rdImagewtPosition.IsChecked.Value==true)
                inputPath = GetFullTemplatePath("TemplateMarkerImageWithPosition.xlsx");
            else if (rdImageFitToCell.IsChecked.Value==true)
                inputPath = GetFullTemplatePath("TemplateMarkerImageFitToCell.xlsx");
            else
                inputPath = GetFullTemplatePath("TemplateMarkerImageWithSize&Position.xlsx");
            //Launching the Input Template using the default Application.[MS Excel Or Free ExcelViewer]
            System.Diagnostics.Process.Start(inputPath);
        }
        # endregion

        # region Helpher Methods
        /// <summary>
        /// Get the file path of input file and return the same
        /// </summary>
        /// <param name="inputPath">Input file</param>
        /// <returns>path of the input file</returns>
        private string GetFullTemplatePath(string inputFile)
        {
            return string.Format(DEFAULTPATH, inputFile);
        }
        /// <summary>
        /// Creates DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable GetTable()
        {
            Random r = new Random();
            DataTable dt = new DataTable("NumbersTable");

            int nCols = 4;
            int nRows = 10;

            for (int i = 0; i < nCols; i++)
                dt.Columns.Add(new DataColumn("Column" + i.ToString()));

            for (int i = 0; i < nRows; ++i)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < nCols; j++)
                    dr[j] = r.Next(0, 10);
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// Gets the Collection of objects from the XML data.
        /// </summary>
        /// <returns>Collection of Customer Objects</returns>
        private IList<Customer> GetCustomerAsObjects()
        {
            DataSet customersDataSet = new DataSet();
            //Get the path of the input file
            string inputXmlPath = GetFullTemplatePath("Customers.xml");
            customersDataSet.ReadXml(inputXmlPath, XmlReadMode.ReadSchema);
            northwindDt = customersDataSet.Tables[0].Copy();
            //Changing the column data type from string to Byte Array
            northwindDt.Columns.Remove("Image");
            northwindDt.Columns.Add("Image", typeof(byte[]));
            //Get the path of Image File and convert it into bytes
            for (int j = 0; j < customersDataSet.Tables[0].Rows.Count; j++)
            {
                string imagePath = GetFullTemplatePath(@"Template_Marker_Images\" + customersDataSet.Tables[0].Rows[j]["Image"].ToString().Trim());
                northwindDt.Rows[j]["Image"] = File.ReadAllBytes(imagePath);
            }
            IList<Customer> tmpCustomers = new List<Customer>();
            Customer customer = new Customer();
            numbersDt = GetTable();
            DataRowCollection rows = northwindDt.Rows;
            foreach (DataRow row in rows)
            {
                customer = new Customer();
                customer.SalesPerson = row[0].ToString();
                customer.SalesJanJune = row[1].ToString();
                customer.SalesJulyDec = row[2].ToString();
                customer.Change = Convert.ToInt32(row[3]);
                customer.Image =(byte[])row[4];
                tmpCustomers.Add(customer);
            }
            customersDataSet.Dispose();
            return tmpCustomers;
        }
        /// <summary>
        /// Inits the data set.
        /// </summary>
        private void InitDataSet()
        {
            //Load Data
            DataSet customersDataSet = new DataSet();
            //Get the path of the input file
            string inputXmlPath = GetFullTemplatePath("Customers.xml");
            customersDataSet.ReadXml(inputXmlPath, XmlReadMode.ReadSchema);
            northwindDt = customersDataSet.Tables[0].Copy();
            //Changing the column data type from string to Byte Array
            northwindDt.Columns.Remove("Image");
            northwindDt.Columns.Add("Image", typeof(byte[]));
            //Get the path of Image File and convert it into bytes
            for (int j = 0; j < customersDataSet.Tables[0].Rows.Count; j++)
            {
                string imagePath = GetFullTemplatePath(@"Template_Marker_Images\" + customersDataSet.Tables[0].Rows[j]["Image"].ToString().Trim());
                northwindDt.Rows[j]["Image"] = File.ReadAllBytes(imagePath);
            }
            numbersDt = GetTable();
            customersDataSet.Dispose();
        }
        # endregion
    }
}