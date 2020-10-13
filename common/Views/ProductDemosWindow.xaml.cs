#region Copyright Syncfusion Inc. 2001-2020.
// Copyright Syncfusion Inc. 2001-2020. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace syncfusion.demoscommon.wpf
{
    /// <summary>
    /// Interaction logic for ProductDemosWindow.xaml
    /// </summary>
    public partial class ProductDemosWindow : ChromelessWindow
    {
        public ProductDemosWindow(DemoBrowserViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.Title = viewModel.SelectedProduct.Product + " Demos";
            if (DemosNavigationService.MainWindow.Width < this.Width ||
                 DemosNavigationService.MainWindow.Height < this.Height)
            {
                this.Width = DemosNavigationService.MainWindow.Width * 0.95;
                this.Height = DemosNavigationService.MainWindow.Height * 0.95;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var viewmodel = this.DataContext as DemoBrowserViewModel;
            if (viewmodel != null)
            {
                viewmodel.SelectedProduct = null;
                viewmodel.SelectedSample = null;
            }
            this.DataContext = null;
            DemosNavigationService.MainWindow.Activate();
            base.OnClosing(e);
        }

    }
}
