﻿using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
#if !Framework3_5
using System.Linq;
#endif

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Forum
{
    public sealed partial class ForumPostControl : UserControl
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostControl"/> class.
        /// </summary>
        public ForumPostControl()
        {
            this.InitializeComponent();
#if !Framework3_5
            //Enables touch manipulation.
            this.richTextBoxAdv.IsManipulationEnabled = true;
#endif
            this.richTextBoxAdv.RequestNavigate += richTextBoxAdv_RequestNavigate;
        }
        #endregion

        #region Hyperlink navigation
        /// <summary>
        /// Handles the request navigate event of the richTextBoxAdv control.
        /// </summary>
        /// <param name="obj">The source of the event.</param>
        /// <param name="args">The <see cref="RequestNavigateEventArgs"/> instance containing the event data.</param>
        void richTextBoxAdv_RequestNavigate(object obj, RequestNavigateEventArgs args)
        {
            if (args.Hyperlink.LinkType == HyperlinkType.Webpage || args.Hyperlink.LinkType == HyperlinkType.Email)
            {
                Uri uri = new Uri(args.Hyperlink.NavigationLink);
                LaunchUri(uri);
            }
        }
        /// <summary>
        /// Launches the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        void LaunchUri(Uri uri)
        {
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        }
        #endregion

        #region Helpher Methods
        /// <summary>
        /// Disposes the resources of ForumPostControl.
        /// </summary>
        public void Dispose()
        {
            if (this.Content is Panel)
                (this.Content as Panel).Children.Clear();
            this.richTextBoxAdv.RequestNavigate -= richTextBoxAdv_RequestNavigate;
            //Disposes the SfRichTextBoxAdv contents explicitly.
            this.richTextBoxAdv.Dispose();
            this.richTextBoxAdv = null;
        }
        #endregion
    }
}
