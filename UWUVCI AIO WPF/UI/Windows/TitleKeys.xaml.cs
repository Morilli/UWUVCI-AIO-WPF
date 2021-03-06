﻿using GameBaseClassLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UWUVCI_AIO_WPF.UI.Frames;
using UWUVCI_AIO_WPF.UI.Frames.KeyFrame;

namespace UWUVCI_AIO_WPF.UI.Windows
{
    /// <summary>
    /// Interaktionslogik für TitleKeys.xaml
    /// </summary>
    public partial class TitleKeys : Window
    {
        string url = "";
        public TitleKeys(string url)
        {
            MainViewModel mvm = FindResource("mvm") as MainViewModel;
            try
            {
                if (this.Owner.GetType() != typeof(MainWindow))
                {
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            catch (Exception)
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            string fullurl = "";
            try
            {
                fullurl = mvm.GetURL(url);
                if (fullurl == "" || fullurl == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Custom_Message cm = new Custom_Message("Not Implemented", $"The Helppage for {url.ToUpper()} is not implemented yet");
                try
                {
                    cm.Owner = mvm.mw;
                }
                catch (Exception)
                {

                }
                cm.ShowDialog();
                this.Close();
                mvm.mw.Show();
            }
            
            wb.Source = new Uri(fullurl, UriKind.Absolute);
            wb.Refresh(true);

            clsWebbrowser_Errors.SuppressscriptErrors(wb, true);
            InitializeComponent();

            this.url = url;
           

        }
        public TitleKeys(string url, string title)
        {
            InitializeComponent();
            wb.Visibility = Visibility.Hidden;
            Thread t = new Thread(() => DoStuff(url));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            tbTitleBar.Text = title.Replace(" Inject "," ");
           
        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    this.Owner.PointToScreen(new Point(this.Left, this.Top));
                    this.DragMove();
                   // this.Owner.PointFromScreen(new Point(this.Left, this.Top));
                    //(FindResource("mvm") as MainViewModel).mw.PointFromScreen(new Point(this.Left, this.Top));
                }
                   
                //PointFromScreen(new Point(this.Left, this.Top));
               
            }
            catch (Exception)
            {

            }
        }

        public void DoStuff(string url)
        {
            MainViewModel mvm = FindResource("mvm") as MainViewModel;
            string fullurl = "";
            try
            {
                fullurl = mvm.GetURL(url);
                if (fullurl == "" || fullurl == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Custom_Message cm = new Custom_Message("Not Implemented", $"The Helppage for {url.ToUpper()} is not implemented yet");
                try
                {
                    cm.Owner = mvm.mw;
                }
                catch (Exception)
                {

                }
                cm.Show();
                this.Close();
                mvm.mw.Show();
            }

            this.Dispatcher.Invoke(() =>
            {
                wb.Source = new Uri(fullurl, UriKind.Absolute);
                clsWebbrowser_Errors.SuppressscriptErrors(wb, true);
            });
            
            /*dynamic activeX = this.wb.GetType().InvokeMember("ActiveXInstance",
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, this.wb, new object[] { });

            activeX.Silent = true;*/
           
        }

        private void Window_Close(object sender, RoutedEventArgs e)
        {
            this.Owner.Left = this.Left;
            this.Owner.Top = this.Top;
            this.Owner.Show();
            this.Close();
        }

        private void Window_Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void wb_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            load.Visibility = Visibility.Hidden;
            wb.Visibility = Visibility.Visible;
        }


      
        private void wb_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            
                if (!e.Uri.ToString().Contains("flumpster"))
                {
                    e.Cancel = true;

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = e.Uri.ToString()
                    };

                    Process.Start(startInfo);
                }
            
           

            // cancel navigation to the clicked link in the webBrowser control
           
        }

        private void wb_LoadCompleted_1(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
        private void wind_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (FindResource("mvm") as MainViewModel).mw.Topmost = true;
        }

        private void wind_Closed(object sender, EventArgs e)
        {
            (FindResource("mvm") as MainViewModel).mw.Topmost = false;
        }
    }
    public static class clsWebbrowser_Errors

    {

        //*set wpf webbrowser Control to silent

        //*code source: https://social.msdn.microsoft.com/Forums/vstudio/en-US/4f686de1-8884-4a8d-8ec5-ae4eff8ce6db



        public static void SuppressscriptErrors(this WebBrowser webBrowser, bool hide)

        {

            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);

            if (fiComWebBrowser == null)

                return;

            object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);

            if (objComWebBrowser == null)

                return;



            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });

        }
       
    }
}
