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
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace CameraVisualizations
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : SurfaceWindow
    {
        SurfaceWindow1 SW = new SurfaceWindow1();
        ScenarioWindow scenario = new ScenarioWindow();
        Config config = new Config();

        public Window1()
        {
            InitializeComponent();
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void WorldMap_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            SW.Show();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void configButton_Click(object sender, RoutedEventArgs e)
        {
            config.Show();
        }

        private void battlefield_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            scenario.Show();
        }

    }
}
