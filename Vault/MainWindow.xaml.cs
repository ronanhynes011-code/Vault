using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

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

            UpdateLineNumbers();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AccountTab_Click(object sender, RoutedEventArgs e)
        {
            AccountPanel.Visibility = Visibility.Visible;
            ScriptPanel.Visibility = Visibility.Collapsed;
            AccountTabBtn.Style = (Style)FindResource("TabBtnActive");
            ScriptTabBtn.Style = (Style)FindResource("TabBtn");
        }

        private void ScriptTab_Click(object sender, RoutedEventArgs e)
        {
            AccountPanel.Visibility = Visibility.Collapsed;
            ScriptPanel.Visibility = Visibility.Visible;
            AccountTabBtn.Style = (Style)FindResource("TabBtn");
            ScriptTabBtn.Style = (Style)FindResource("TabBtnActive");
        }

        private void NewTab_Click(object sender, RoutedEventArgs e)
        {
            ScriptBox.Text = "";
            UpdateLineNumbers();
        }

        private void ScriptBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLineNumbers();
        }

        private void UpdateLineNumbers()
        {
            if (LineNumbers == null || ScriptBox == null)
            {
                return;
            }

            int lineCount = ScriptBox.LineCount;
            if (lineCount < 1)
            {
                lineCount = 1;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= lineCount; i++)
            {
                sb.Append(i);
                if (i < lineCount)
                {
                    sb.Append('\n');
                }
            }
            LineNumbers.Text = sb.ToString();
        }

        private void AttachButton_Click(object sender, RoutedEventArgs e)
        {
            AttachButton.IsEnabled = false;
            StatusText.Text = "Attaching...";
            StatusText.Foreground = Brushes.Gold;

            try
            {
                VaultApi.Inject();
                StatusText.Text = "Attached";
                StatusText.Foreground = Brushes.LimeGreen;

                AttachedPopup.IsOpen = true;
                System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Tick += delegate(object s, EventArgs args)
                {
                    AttachedPopup.IsOpen = false;
                    timer.Stop();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                StatusText.Text = "Attach failed";
                StatusText.Foreground = Brushes.IndianRed;
                MessageBox.Show(ex.Message, "Vault", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                AttachButton.IsEnabled = true;
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
                StatusText.Text = "Execution failed";
                StatusText.Foreground = Brushes.IndianRed;
                MessageBox.Show(ex.Message, "Vault", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ExecuteButton.IsEnabled = true;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ScriptBox.Clear();
            UpdateLineNumbers();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Lua scripts (*.lua;*.txt)|*.lua;*.txt|All files (*.*)|*.*";
            dlg.Title = "Open script";

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    ScriptBox.Text = File.ReadAllText(dlg.FileName);
                    UpdateLineNumbers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to open: " + ex.Message, "Vault", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Lua scripts (*.lua)|*.lua|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.Title = "Save script";
            dlg.FileName = "script.lua";

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(dlg.FileName, ScriptBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save: " + ex.Message, "Vault", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DiscordSignIn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Discord sign-in will be wired up in the next build.", "Vault", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
