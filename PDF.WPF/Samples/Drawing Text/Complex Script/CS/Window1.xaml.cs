﻿using System;
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
using System.Drawing;
using Syncfusion.Pdf.Graphics;
using System.IO;
using Syncfusion.Windows.Shared;


namespace ComplexScript
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : ChromelessWindow
    {
        # region Constructor
        /// <summary>
        /// Window Constructor
        /// </summary>
        public Window1()	
        {
		   
            InitializeComponent();
            ImageSourceConverter img = new ImageSourceConverter();
            string file = @"..\..\..\..\..\..\..\Common\images\PDF\pdf_header.png";
            this.image1.Source = (ImageSource)img.ConvertFromString(file);
            this.Icon = (ImageSource)img.ConvertFromString(@"..\..\..\..\..\..\..\Common\Images\PDF\pdf_button.png");
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
            string inputText = @"เอกสาร PDF มีการใช้อย่างแพร่หลายเป็นเวลาหลายปีและนับสิบ ๆ แถมฟรี ผู้อ่าน 
PDF เชิงพาณิชย์บรรณาธิการและห้องสมุดพร้อมใช้งาน อย่างไรก็ตามแม้จะมี
ความนิยมนี้ก็ยังคงยากที่จะหาคำแนะนำกระชับรูปแบบ PDF พื้นเมือง
การทำความเข้าใจการทำงานภายในของ PDF ทำให้สามารถสร้างได้แบบไดนามิก
เอกสาร PDF ตัวอย่างเช่นเว็บเซิร์ฟเวอร์สามารถดึงข้อมูลจากฐานข้อมูล
ใช้เพื่อกำหนดใบกำกับสินค้าและให้บริการกับลูกค้าได้ทันที";

            //Create a new instance of PdfDocument class.
            PdfDocument document = new PdfDocument();

            //Add a new page to the document.
            PdfPage page = document.Pages.Add();

            //Create font.
            PdfFont font = new PdfTrueTypeFont(new Font("Tahoma", 14), true);

            //Create brush.
            PdfBrush brush = new PdfSolidBrush(System.Drawing.Color.Black);

            //Get page client size.          
            SizeF clipBounds = page.Graphics.ClientSize;

            RectangleF rect = new RectangleF(0, 0, clipBounds.Width, clipBounds.Height);

            //Create a new instance of the PdfStringFormat class.
            PdfStringFormat format = new PdfStringFormat();

            //Set the property for complex text layout.
            format.ComplexScript = true;

            //Draw the text.
            page.Graphics.DrawString(inputText, font, brush, rect, format);

            //Save the document.
            document.Save("sample.pdf");

            //Close the document.
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
    }
}