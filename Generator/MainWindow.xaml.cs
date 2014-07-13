using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;

namespace Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> strFiles;
        string lockListFile = string.Empty;
        string filePrefix;
        string FilePath;
        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            strFiles = new ObservableCollection<string>();
            lstGen.DataContext = strFiles;
        }

        private void btnGen_Click(object sender, RoutedEventArgs e)
        {
            FilePath = "C:\\suhas\\";
            filePrefix = "abc";
            Directory.CreateDirectory(FilePath);
            for (int i = 0; i < 10; i++)
            {
                Thread th = new Thread(this.generateFiles);
                th.Start();
            }
           
        }

        private void addFile(string file)
        {
            strFiles.Add(file);
            lstGen.ScrollIntoView(strFiles[strFiles.Count-1]);
        }

        private void generateFiles()
        {
            while (true)
            {
                StreamWriter swFile = null;
                try
                {
                    string fileName;
                    int fileNumber = rnd.Next();
                    string contains = "File Generated " + fileNumber;
                    fileName = FilePath + filePrefix + fileNumber + ".txt";
                    //lock (lockListFile)
                    //{
                    if (strFiles.Contains(fileName))
                        continue;

                    Dispatcher.BeginInvoke(DispatcherPriority.Send,
                                           new ThreadStart(() => addFile(fileName)));
                    //}
                    FileStream fsRandomFile = File.Create(fileName);
                    swFile = new StreamWriter(fsRandomFile);

                    while (swFile.BaseStream.Length <= 2048000)
                    {
                        swFile.WriteLine(contains);
                    }

                }
                catch (Exception)
                {

                }
                finally
                {
                    if (swFile != null)
                    {
                        swFile.Flush();
                        swFile.Close();
                    }
                }
            }
        }
    }
}
