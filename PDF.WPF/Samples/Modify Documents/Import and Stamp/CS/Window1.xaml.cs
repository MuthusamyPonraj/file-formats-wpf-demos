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
using Microsoft.Win32;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Windows.Shared;


namespace ImportAndStamp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : ChromelessWindow
    {
        private string filepath;
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
            filepath =@"..\..\..\..\..\..\..\Common\Data\PDF\Essential_Studio.pdf";
			this.Icon = (ImageSource)img.ConvertFromString(@"..\..\..\..\..\..\..\Common\images\PDF\pdf_button.png");
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
            if (this.textBoxUrl.Text != String.Empty && filepath != string.Empty )
            {
                if (this.textBoxUrl.Text.Contains("\\"))
                    filepath = this.textBoxUrl.Text;
                
            }
            PdfLoadedDocument lDoc = new PdfLoadedDocument(filepath);
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 36f);

            foreach (PdfPageBase lPage in lDoc.Pages)
            {
                PdfGraphics g = lPage.Graphics;
                PdfGraphicsState state = g.Save();
                g.SetTransparency(0.25f);
                g.RotateTransform(-40);
                g.DrawString(textBoxStamp.Text, font, PdfPens.Red, PdfBrushes.Red, new PointF(-150, 450));
                g.Restore(state);
                if (checkBoxImage.IsChecked.Value)
                {
                    g.Save();
                    PdfImage image = new PdfBitmap(@"..\..\..\..\..\..\..\Common\Images\PDF\Ani.gif");
                    g.SetTransparency(0.25f);
                    g.DrawImage(image, 0, 0, lPage.Graphics.ClientSize.Width, lPage.Graphics.ClientSize.Height);
                    g.Restore();
                }
            }

            lDoc.Save("Sample.pdf");

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

        /// <summary>
        /// Gets the input file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "PDF Documents (*.pdf)|*.PDF";

            if (file.ShowDialog().Value)
                textBoxUrl.Text = file.FileName;
        }
        # endregion
    }
}