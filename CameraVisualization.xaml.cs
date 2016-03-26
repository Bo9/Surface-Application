using System;
using System.Diagnostics;
using System.Collections.Generic;
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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using Microsoft.Maps.MapControl.WPF;
using ESRI.ArcGIS.Client;

namespace CameraVisualizations
{
    /// <summary>
    /// Interaction logic for CameraVisualization.xaml
    /// </summary>
    public partial class CameraVisualization : TagVisualization
    {
        String iconPath = "";
        String info = "";
        public CameraVisualization()
        {
            InitializeComponent();
        }

        public void setIcon(String iconPath)
        {
            this.iconPath = iconPath;
        }

        public void setInfo(String newInfo)
        {
            this.info = newInfo;
        }

        private void CameraVisualization_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: customize CameraVisualization's UI based on this.VisualizedTag here
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start(@"c:\\test.wmv");
        }

        private void button1_Click(object sender, TouchEventArgs e)
        {
            //Process.Start(@"c:\\test.wmv");
        }

        private void menu_click(object sender, TouchEventArgs e)
        {
            Process.Start(@"video/test.wmv");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void placeSymbol(object sender, RoutedEventArgs e)
        {
            

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(SurfaceWindow1))
                {
                    SurfaceWindow1 surface = window as SurfaceWindow1;
                    ESRI.ArcGIS.Client.GraphicsLayer graphicLayer = null;
                    graphicLayer = (ESRI.ArcGIS.Client.GraphicsLayer)(surface.MyMap.Layers["myGraphicLayer"]);

                    Graphic graphic = new Graphic()
                    {
                        Geometry = surface.MyMap.ScreenToMap(new Point(MenuTag.Center.X, MenuTag.Center.Y)),
                        Symbol = new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol()
                        {
                            Source = (ImageSource)new ImageSourceConverter().ConvertFromString("C:/" + this.iconPath),
                            Width = 100,
                            Height = 100,
                            OffsetX = 0.5 * 100,
                            OffsetY = 0.5 * 100
                        }
                            
                    };

                    graphic.Attributes.Add("Name", "Thur");
                    graphic.Attributes.Add("Age", "1");
                    graphicLayer.Graphics.Add(graphic);
                    
                }
            }
        }
        
        public void showProfile(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(SurfaceWindow1))
                {
                    SurfaceWindow1 surface = window as SurfaceWindow1;
                    //Popup infobox = new Popup();
                    //infobox.Show();
                    surface.Infobox.Visibility = Visibility.Visible;
                    //surface.TxtBox.Content = this.info;
                    //TagElement tag = surface.giveTagElement((int)this.VisualizedTag.Value);

                    /*surface.Infobox.Visibility = Visibility.Visible;
                    surface.Profile.Text = tag.text();
                    Point location = new Point(this.Center.X + 100, this.Center.Y);
                    Location pinLocation = surface.myMap.ViewportPointToLocation(location);
                    MapLayer.SetPosition(surface.Infobox, pinLocation);*/
                }
            }
        }

        private void secretCode(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(SurfaceWindow1))
                {
                    SurfaceWindow1 surface = window as SurfaceWindow1;
                    surface.CreateDynamicWPFGrid();
                    //Popup infobox = new Popup();
                    //infobox.Show();
                    surface.landmarkInfobox.Visibility = Visibility.Visible;
                    
                }
            }
        }

        public void decryptCode(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(SurfaceWindow1))
                {
                    SurfaceWindow1 surface = window as SurfaceWindow1;
                    surface.CreateDynamicWPFGrid();
                    //Popup infobox = new Popup();
                    //infobox.Show();
                    surface.scoutInfobox.Visibility = Visibility.Visible;

                }
            }
        }

    }
}
