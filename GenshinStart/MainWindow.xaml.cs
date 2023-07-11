using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using GiveMeOpop;

namespace GenshinStart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            FileInfo genshin_video = new FileInfo("genshin_start.mp4");
            
            using (var vs = genshin_video.OpenWrite())
            {
                vs.Write(Properties.Resources.genshin_start, 0, Properties.Resources.genshin_start.Length); 
            }

            InitializeComponent();

            player.Source = new Uri(genshin_video.FullName);
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Process? p = App.StartGenshinGame(App.GenshinPath);

            if (p != null)
                NativeMethods.DontStopProcess(p);

            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {

            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
