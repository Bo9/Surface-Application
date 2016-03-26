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

namespace CameraVisualizations
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Popup : Window
    {
        public Popup()
        {
            InitializeComponent();
        }

        private void Popup_TouchDown1(object sender, TouchEventArgs e)
        {
            this.DragMove();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void Window_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this;
            e.Handled = true;
        }

        void Window_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {

            // Get the Rectangle and its RenderTransform matrix.
            Rectangle rectToMove = e.OriginalSource as Rectangle;
            Matrix rectsMatrix = ((MatrixTransform)rectToMove.RenderTransform).Matrix;

            // Rotate the Rectangle.
            rectsMatrix.RotateAt(e.DeltaManipulation.Rotation,
                                 e.ManipulationOrigin.X,
                                 e.ManipulationOrigin.Y);

            // Resize the Rectangle.  Keep it square 
            // so use only the X value of Scale.
            rectsMatrix.ScaleAt(e.DeltaManipulation.Scale.X,
                                e.DeltaManipulation.Scale.X,
                                e.ManipulationOrigin.X,
                                e.ManipulationOrigin.Y);

            // Move the Rectangle.
            rectsMatrix.Translate(e.DeltaManipulation.Translation.X,
                                  e.DeltaManipulation.Translation.Y);

            // Apply the changes to the Rectangle.
            rectToMove.RenderTransform = new MatrixTransform(rectsMatrix);

            Rect containingRect =
                new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);
            
            Rect shapeBounds =
                rectToMove.RenderTransform.TransformBounds(
                    new Rect(rectToMove.RenderSize));

            // Check if the rectangle is completely in the window.
            // If it is not and intertia is occuring, stop the manipulation.
            if (e.IsInertial && !containingRect.Contains(shapeBounds))
            {
                e.Complete();
            }

        }

        void Window_InertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {

            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 pixels per inch / 1000ms^2)
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 pixels per inch / (1000ms^2)
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);

            e.Handled = true;
        }
    }
}
