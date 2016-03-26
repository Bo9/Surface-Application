using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Local;

using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps;

namespace CameraVisualizations
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    /// 

    public partial class SurfaceWindow1 : SurfaceWindow
    {
        

        public List<TagElement> Symbols;
        public Draw MyDrawObject;
        CameraVisualization camera;

        public bool mapLock;
        public TimeSpan panValue;
        public bool measure;

        public Draw MeasureObject;
        public List<ESRI.ArcGIS.Client.Geometry.MapPoint> measureList;

        public void CreateDynamicWPFGrid()
        {
            // Create the Grid
            Grid DynamicGrid = new Grid();
            DynamicGrid.Width = 400;
            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Left;
            DynamicGrid.VerticalAlignment = VerticalAlignment.Top;
            DynamicGrid.ShowGridLines = true;
            DynamicGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);

            // Create Columns
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            ColumnDefinition gridCol3 = new ColumnDefinition();
            DynamicGrid.ColumnDefinitions.Add(gridCol1);
            DynamicGrid.ColumnDefinitions.Add(gridCol2);
            DynamicGrid.ColumnDefinitions.Add(gridCol3);

            // Create Rows
            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.Height = new GridLength(45);
            RowDefinition gridRow2 = new RowDefinition();
            gridRow2.Height = new GridLength(45);
            RowDefinition gridRow3 = new RowDefinition();
            gridRow3.Height = new GridLength(45);
            DynamicGrid.RowDefinitions.Add(gridRow1);
            DynamicGrid.RowDefinitions.Add(gridRow2);
            DynamicGrid.RowDefinitions.Add(gridRow3);

            // Add first column header
            TextBlock txtBlock1 = new TextBlock();
            txtBlock1.Text = "Author Name";
            txtBlock1.FontSize = 14;
            txtBlock1.FontWeight = FontWeights.Bold;
            txtBlock1.Foreground = new SolidColorBrush(Colors.Green);
            txtBlock1.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(txtBlock1, 0);
            Grid.SetColumn(txtBlock1, 0);

            // Add second column header
            TextBlock txtBlock2 = new TextBlock();
            txtBlock2.Text = "Age";
            txtBlock2.FontSize = 14;
            txtBlock2.FontWeight = FontWeights.Bold;
            txtBlock2.Foreground = new SolidColorBrush(Colors.Green);
            txtBlock2.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(txtBlock2, 0);
            Grid.SetColumn(txtBlock2, 1);

            // Add third column header
            TextBlock txtBlock3 = new TextBlock();
            txtBlock3.Text = "Book";
            txtBlock3.FontSize = 14;
            txtBlock3.FontWeight = FontWeights.Bold;
            txtBlock3.Foreground = new SolidColorBrush(Colors.Green);
            txtBlock3.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(txtBlock3, 0);
            Grid.SetColumn(txtBlock3, 2);

            //// Add column headers to the Grid
            DynamicGrid.Children.Add(txtBlock1);
            DynamicGrid.Children.Add(txtBlock2);
            DynamicGrid.Children.Add(txtBlock3);

            // Create first Row
            TextBlock authorText = new TextBlock();
            authorText.Text = "Mahesh Chand";
            authorText.FontSize = 12;
            authorText.FontWeight = FontWeights.Bold;
            Grid.SetRow(authorText, 1);
            Grid.SetColumn(authorText, 0);

            TextBlock ageText = new TextBlock();
            ageText.Text = "33";
            ageText.FontSize = 12;
            ageText.FontWeight = FontWeights.Bold;
            Grid.SetRow(ageText, 1);
            Grid.SetColumn(ageText, 1);

            TextBlock bookText = new TextBlock();
            bookText.Text = "GDI+ Programming";
            bookText.FontSize = 12;
            bookText.FontWeight = FontWeights.Bold;
            Grid.SetRow(bookText, 1);
            Grid.SetColumn(bookText, 2);
            // Add first row to Grid
            DynamicGrid.Children.Add(authorText);
            DynamicGrid.Children.Add(ageText);
            DynamicGrid.Children.Add(bookText);

            // Create second row
            authorText = new TextBlock();
            authorText.Text = "Mike Gold";
            authorText.FontSize = 12;
            authorText.FontWeight = FontWeights.Bold;
            Grid.SetRow(authorText, 2);
            Grid.SetColumn(authorText, 0);

            ageText = new TextBlock();
            ageText.Text = "35";
            ageText.FontSize = 12;
            ageText.FontWeight = FontWeights.Bold;
            Grid.SetRow(ageText, 2);
            Grid.SetColumn(ageText, 1);

            bookText = new TextBlock();
            bookText.Text = "Programming C#";
            bookText.FontSize = 12;
            bookText.FontWeight = FontWeights.Bold;
            Grid.SetRow(bookText, 2);
            Grid.SetColumn(bookText, 2);

            // Add second row to Grid
            DynamicGrid.Children.Add(authorText);
            DynamicGrid.Children.Add(ageText);
            DynamicGrid.Children.Add(bookText);

            // Display grid into a Window
            
            this.testControl.Content = DynamicGrid;
            //this.TestBox.Visibility = Visibility.Visible;

        }

        Point m_start;
        Point temp;
        Vector m_startOffset;
        TouchDevice infoboxControl;

        private void Infobox_TouchDown(object sender, TouchEventArgs e)
        {
            m_start = e.GetTouchPoint(this).Position;
            m_startOffset = new Vector(this.test.X, this.test.Y);
            e.TouchDevice.Capture(this.Infobox);
            if (infoboxControl == null)
            {
                infoboxControl = e.TouchDevice;
            }

            e.Handled = true;
        }

        private void Infobox_TouchMove(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl)
            {
                temp =  e.GetTouchPoint(this).Position;
                Vector offset = Point.Subtract(temp, m_start);

                this.test.X = m_startOffset.X + offset.X;
                this.test.Y = m_startOffset.Y + offset.Y;
            }
            e.Handled = true;
        }

        private void Infobox_TouchLeave(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl)
            {
                infoboxControl = null;
            }
            e.Handled = true;
        }

        Point m_start_landmark;
        Point temp_landmark;
        Vector m_startOffset_landmark;
        TouchDevice infoboxControl_landmark;

        private void Landmark_TouchDown(object sender, TouchEventArgs e)
        {
            m_start_landmark = e.GetTouchPoint(this).Position;
            m_startOffset_landmark = new Vector(this.landmarkTransform.X, this.landmarkTransform.Y);
            e.TouchDevice.Capture(this.landmarkInfobox);
            if (infoboxControl_landmark == null)
            {
                infoboxControl_landmark = e.TouchDevice;
            }

            e.Handled = true;
        }

        private void Landmark_TouchMove(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_landmark)
            {
                temp_landmark = e.GetTouchPoint(this).Position;
                Vector offset = Point.Subtract(temp_landmark, m_start_landmark);

                this.landmarkTransform.X = m_startOffset_landmark.X + offset.X;
                this.landmarkTransform.Y = m_startOffset_landmark.Y + offset.Y;
            }
            e.Handled = true;
        }

        private void Landmark_TouchLeave(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_landmark)
            {
                infoboxControl_landmark = null;
            }
            e.Handled = true;
        }


        private void landmarkVideoClick(object sender, RoutedEventArgs e)
        {
            this.landmarkMedia.Visibility = Visibility.Visible;
            this.landmarkMedia.Play();

        }

        private void landmarkCloseClick(object sender, RoutedEventArgs e)
        {
            landmarkInfobox.Visibility = Visibility.Collapsed;
            landmarkMedia.Stop();
            this.landmarkMedia.Visibility = Visibility.Collapsed;
        }

        Point m_start_landmark2;
        Point temp_landmark2;
        Vector m_startOffset_landmark2;
        TouchDevice infoboxControl_landmark2;

        private void Landmark_TouchDown2(object sender, TouchEventArgs e)
        {
            m_start_landmark2 = e.GetTouchPoint(this).Position;
            m_startOffset_landmark2 = new Vector(this.landmarkTransform2.X, this.landmarkTransform2.Y);
            e.TouchDevice.Capture(this.landmarkInfobox2);
            if (infoboxControl_landmark2 == null)
            {
                infoboxControl_landmark2 = e.TouchDevice;
            }

            e.Handled = true;
        }

        private void Landmark_TouchMove2(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_landmark2)
            {
                temp_landmark2 = e.GetTouchPoint(this).Position;
                Vector offset = Point.Subtract(temp_landmark2, m_start_landmark2);

                this.landmarkTransform2.X = m_startOffset_landmark2.X + offset.X;
                this.landmarkTransform2.Y = m_startOffset_landmark2.Y + offset.Y;
            }
            e.Handled = true;
        }

        private void Landmark_TouchLeave2(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_landmark2)
            {
                infoboxControl_landmark2 = null;
            }
            e.Handled = true;
        }


        private void landmarkVideoClick2(object sender, RoutedEventArgs e)
        {
            this.landmarkMedia2.Visibility = Visibility.Visible;
            this.landmarkMedia2.Play();

        }

        private void landmarkCloseClick2(object sender, RoutedEventArgs e)
        {
            landmarkInfobox2.Visibility = Visibility.Collapsed;
            landmarkMedia2.Stop();
            this.landmarkMedia2.Visibility = Visibility.Collapsed;
        }

        Point m_start_landmark3;
        Point temp_landmark3;
        Vector m_startOffset_landmark3;
        TouchDevice infoboxControl_landmark3;

        private void Landmark_TouchDown3(object sender, TouchEventArgs e)
        {
            m_start_landmark3 = e.GetTouchPoint(this).Position;
            m_startOffset_landmark3 = new Vector(this.landmarkTransform3.X, this.landmarkTransform3.Y);
            e.TouchDevice.Capture(this.landmarkInfobox3);
            if (infoboxControl_landmark3 == null)
            {
                infoboxControl_landmark3 = e.TouchDevice;
            }

            e.Handled = true;
        }

        private void Landmark_TouchMove3(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_landmark3)
            {
                temp_landmark3 = e.GetTouchPoint(this).Position;
                Vector offset = Point.Subtract(temp_landmark3, m_start_landmark3);

                this.landmarkTransform3.X = m_startOffset_landmark3.X + offset.X;
                this.landmarkTransform3.Y = m_startOffset_landmark3.Y + offset.Y;
            }
            e.Handled = true;
        }

        private void Landmark_TouchLeave3(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_landmark3)
            {
                infoboxControl_landmark3 = null;
            }
            e.Handled = true;
        }


        private void landmarkVideoClick3(object sender, RoutedEventArgs e)
        {
            this.landmarkMedia3.Visibility = Visibility.Visible;
            this.landmarkMedia3.Play();

        }

        private void landmarkCloseClick3(object sender, RoutedEventArgs e)
        {
            landmarkInfobox3.Visibility = Visibility.Collapsed;
            landmarkMedia3.Stop();
            this.landmarkMedia3.Visibility = Visibility.Collapsed;
        }

        Point m_start_landmark4;
        Point temp_landmark4;
        Vector m_startOffset_landmark4;
        TouchDevice infoboxControl_landmark4;

        private void Landmark_TouchDown4(object sender, TouchEventArgs e)
        {
            m_start_landmark4 = e.GetTouchPoint(this).Position;
            m_startOffset_landmark4 = new Vector(this.landmarkTransform4.X, this.landmarkTransform4.Y);
            e.TouchDevice.Capture(this.landmarkInfobox4);
            if (infoboxControl_landmark4 == null)
            {
                infoboxControl_landmark4 = e.TouchDevice;
            }

            e.Handled = true;
        }

        private void Landmark_TouchMove4(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_landmark4)
            {
                temp_landmark4 = e.GetTouchPoint(this).Position;
                Vector offset = Point.Subtract(temp_landmark4, m_start_landmark4);

                this.landmarkTransform4.X = m_startOffset_landmark4.X + offset.X;
                this.landmarkTransform4.Y = m_startOffset_landmark4.Y + offset.Y;
            }
            e.Handled = true;
        }

        private void Landmark_TouchLeave4(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_landmark4)
            {
                infoboxControl_landmark4 = null;
            }
            e.Handled = true;
        }


        private void landmarkVideoClick4(object sender, RoutedEventArgs e)
        {
            this.landmarkMedia4.Visibility = Visibility.Visible;
            this.landmarkMedia4.Play();

        }

        private void landmarkCloseClick4(object sender, RoutedEventArgs e)
        {
            landmarkInfobox4.Visibility = Visibility.Collapsed;
            landmarkMedia4.Stop();
            this.landmarkMedia4.Visibility = Visibility.Collapsed;
        }

        Point m_start_FinalBoss;
        Point temp_FinalBoss;
        Vector m_startOffset_FinalBoss;
        TouchDevice infoboxControl_FinalBoss;

        private void FinalBoss_TouchDown(object sender, TouchEventArgs e)
        {
            m_start_FinalBoss = e.GetTouchPoint(this).Position;
            m_startOffset_FinalBoss = new Vector(this.FinalBossTransform.X, this.FinalBossTransform.Y);
            e.TouchDevice.Capture(this.FinalBoss);
            if (infoboxControl_FinalBoss == null)
            {
                infoboxControl_FinalBoss = e.TouchDevice;
            }

            e.Handled = true;
        }

        private void FinalBoss_TouchMove(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_FinalBoss)
            {
                temp_FinalBoss = e.GetTouchPoint(this).Position;
                Vector offset = Point.Subtract(temp_FinalBoss, m_start_FinalBoss);

                this.FinalBossTransform.X = m_startOffset_FinalBoss.X + offset.X;
                this.FinalBossTransform.Y = m_startOffset_FinalBoss.Y + offset.Y;
            }
            e.Handled = true;
        }

        private void FinalBoss_TouchLeave(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_FinalBoss)
            {
                infoboxControl_FinalBoss = null;
            }
            e.Handled = true;
        }


        private void FinalBossVideoClick(object sender, RoutedEventArgs e)
        {
            this.FinalBossMedia.Stop();
            this.FinalBossMedia.Visibility = Visibility.Visible;
            this.FinalBossMedia.Play();

        }

        private void FinalBossCloseClick(object sender, RoutedEventArgs e)
        {
            FinalBoss.Visibility = Visibility.Collapsed;
            this.FinalBossMedia.Stop();
            this.FinalBossMedia.Visibility = Visibility.Collapsed;
        }
        
        Point m_start_scout;
        Point temp_scout;
        Vector m_startOffset_scout;
        TouchDevice infoboxControl_scout;

        private void Scout_TouchDown(object sender, TouchEventArgs e)
        {
            m_start_scout = e.GetTouchPoint(this).Position;
            m_startOffset_scout = new Vector(this.scoutTransform.X, this.scoutTransform.Y);
            e.TouchDevice.Capture(this.scoutInfobox);
            if (infoboxControl_scout == null)
            {
                infoboxControl_scout = e.TouchDevice;
            }

            e.Handled = true;
        }

        private void Scout_TouchMove(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_scout)
            {
                temp_scout = e.GetTouchPoint(this).Position;
                Vector offset = Point.Subtract(temp_scout, m_start_scout);

                this.scoutTransform.X = m_startOffset_scout.X + offset.X;
                this.scoutTransform.Y = m_startOffset_scout.Y + offset.Y;
            }
            e.Handled = true;
        }

        private void Scout_TouchLeave(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_scout)
            {
                infoboxControl_scout = null;
            }
            e.Handled = true;
        }

        //private void scoutVideoClick(object sender, RoutedEventArgs e)
        //{
            //this.scoutMedia.Visibility = Visibility.Visible;
            //this.scoutMedia.Play();

        //}

        private void scoutCloseClick(object sender, RoutedEventArgs e)
        {
            scoutInfobox.Visibility = Visibility.Collapsed;
            //scoutMedia.Stop();
        }

        Point m_start_strategy1;
        Point temp_strategy1;
        Vector m_startOffset_strategy1;
        TouchDevice infoboxControl_strategy1;

        private void strategy_TouchDown1(object sender, TouchEventArgs e)
        {
            m_start_strategy1 = e.GetTouchPoint(this).Position;
            m_startOffset_strategy1 = new Vector(this.strategyTransform1.X, this.strategyTransform1.Y);
            e.TouchDevice.Capture(this.strategy1);
            if (infoboxControl_strategy1 == null)
            {
                infoboxControl_strategy1 = e.TouchDevice;
            }

            e.Handled = true;
        }

        private void strategy_TouchMove1(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_strategy1)
            {
                temp_strategy1 = e.GetTouchPoint(this).Position;
                Vector offset = Point.Subtract(temp_strategy1, m_start_strategy1);

                this.strategyTransform1.X = m_startOffset_strategy1.X + offset.X;
                this.strategyTransform1.Y = m_startOffset_strategy1.Y + offset.Y;
            }
            e.Handled = true;
        }

        private void strategy_TouchLeave1(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == infoboxControl_strategy1)
            {
                infoboxControl_strategy1 = null;
            }
            e.Handled = true;
        }


        private void strategyVideoClick1(object sender, RoutedEventArgs e)
        {
            //this.strategyMedia1.Visibility = Visibility.Visible;
            //this.strategyMedia1.Play();

        }

        private void strategyCloseClick1(object sender, RoutedEventArgs e)
        {
            strategy1.Visibility = Visibility.Collapsed;
            //strategyMedia1.Stop();
            //this.strategyMedia1.Visibility = Visibility.Collapsed;
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

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
            MCquestion obj = new MCquestion();
            obj.question = "INITIAL!";
            this.DataContext = obj;

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
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

            this.MyDrawObject = new Draw(MyMap)
            {
                LineSymbol = CanvasID.Resources["RedLineSymbol"] as ESRI.ArcGIS.Client.Symbols.CartographicLineSymbol
            };
            MyDrawObject.DrawComplete += drawing_EventHandler;
            MyDrawObject.DrawMode = DrawMode.Freehand;
            MyDrawObject.IsEnabled = false;

            this.MeasureObject = new Draw(MyMap)
            {
                LineSymbol = CanvasID.Resources["BlueLineSymbol"] as ESRI.ArcGIS.Client.Symbols.CartographicLineSymbol
            };
            MeasureObject.VertexAdded += measure_EventHandler;
            MeasureObject.DrawMode = DrawMode.Polyline;
            MeasureObject.IsEnabled = false;
            measureList = new List<ESRI.ArcGIS.Client.Geometry.MapPoint>();

            mapLock = false;
            this.measure = false;
            MyMap.MaximumResolution = 10000;
            MyMap.MinimumResolution = 5;
            // MyMap.Zoom(2);
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        private void InitializeDefinitions()
        {
            for (byte k = 1; k <= 1; k++)
            {
                TagVisualizationDefinition tagDef =
                    new TagVisualizationDefinition();
                // The tag value that this definition will respond to.
                tagDef.Value = k;
                // The .xaml file for the UI
                tagDef.Source =
                    new Uri("CameraVisualization.xaml", UriKind.Relative);
                // The maximum number for this tag value.
                tagDef.MaxCount = 2;
                // The visualization stays for 2 seconds.
                tagDef.LostTagTimeout = 20000.0;
                // Orientation offset (default).
                tagDef.OrientationOffsetFromTag = 0.0;
                // Physical offset (horizontal inches, vertical inches).
                tagDef.PhysicalCenterOffsetFromTag = new Vector(2.0, 2.0);
                // Tag removal behavior (default).
                tagDef.TagRemovedBehavior = TagRemovedBehavior.Fade;
                // Orient UI to tag? (default).
                tagDef.UsesTagOrientation = true;
                // Add the definition to the collection.
                MyTagVisualizer.Definitions.Add(tagDef);
            }

            
        }

        private void removeAnswers()
        {
            
        }

        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            camera = (CameraVisualization)e.TagVisualization;
            string[] tagValue = { "Enemy1", "Enemy2", "Enemy3" , "Ally"};

            for (int i = 1; i < Symbols.Count()+1; i++)
            {
                TagElement objectOnSurface = Symbols.ElementAt(i-1);
                if (camera.VisualizedTag.Value == objectOnSurface.tagValue)
                {
                    camera.setIcon(objectOnSurface.iconPath);
                    camera.setInfo("This is a car");
                    //camera.CameraModel.Content = objectOnSurface.stringName + " is at " + camera.Center.X.ToString() + ","+ camera.Center.Y.ToString();
                    //camera.myEllipse.Fill = new SolidColorBrush(System.Windows.Media.Colors.Blue); 
                    if (objectOnSurface.tagValue == 4)
                    {
                        SurfaceButton testControl1 = new SurfaceButton();
                        testControl1.Content = "Perform Action";
                        testControl1.Click += new RoutedEventHandler(secretCode);
                        camera.testControl.Content = testControl1;

                        Label testLabel1 = new Label();
                        testLabel1.Content = "Base";
                        camera.CameraModelControl.Content = testLabel1;
                        //MCanswer = new RoutedEventHandler(testMC);
                        //MCButton1.Click += MCanswer;
                    }
                    if (objectOnSurface.tagValue == 32)
                    {
                        SurfaceButton testControl32 = new SurfaceButton();
                        testControl32.Content = "Perform Action";
                        testControl32.Click += new RoutedEventHandler(secretCodex20);
                        camera.testControl.Content = testControl32;

                        Label testLabel32 = new Label();
                        testLabel32.Content = "Trench";
                        camera.CameraModelControl.Content = testLabel32;
                        //MCanswer = new RoutedEventHandler(testMC);
                        //MCButton1.Click += MCanswer;
                    }
                    if (objectOnSurface.tagValue == 33)
                    {
                        SurfaceButton testControl33 = new SurfaceButton();
                        testControl33.Content = "Perform Action";
                        testControl33.Click += new RoutedEventHandler(secretCodex21);
                        camera.testControl.Content = testControl33;

                        Label testLabel33 = new Label();
                        testLabel33.Content = "Mission";
                        camera.CameraModelControl.Content = testLabel33;
                        //MCanswer = new RoutedEventHandler(testMC);
                        //MCButton1.Click += MCanswer;
                    }
                    if (objectOnSurface.tagValue == 34)
                    {
                        SurfaceButton testControl34 = new SurfaceButton();
                        testControl34.Content = "Perform Action";
                        testControl34.Click += new RoutedEventHandler(secretCodex22);
                        camera.testControl.Content = testControl34;

                        Label testLabel34 = new Label();
                        testLabel34.Content = "Aerial";
                        camera.CameraModelControl.Content = testLabel34;
                        //MCanswer = new RoutedEventHandler(testMC);
                        //MCButton1.Click += MCanswer;
                    }
                    if (objectOnSurface.tagValue == 35)
                    {
                        SurfaceButton testControl35 = new SurfaceButton();
                        testControl35.Content = "Perform Action";
                        testControl35.Click += new RoutedEventHandler(FinalTest);
                        camera.testControl.Content = testControl35;

                        Label testLabel35 = new Label();
                        testLabel35.Content = "Headquarters";
                        camera.CameraModelControl.Content = testLabel35;
                        //MCanswer = new RoutedEventHandler(testMC);
                        //MCButton1.Click += MCanswer;
                    }
                    if (objectOnSurface.tagValue == 36)
                    {
                        SurfaceButton testControl36 = new SurfaceButton();
                        testControl36.Content = "Perform Action";
                        testControl36.Click += new RoutedEventHandler(testwindow);
                        camera.testControl.Content = testControl36;

                        Label testLabel36 = new Label();
                        testLabel36.Content = "TESTING";
                        camera.CameraModelControl.Content = testLabel36;
                        //MCanswer = new RoutedEventHandler(testMC);
                        //MCButton1.Click += MCanswer;
                    }
                    if (objectOnSurface.tagValue == 2)
                    {
                        SurfaceButton testControl4 = new SurfaceButton();
                        testControl4.Content = "Ask Question";
                        testControl4.Click += new RoutedEventHandler(secretCode);
                        camera.testControl.Content = testControl4;
                    }
                    if (objectOnSurface.tagValue == 3)
                    {
                        SurfaceButton testControl2 = new SurfaceButton();
                        testControl2.Content = "Decrypter";
                        testControl2.Click += new RoutedEventHandler(decryptCode);
                        camera.testControl.Content = testControl2;

                        camera.CustomMenu.Header = "Code";
                        camera.CM1.Header = "Alpha";
                        camera.CM1.Click += new RoutedEventHandler(decryptAlpha);
                        camera.CM2.Header = "Omega";
                        camera.CM2.Click += new RoutedEventHandler(decryptOmega);
                        camera.CM3.Header = "Gamma";
                        camera.CM3.Click += new RoutedEventHandler(decryptGamma);
                        camera.CM4.Header = "Delta";
                        camera.CM4.Click += new RoutedEventHandler(decryptDelta);
                        String blah = "C:\\Users\\SURFACE\\Documents\\Visual Studio 2010\\Projects\\CameraVisualizations\\CameraVisualizations\\images\\decrypt.png";
                        camera.tagImage.Source = new Uri(@blah);
                    }
                    if (objectOnSurface.tagValue == 1)
                    {
                        SurfaceButton testControl3 = new SurfaceButton();
                        testControl3.Content = "Recon: Get a fix on target";
                        testControl3.Click += new RoutedEventHandler(performRecon);
                        camera.testControl.Content = testControl3;

                        camera.CustomMenu.Header = "Target";
                        camera.CM1.Header = "Base";
                        camera.CM1.Click += new RoutedEventHandler(baseRecon);
                        camera.CM2.Header = "Trench";
                        camera.CM2.Click += new RoutedEventHandler(trenchRecon);
                        camera.CM3.Header = "Mission";
                        camera.CM3.Click += new RoutedEventHandler(missionRecon);
                        camera.CM4.Header = "Aerial";
                        camera.CM4.Click += new RoutedEventHandler(aerialRecon);
                        camera.tagImage.Source = new Uri(@"C:\Users\SURFACE\Documents\Visual Studio 2010\Projects\CameraVisualizations\CameraVisualizations\images\recon.png");
                    }
                }
            }
            e.Handled = true;

        }

        private void testwindow(object sender, RoutedEventArgs e)
        {

            strategy1.Visibility = Visibility.Visible;
        }

        private void decryptAlpha(object sender, RoutedEventArgs e)
        {
            TextBlock result = new TextBlock();
            result.Text = "7";
            result.FontSize = 20;
            decryptContentControl.Content = result;
        }
        private void decryptOmega(object sender, RoutedEventArgs e)
        {
            TextBlock result = new TextBlock();
            result.Text = "8";
            result.FontSize = 20;
            decryptContentControl.Content = result;
        }
        private void decryptGamma(object sender, RoutedEventArgs e)
        {
            TextBlock result = new TextBlock();
            result.Text = "3";
            result.FontSize = 20;
            decryptContentControl.Content = result;
        }
        private void decryptDelta(object sender, RoutedEventArgs e)
        {
            TextBlock result = new TextBlock();
            result.Text = "1";
            result.FontSize = 20;
            decryptContentControl.Content = result;
        }

        private void decryptCode(object sender, RoutedEventArgs e)
        {
            Infobox.Visibility = Visibility.Visible;
        }

        private void baseRecon(object sender, EventArgs e)
        {
            TextBlock result = new TextBlock();
            result.Text = "First";
            result.FontSize = 20;
            reconContentControl.Content = result;
        }

        private void trenchRecon(object sender, EventArgs e)
        {

            TextBlock result = new TextBlock();
            result.Text = "Second";
            result.FontSize = 20;
            reconContentControl.Content = result;
        }
        private void missionRecon(object sender, EventArgs e)
        {
            TextBlock result = new TextBlock();
            result.Text = "Fourth";
            result.FontSize = 20;
            reconContentControl.Content = result;
        }
        private void aerialRecon(object sender, EventArgs e)
        {
            TextBlock result = new TextBlock();
            result.Text = "Third";
            result.FontSize = 20;
            reconContentControl.Content = result;
        }

        private void performRecon(object sender, RoutedEventArgs e)
        {
            scoutInfobox.Visibility = Visibility.Visible;
        }

        //1
        private void MC01(object sender, RoutedEventArgs e)
        {
            this.correctSound.Stop();
            this.correctSound.Play();

            Label code = new Label();
            code.Content = "Alpha";
            this.landmarkContentControl.Content = code;
        }
        private void MC02(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }
        private void MC03(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }
            
        private void MC04(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }

        private void secretCode(object sender, RoutedEventArgs e)
        {
            
                    landmarkInfobox.Visibility = Visibility.Visible;

        }

        //3
        private void MC11(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }
        private void MC12(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }
        private void MC13(object sender, RoutedEventArgs e)
        {
            this.correctSound.Stop();
            this.correctSound.Play();

            Label code = new Label();
            code.Content = "Gamma";
            this.landmarkContentControl2.Content = code;
        }

        private void MC14(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }

        private void secretCodex20(object sender, RoutedEventArgs e)
        {
            
                    landmarkInfobox2.Visibility = Visibility.Visible;

        }

        //4
        private void MC21(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }
        private void MC22(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }
        private void MC23(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }

        private void MC24(object sender, RoutedEventArgs e)
        {
            this.correctSound.Stop();
            this.correctSound.Play();

            Label code = new Label();
            code.Content = "Omega";
            this.landmarkContentControl3.Content = code;
        }

        private void secretCodex21(object sender, RoutedEventArgs e)
        {

            landmarkInfobox3.Visibility = Visibility.Visible;

        }

        //1
        private void MC31(object sender, RoutedEventArgs e)
        {
            this.correctSound.Stop();
            this.correctSound.Play();

            Label code = new Label();
            code.Content = "Delta";
            this.landmarkContentControl4.Content = code;
        }
        private void MC32(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }
        private void MC33(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }

        private void MC34(object sender, RoutedEventArgs e)
        {
            this.wrongSound.Stop();
            this.wrongSound.Play();
        }

        private void secretCodex22(object sender, RoutedEventArgs e)
        {

            landmarkInfobox4.Visibility = Visibility.Visible;

        }

        private void FinalBossAnswerClick(object sender, RoutedEventArgs e)
        {
            if (firstDigit7.IsChecked == true && secondDigit3.IsChecked == true && thirdDigit1.IsChecked == true && fourthDigit8.IsChecked == true)
            {
                this.correctSound.Stop();
                this.correctSound.Play();
            }
            else
            {
                this.FinalBossMedia.Stop();
                this.FinalBossMedia.Visibility = Visibility.Visible;
                this.FinalBossMedia.Play();
            }
        }

        private void FinalTest(object sender, RoutedEventArgs e)
        {

            FinalBoss.Visibility = Visibility.Visible;

        }

        public class MCquestion
        {
            public String question { get; set; }
        }

        private void ChangeQuestion(object sender, RoutedEventArgs e)
        {
            MCquestion obj = new MCquestion();
            obj.question = "Hello!";
            this.DataContext = obj;
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

        void pin_MouseDown(object sender, MouseButtonEventArgs e, String s)
        {
            //Infobox.Visibility = Visibility.Visible;
        }

        void pin_TouchDown(object sender, TouchEventArgs e, String s)
        {
            /*if (Infobox.Visibility != Visibility.Visible)
            {
                this.Profile.Text = s;
                Pushpin p = sender as Pushpin;
                MapLayer.SetPosition(Infobox, MapLayer.GetPosition(p));
                Infobox.Visibility = Visibility.Visible;
            }
            else
            {
                Infobox.Visibility = Visibility.Collapsed;
            }*/

            
        }

        private void mapLock_Click(object sender, RoutedEventArgs e)
        {
           /* if(this.myMap.SupportedManipulations == System.Windows.Input.Manipulations.Manipulations2D.None){
                this.myMap.SupportedManipulations = System.Windows.Input.Manipulations.Manipulations2D.All;
            }
            else{
                this.myMap.SupportedManipulations = System.Windows.Input.Manipulations.Manipulations2D.None;
            }*/

            SurfaceButton sfButton = (SurfaceButton)sender;
            var brush = new ImageBrush();

            MyMap.IsHitTestVisible = !MyMap.IsHitTestVisible;
            this.mapLock = !this.mapLock;

            if (this.mapLock)
            {
                brush.ImageSource = new BitmapImage(new Uri("images/unlock.png",UriKind.Relative));
                this.MapLock.Content = "Locked";
            }
            else
            {
                brush.ImageSource = new BitmapImage(new Uri("images/lock.png",UriKind.Relative));
                this.MapLock.Content = "Unlocked";
            }

            sfButton.Background = brush;
            
            /*if (mapLock == false)
            {
                mapLock = true;
                panValue = MyMap.PanDuration;
                MyMap.PanDuration = TimeSpan.FromSeconds(0);
            }
            else
            {
                mapLock = false;
                MyMap.PanDuration = panValue;
            }*/
        }


        private void draw_Click(object sender, RoutedEventArgs e)
        {
            /*MyMap.Cursor = System.Windows.Input.Cursors.Pen;
            SimpleRenderer newRend = new SimpleRenderer
            {
                Symbol = new ESRI.ArcGIS.Client.Symbols.LineSymbol
                {
                    Color = System.Windows.Media.Brushes.Red,
                    Width = 50
                }
            };*/

            measure = false;
            MyDrawObject.IsEnabled = !MyDrawObject.IsEnabled;
            if (MyDrawObject.IsEnabled)
            {
                CheckDrawButton.Visibility = Visibility.Visible;
            }
            else 
            {
                CheckDrawButton.Visibility = Visibility.Collapsed;
            }
            
        }

        private void erase_Click(object sender, RoutedEventArgs e)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["drawingLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Clear();

            this.MyDrawObject = null;
            this.MyDrawObject = new Draw(MyMap)
            {
                LineSymbol = CanvasID.Resources["RedLineSymbol"] as ESRI.ArcGIS.Client.Symbols.CartographicLineSymbol
            };
            MyDrawObject.DrawComplete += drawing_EventHandler;
            MyDrawObject.DrawMode = DrawMode.Freehand;
            MyDrawObject.IsEnabled = false;
        }

        public void drawing_EventHandler(object sender, ESRI.ArcGIS.Client.DrawEventArgs args)
        {
            Graphic graphic = new Graphic()
            {
                Geometry = args.Geometry,
                Symbol = CanvasID.Resources["RedLineSymbol"] as ESRI.ArcGIS.Client.Symbols.CartographicLineSymbol
            };
            GraphicsLayer graphicsLayer = MyMap.Layers["drawingLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Add(graphic);
        }

        public double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(lat1 / 180 * Math.PI) * Math.Cos(lat2 / 180 * Math.PI)
                        * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            //Calculate radius of earth
            // For this you can assume any of the two points.
            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                            + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            //Calculate distance in meters.
            distance = radius * c;
            return distance; // distance in meters
        }


        //private void Video_Click(object sender, RoutedEventArgs e)
        //{
            //this.media.Visibility = Visibility.Visible;
            //this.media.Play();
            
        //}

        public void BaseUriHelper(){
            //this.media.Visibility = Visibility.Visible;
            //this.media.Play();
        }

       

        private void infoboxClose(object sender, RoutedEventArgs e)
        {
            Infobox.Visibility = Visibility.Collapsed;
            //media.Stop();
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void btnRuler_Click(object sender, RoutedEventArgs e)
        {
            this.measure = !this.measure;
            GraphicsLayer graphicsLayer = MyMap.Layers["measureLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Clear();
            measureList.Clear();
            DistanceInfo.Text = "Select points";
            
            MeasureObject.IsEnabled = !MeasureObject.IsEnabled;

            if (DistanceBox.Visibility == Visibility.Visible)
                DistanceBox.Visibility = Visibility.Collapsed;
            else
                DistanceBox.Visibility = Visibility.Visible;
        }

        public void measure_EventHandler(object sender, VertexAddedEventArgs args)
        {
            //DistanceInfo.Text = String.Concat(DistanceInfo.Text, "m");
            ESRI.ArcGIS.Client.Projection.WebMercator coordHelper = new ESRI.ArcGIS.Client.Projection.WebMercator();
            coordHelper.ToGeographic(args.Vertex);
            
            measureList.Add((ESRI.ArcGIS.Client.Geometry.MapPoint) coordHelper.ToGeographic(args.Vertex));
            DistanceBox.Visibility = Visibility.Visible;

            ESRI.ArcGIS.Client.Geometry.Polyline polyline = new ESRI.ArcGIS.Client.Geometry.Polyline();
            ESRI.ArcGIS.Client.Geometry.PointCollection pointCollection = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            
            foreach (ESRI.ArcGIS.Client.Geometry.MapPoint p in measureList)
            {
                pointCollection.Add(p);
            }
            System.Collections.ObjectModel.ObservableCollection<ESRI.ArcGIS.Client.Geometry.PointCollection> obsCollection = new System.Collections.ObjectModel.ObservableCollection<ESRI.ArcGIS.Client.Geometry.PointCollection>();
            obsCollection.Add(pointCollection);
            polyline.Paths = obsCollection;
            polyline.SpatialReference = measureList.First().SpatialReference;

            double Distance = ESRI.ArcGIS.Client.Geometry.Geodesic.Length(polyline)/1000;

            DistanceInfo.Text = Distance.ToString("N2");

            DistanceInfo.Text = String.Concat(DistanceInfo.Text, " km");
           
        }

        private void SurfaceWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

         
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
            returnString += stringName + "\nRank : " + rank + "\nThere are " + availableUnit + " units available." ;

            return returnString;
        }

    }
}