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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class ScenarioWindow : SurfaceWindow
    {
        ScenarioVisualization camera;
        public List<TagElement> Symbols;

        public ScenarioWindow()
        {
            InitializeComponent();
            Symbols = new List<TagElement>();
            Symbols.Add(new TagElement(1, "Tank", "100", "2", "100", "tank.png"));
            Symbols.Add(new TagElement(2, "IFV", "100", "2", "100", "afv.png"));
            Symbols.Add(new TagElement(3, "Ally", "100", "2", "100", "ally.png"));
            Symbols.Add(new TagElement(4, "Enemy", "100", "2", "100", "enemy.png"));
            Symbols.Add(new TagElement(32, "Enemy", "100", "2", "100", "enemy.png"));
            Symbols.Add(new TagElement(33, "Enemy", "100", "2", "100", "enemy.png"));
            Symbols.Add(new TagElement(34, "Enemy", "100", "2", "100", "enemy.png"));
            Symbols.Add(new TagElement(35, "Enemy", "100", "2", "100", "enemy.png"));
            Symbols.Add(new TagElement(36, "Enemy", "100", "2", "100", "enemy.png"));
        }

        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            camera = (ScenarioVisualization)e.TagVisualization;
            string[] tagValue = { "Enemy1", "Enemy2", "Enemy3", "Ally" };

            for (int i = 1; i < Symbols.Count() + 1; i++)
            {
                TagElement objectOnSurface = Symbols.ElementAt(i - 1);
                if (camera.VisualizedTag.Value == objectOnSurface.tagValue)
                {
                    //camera.setIcon(objectOnSurface.iconPath);
                    //camera.setInfo("This is a car");
                    //camera.CameraModel.Content = objectOnSurface.stringName + " is at " + camera.Center.X.ToString() + ","+ camera.Center.Y.ToString();
                    //camera.myEllipse.Fill = new SolidColorBrush(System.Windows.Media.Colors.Blue); 
                    if (objectOnSurface.tagValue == 1)
                    {
                        camera.TTcontact.Text = "Contact Infantry";
                        //SurfaceButton testControl4 = new SurfaceButton();
                        //testControl4.Content = "Ask Question";
                        //testControl4.Click += new RoutedEventHandler(secretCode);
                        //camera.testControl.Content = testControl4;
                    }
                    if (objectOnSurface.tagValue == 2)
                    {
                        camera.TTcontact.Text = "Forward Observer";
                        //SurfaceButton testControl4 = new SurfaceButton();
                        //testControl4.Content = "Ask Question";
                        //testControl4.Click += new RoutedEventHandler(secretCode);
                        //camera.testControl.Content = testControl4;
                    }
                }
            }
            e.Handled = true;

        }

        private void startScenario(object sender, RoutedEventArgs e)
        {
            this.yellow25.Visibility = Visibility.Visible;
            this.contactCB.Visibility = Visibility.Visible;
            this.contactCB.IsEnabled = true;
            this.contactCB.IsChecked = true;
        }

        private void warningPlaced(object sender, TouchEventArgs e)
        {
            if (this.contactCB.IsChecked == true)
            {
                this.yellow25.Visibility = Visibility.Collapsed;
                this.green25.Visibility = Visibility.Visible;
                this.red35.Visibility = Visibility.Visible;
                this.red15.Visibility = Visibility.Visible;
                this.obCB.Visibility = Visibility.Visible;
                this.obCB.IsEnabled = true;
                this.obCB.IsChecked = true;
            }
        }

        private void warningRemoved(object sender, TouchEventArgs e)
        {
            this.green25.Visibility = Visibility.Collapsed;
            this.red35.Visibility = Visibility.Collapsed;
            this.red15.Visibility = Visibility.Collapsed;
        }

        private void observationPlaced(object sender, TouchEventArgs e)
        {
            if (this.obCB.IsChecked == true)
            {
                this.green23.Visibility = Visibility.Visible;
                this.green24.Visibility = Visibility.Visible;
                this.green25.Visibility = Visibility.Visible;
                this.green33.Visibility = Visibility.Visible;
                this.green34.Visibility = Visibility.Visible;
                this.green35.Visibility = Visibility.Visible;
                this.green43.Visibility = Visibility.Visible;
                this.green44.Visibility = Visibility.Visible;
                this.green45.Visibility = Visibility.Visible;
                this.armourCB.Visibility = Visibility.Visible;
                this.armourCB.IsEnabled = true;
                this.armourCB.IsChecked = true;
            }
        }

        private void observationRemoved(object sender, TouchEventArgs e)
        {
            this.green23.Visibility = Visibility.Collapsed;
            this.green24.Visibility = Visibility.Collapsed;
            this.green25.Visibility = Visibility.Collapsed;
            this.green33.Visibility = Visibility.Collapsed;
            this.green34.Visibility = Visibility.Collapsed;
            this.green35.Visibility = Visibility.Collapsed;
            this.green43.Visibility = Visibility.Collapsed;
            this.green44.Visibility = Visibility.Collapsed;
            this.green45.Visibility = Visibility.Collapsed;
        }

        private void armourPlaced(object sender, TouchEventArgs e)
        {
            if (this.armourCB.IsChecked == true)
            {
                this.green25.Visibility = Visibility.Visible;
                this.green15.Visibility = Visibility.Visible;
                this.red14.Visibility = Visibility.Visible;
                this.reinforceCB.Visibility = Visibility.Visible;
                this.reinforceCB.IsEnabled = true;
                this.reinforceCB.IsChecked = true;
            }
        }

        private void armourRemoved(object sender, TouchEventArgs e)
        {
            this.green25.Visibility = Visibility.Collapsed;
            this.green15.Visibility = Visibility.Collapsed;
            this.red14.Visibility = Visibility.Collapsed;
        }

        private void reinforcePlaced(object sender, TouchEventArgs e)
        {
            if (this.reinforceCB.IsChecked == true)
            {
                this.green32.Visibility = Visibility.Visible;
                this.green33.Visibility = Visibility.Visible;
                this.green34.Visibility = Visibility.Visible;
                this.green42.Visibility = Visibility.Visible; 
                this.green43.Visibility = Visibility.Visible;
                this.green44.Visibility = Visibility.Visible;
                this.assaultCB.Visibility = Visibility.Visible;
                this.assaultCB.IsEnabled = true;
                this.assaultCB.IsChecked = true;
            }
        }

        private void reinforceRemoved(object sender, TouchEventArgs e)
        {
            this.green32.Visibility = Visibility.Collapsed;
            this.green33.Visibility = Visibility.Collapsed;
            this.green34.Visibility = Visibility.Collapsed;
            this.green42.Visibility = Visibility.Collapsed;
            this.green43.Visibility = Visibility.Collapsed;
            this.green44.Visibility = Visibility.Collapsed;
        }

        private void assaultPlaced(object sender, TouchEventArgs e)
        {
            if (this.assaultCB.IsChecked == true)
            {
                this.correctSound.Stop();
                this.correctSound.Play();
                this.findButton.Visibility = Visibility.Visible;
            }
        }

        private void findPressed(object sender, TouchEventArgs e)
        {
            if (this.obCB.IsChecked == true || this.reinforceCB.IsChecked == true || this.enemyCB.IsChecked == true)
            {
                this.yellow33.Visibility = Visibility.Visible;
                this.yellow34.Visibility = Visibility.Visible;
                this.yellow23.Visibility = Visibility.Visible;
                this.yellow24.Visibility = Visibility.Visible;
                this.enemyCB.Visibility = Visibility.Visible;
                this.enemyCB.IsEnabled = true;
                this.enemyCB.IsChecked = true;
                this.flankCB.Visibility = Visibility.Visible;
                this.flankCB.IsEnabled = true;
                this.flankCB.IsChecked = true;
            }
        }

        private void findReleased(object sender, TouchEventArgs e)
        {
            this.yellow33.Visibility = Visibility.Collapsed;
            this.yellow34.Visibility = Visibility.Collapsed;
            this.yellow23.Visibility = Visibility.Collapsed;
            this.yellow24.Visibility = Visibility.Collapsed;
        }

        private void flankPlaced(object sender, TouchEventArgs e)
        {
            if (this.flankCB.IsChecked == true)
            {
                this.green22.Visibility = Visibility.Visible;
                this.green12.Visibility = Visibility.Visible;
                this.green23.Visibility = Visibility.Visible;
                this.red32.Visibility = Visibility.Visible;
            }
        }

        private void flankRemoved(object sender, TouchEventArgs e)
        {
            this.green22.Visibility = Visibility.Collapsed;
            this.green23.Visibility = Visibility.Collapsed;
            this.green12.Visibility = Visibility.Collapsed;
            this.red32.Visibility = Visibility.Collapsed;
        }

        public class TagElement : Object
        {
            public int tagValue;
            public String stringName;
            public String power;
            public String rank;
            public String availableUnit;
            public String iconPath;

            public TagElement(int index, String name, String power, String rank, String availableUnit, String iconPath)
            {
                this.tagValue = index;
                this.stringName = name;
                this.power = power;
                this.rank = rank;
                this.availableUnit = availableUnit;
                this.iconPath = iconPath;
            }

            public String text()
            {
                String returnString = "Profile\n";
                returnString += stringName + "\nRank : " + rank + "\nThere are " + availableUnit + " units available.";

                return returnString;
            }

        }

        public TagElement giveTagElement(int index)
        {
            for (int i = 1; i < Symbols.Count() + 1; i++)
            {
                TagElement objectOnSurface = Symbols.ElementAt(i - 1);
                if (index == objectOnSurface.tagValue)
                {
                    return objectOnSurface;
                }
            }

            return null;
        }

        private void mapClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mainMenu(object sender, RoutedEventArgs e)
        {
            Window1 mainLaunch = new Window1();
            this.Close();
            mainLaunch.Show();
        }

    }
}
