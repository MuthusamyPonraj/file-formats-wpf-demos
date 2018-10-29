using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Data.SqlServerCe;
using Syncfusion.Windows.Shared;

namespace NorthwindReport
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : ChromelessWindow
    {
        # region Private Members    

        string styleName;
        # endregion

        # region Constructor
        /// <summary>
        /// Window constructor
        /// </summary>
        public Window1()
        {
            InitializeComponent();
            ImageSourceConverter img = new ImageSourceConverter();
            string file = @"..\..\..\..\..\..\..\Common\images\PDF\pdf_header.png";          
            this.image1.Source = (ImageSource)img.ConvertFromString(file);
            this.Icon = (ImageSource)img.ConvertFromString(@"..\..\..\..\..\..\..\Common\images\PDF\pdf_button.png");           
            this.comboBox1.SelectedIndex = 26;
            this.checkBox1.IsChecked = true;
            this.checkBox2.IsChecked = true;
            this.Height = 345;
        }
        # endregion

        # region Events
        /// <summary>
        /// Creates PDF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            styleName = this.comboBox1.SelectedItem.ToString();
            //Create a new instance of PdfDocument class.
            PdfDocument document = new PdfDocument();

            //Set font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 7);

            // Add a new page to the document.
            PdfPage page = document.Pages.Add();

            //Creating PdfGrid
            PdfGrid grid = new PdfGrid();

            //Create dataset with the "Customers" table from Northwind database.
            DataSet dataSet = GetAdventureWorkCycleDataSet();

            DataTable dt = dataSet.Tables[0];
            string[] unwantedColumns = { "Freight", "EmployeeID", "OrderDate", "RequiredDate", "ShippedDate", "ShipVia", "ShipRegion" };
            foreach (string columnIndex in unwantedColumns)
            {
                dt.Columns.Remove(columnIndex);
            }

            grid.DataSource = dt;
            grid.Style.Font = font;
           
            grid.Style.AllowHorizontalOverflow = true;

            #region PdfGridBuiltinStyleSettings
            PdfGridBuiltinStyleSettings setting = new PdfGridBuiltinStyleSettings();
            setting.ApplyStyleForHeaderRow = this.checkBox1.IsChecked.Value;
            setting.ApplyStyleForBandedRows = this.checkBox2.IsChecked.Value;
            setting.ApplyStyleForFirstColumn = this.checkBox3.IsChecked.Value;
            setting.ApplyStyleForLastRow = this.checkBox4.IsChecked.Value;
            setting.ApplyStyleForBandedColumns = this.checkBox5.IsChecked.Value;       
            setting.ApplyStyleForLastColumn = this.checkBox6.IsChecked.Value;         
            #endregion

            PdfGridLayoutFormat format = new PdfGridLayoutFormat();
            format.Break = PdfLayoutBreakType.FitPage;
            format.Layout = PdfLayoutType.Paginate;          

            PdfGridBuiltinStyle style = ConvertToPdfGridBuiltinStyle(styleName);

            //Apply Style to PdfGrid
            grid.ApplyBuiltinStyle(style, setting);

            grid.Draw(page, PointF.Empty, format);

            //Save to disk
            document.Save("Sample.pdf");
            document.Close(true);

            //Message box confirmation to view the created PDF document.
            if (MessageBox.Show("Do you want to view the PDF file?", "PDF File Created",
                MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                //Launching the PDF file using the default Application.[Acrobat Reader]
                System.Diagnostics.Process.Start("Sample.pdf");
                this.Close();
            }
            else
                // Exit
                this.Close();
        }

        # endregion

        # region Helpher Methods
        /// <summary>
        /// Convert string to PdfGridBuiltinStyle
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private PdfGridBuiltinStyle ConvertToPdfGridBuiltinStyle(string styleName)
        {
            PdfGridBuiltinStyle value = (PdfGridBuiltinStyle)Enum.Parse(typeof(PdfGridBuiltinStyle), styleName);
            return value;
        }
        # region Dataset

        /// <summary>
        /// Returns dataset.
        /// </summary>
        private DataSet GetAdventureWorkCycleDataSet()
        {
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(GetFullTemplatePath("Orders.xml", false));           
            return dataSet;
        }

        /// <summary>
        /// Gets the full path of the PDF template or image.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="image">True if image</param>
        /// <returns>Path of the file</returns>
        private string GetFullTemplatePath(string fileName, bool image)
        {
            string fullPath = @"..\..\..\..\..\..\..\Common\";

            string folder = image ? "Images\\PDF" : "Data";

            return string.Format(@"{0}{1}\{2}", fullPath, folder, fileName);
        }
      
        # endregion

        # region Header
        /// <summary>
        /// Adds header to the PDF document
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        private void AddHeader(PdfDocument doc, string title, string description)
        {
            RectangleF rect = new RectangleF(0, 0, doc.Pages[0].GetClientSize().Width, 54);
            PdfPageTemplateElement header = new PdfPageTemplateElement(rect);
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 24);
            float doubleHeight = font.Height * 2;
            System.Drawing.Color activeColor = System.Drawing.Color.FromArgb(44, 71, 120);
            SizeF imageSize = new SizeF(110f, 35f);
            //Locating the logo on the right corner of the Drawing Surface
            PointF imageLocation = new PointF(doc.Pages[0].GetClientSize().Width - imageSize.Width - 20, 8);

            PdfImage img = new PdfBitmap(@"..\..\..\..\..\..\..\Common\Images\PDF\logo.jpg");           

            //Draw the image in the Header.
            header.Graphics.DrawImage(img, imageLocation, imageSize);

            PdfSolidBrush brush = new PdfSolidBrush(activeColor);

            PdfPen pen = new PdfPen(System.Drawing.Color.DarkBlue, 3f);
            font = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
            PdfStringFormat format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Center;
            format.LineAlignment = PdfVerticalAlignment.Middle;
            header.Graphics.DrawString(title, font, brush, new RectangleF(0, 0, header.Width, header.Height), format);
            brush = new PdfSolidBrush(System.Drawing.Color.Gray);
            font = new PdfStandardFont(PdfFontFamily.Helvetica, 6, PdfFontStyle.Bold);

            format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Left;
            format.LineAlignment = PdfVerticalAlignment.Bottom;

            header.Graphics.DrawString(description, font, brush, new RectangleF(0, 0, header.Width, header.Height - 8), format);

            header.Graphics.DrawLine(pen, 0, 0, header.Width, 0);
            pen = new PdfPen(System.Drawing.Color.DarkBlue, 2f);
            header.Graphics.DrawLine(pen, 0, header.Height - 4, header.Width, header.Height - 4);

            doc.Template.Top = header;
        }
        # endregion      

        private void ChromelessWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Array styleArray=Enum.GetValues(typeof(PdfGridBuiltinStyle));
            foreach (PdfGridBuiltinStyle style in styleArray)
            {
                this.comboBox1.Items.Add(style);
            }
        }
        # endregion
    }
}