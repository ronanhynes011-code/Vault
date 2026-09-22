using System;
using System.Windows;
using System.Windows.Media;

namespace Vault
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!VaultApi.IsRobloxOpen())
            {
                StatusText.Text = "Roblox not detected";
                StatusText.Foreground = Brushes.IndianRed;
            }
        }

        private void InjectButton_Click(object sender, RoutedEventArgs e)
        {
            InjectButton.IsEnabled = false;
            StatusText.Text = "Injecting...";
            StatusText.Foreground = Brushes.Gold;

            try
            {
                VaultApi.Inject();
                StatusText.Text = "Attached";
                StatusText.Foreground = Brushes.LimeGreen;

                AttachedPopup.IsOpen = true;

                var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
                timer.Tick += (s, args) =>
                {
                    AttachedPopup.IsOpen = false;
                    timer.Stop();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                StatusText.Text = "Injection Failed";
                StatusText.Foreground = Brushes.IndianRed;
                MessageBox.Show(ex.Message, "Vault Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                InjectButton.IsEnabled = true;
            }
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ScriptBox.Text))
            {
                MessageBox.Show("Script is empty.", "Vault", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ExecuteButton.IsEnabled = false;
            try
            {
                VaultApi.Execute(ScriptBox.Text);
                StatusText.Text = "Executed";
                StatusText.Foreground = Brushes.LimeGreen;
            }
            catch (Exception ex)
            {
                StatusText.Text = "Execution Failed";
                StatusText.Foreground = Brushes.IndianRed;
                MessageBox.Show(ex.Message, "Vault Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ExecuteButton.IsEnabled = true;
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}