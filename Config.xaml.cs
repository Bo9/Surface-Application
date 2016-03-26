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
using System.IO;
using Microsoft.Win32;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace CameraVisualizations
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    /// 

    public partial class Config : SurfaceWindow
    {
        sourceFiles obj = new sourceFiles();
        SurfaceWindow1 SW = (SurfaceWindow1)Application.Current.MainWindow;

        public Config()
        {
            InitializeComponent();

            //default video source paths
            obj.landmark1 = "C:\\Users\\SURFACE\\Documents\\Visual Studio 2010\\Projects\\CameraVisualizations\\CameraVisualizations\\video\\test.wmv";
            obj.landmark2 = "C:\\Users\\SURFACE\\Documents\\Visual Studio 2010\\Projects\\CameraVisualizations\\CameraVisualizations\\video\\test.wmv";
            obj.landmark3 = "C:\\Users\\SURFACE\\Documents\\Visual Studio 2010\\Projects\\CameraVisualizations\\CameraVisualizations\\video\\test.wmv";
            obj.landmark4 = "C:\\Users\\SURFACE\\Documents\\Visual Studio 2010\\Projects\\CameraVisualizations\\CameraVisualizations\\video\\echelonhint.mp4";
            SW.landmarkMedia.Source = new Uri(@obj.landmark1);
            SW.landmarkMedia2.Source = new Uri(@obj.landmark2);
            SW.landmarkMedia3.Source = new Uri(@obj.landmark3);
            SW.landmarkMedia4.Source = new Uri(@obj.landmark4);
        }

        private void display_file_contents(object sender, RoutedEventArgs e)
        {
            String File_name = "config.txt";
            if (System.IO.File.Exists(File_name) == true)
            {
                System.IO.StreamReader objReader;
                objReader = new StreamReader(File_name);
                textBox1.Text = objReader.ReadToEnd();
                objReader.Close();
            }
            else
            {
                MessageBox.Show("File does not exist");
            }
        }

        private void write_to_file(object sender, RoutedEventArgs e)
        {
            String File_name = "config.txt";
            if (System.IO.File.Exists(File_name) == true)
            {
                FileStream fs = new FileStream(File_name, FileMode.Append, FileAccess.Write);
                StreamWriter objWrite = new StreamWriter(fs);
                objWrite.Write(textBox1.Text);
                objWrite.Close();
                File.AppendAllText(File_name, Environment.NewLine);
                //Use to clear contents of text file
                // File.WriteAllText(File_name, String.Empty);
            }
            else
            {
                FileStream fs = new FileStream(File_name, FileMode.Create, FileAccess.Write);
                StreamWriter objWrite = new StreamWriter(fs);
                objWrite.Write(textBox1.Text);
                objWrite.Close();
            }
            textBox1.Clear();
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void selectFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                String[] temp = openFileDialog.FileName.Split('\\');
                String final = "";
                for (int i = 0; i < temp.Length; i++)
                {
                    if (i == temp.Length - 1)
                    {
                        final += temp[i];
                    }
                    else
                    {
                        final += temp[i] + @"\\";
                    }
                }
                textBox1.Text = final;
                //textbox1.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void updateSource(object sender, RoutedEventArgs e)
        {
            if (this.comboBox1.Text == "Landmark1")
            {
                obj.landmark1 = this.textBox1.Text;
                SW.landmarkMedia.Source = new Uri(@obj.landmark1);
                MessageBox.Show("Source path has been changed.");
            }
            if (this.comboBox1.Text == "Landmark2")
            {
                obj.landmark2 = this.textBox1.Text;
                SW.landmarkMedia2.Source = new Uri(@obj.landmark2);
                MessageBox.Show("Source path has been changed.");
            }
            if (this.comboBox1.Text == "Landmark3")
            {
                obj.landmark3 = this.textBox1.Text;
                SW.landmarkMedia3.Source = new Uri(@obj.landmark3);
                MessageBox.Show("Source path has been changed.");
            }
            if (this.comboBox1.Text == "Landmark4")
            {
                obj.landmark4 = this.textBox1.Text;
                SW.landmarkMedia4.Source = new Uri(@obj.landmark4);
                MessageBox.Show("Source path has been changed.");
            }
        }
    }

    public class sourceFiles
    {
        public String landmark1 { get; set; }
        public String landmark2 { get; set; }
        public String landmark3 { get; set; }
        public String landmark4 { get; set; }
    }
}
