using System.IO.Compression;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Pipes;
using static System.Net.Mime.MediaTypeNames;

namespace ReadE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<byte[]> images = new List<byte[]>();
        public int currentIndex = 0;
        public MainWindow()
        {
            InitializeComponent();
            btnNext.IsEnabled = false;
            btnPrev.IsEnabled = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            images = new List<byte[]>();
            currentIndex = 0;


            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() != true)
                return;


            txtFileName.Text = fileDialog.FileName;

            using (ZipArchive zip = ZipFile.Open(fileDialog.FileName, ZipArchiveMode.Read))
            {
                
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                   
                    using (var entrystream = entry.Open())
                    {
                        
                        using (var mem = new MemoryStream())
                        {
                            entrystream.CopyTo(mem);
                            byte[] buffer = mem.ToArray();
                            images.Add(buffer);
                            //bitmap.Freeze();
                        }
                      
                    }
                    
                }

            }

            DisplayImage();

            btnNext.IsEnabled = true;
            btnPrev.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            
            if (button.Name == "btnNext")
                currentIndex++;
            else
                currentIndex--;

            if (currentIndex < 0)
                currentIndex = 0;

            if (currentIndex > images.Count -  1)
                currentIndex = images.Count - 1;

            DisplayImage();
        }

        private void IncreaseZoom(object sender, RoutedEventArgs e)
        {
            imgPage.Height += 500;
        }

        private void DisplayImage()
        {
            using (var newmem = new MemoryStream(images[currentIndex]))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = newmem;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                imgPage.Source = bitmap;
            }
        }
    }
}
