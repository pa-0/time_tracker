using System;
using System.Windows;
using System.Windows.Controls;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IProjectTimesData _dataSet;

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
 
