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

        // ---------------- Window chrome ----------------

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void Maximize_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;

        private void Close_Click(object sender, RoutedEventArgs e)
            => Close();

        // ---------------- Tab switching ----------------

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
            // Placeholder for future multi-tab script editing.
            ScriptBox.Text = "-- New tab\n";
        }

        // ---------------- Line numbers ----------------

        private void ScriptBox_TextChanged(object sender, TextChangedEventArgs e)
            => UpdateLineNumbers();

        private void UpdateLineNumbers()
        {
            if (LineNumbers == null) return;

            int lineCount = ScriptBox.LineCount;
            if (lineCount < 1) lineCount = 1;

            var sb = new StringBuilder();
            for (int i = 1; i <= lineCount; i++)
            {
                sb.Append(i);
                if (i < lineCount) sb.Append('\n');
            }
            LineNumbers.Text = sb.ToString();
        }

        private void ScriptBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (LineNumberScroller != null && e.VerticalChange != 0)
                LineNumberScroller.ScrollToVerticalOffset(e.VerticalOffset);
        }

        // ---------------- Bottom toolbar ----------------

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
                var timer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(3)
                };
                timer.Tick += (s, args) =>
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
                MessageBox.Show("Script is empty.", "Vault",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
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
            var dlg = new OpenFileDialog
            {
                Filter = "Lua scripts (*.lua;*.txt)|*.lua;*.txt|All files (*.*)|*.*",
                Title = "Open script"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    ScriptBox.Text = File.ReadAllText(dlg.FileName);
                    UpdateLineNumbers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to open: " + ex.Message, "Vault",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "Lua scripts (*.lua)|*.lua|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save script",
                FileName = "script.lua"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(dlg.FileName, ScriptBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save: " + ex.Message, "Vault",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DiscordSignIn_Click(object sender, RoutedEventArgs e)
        {
            // Wired up in the next phase, once the Cloudflare Worker exists.
            MessageBox.Show(
                "Discord sign-in will be wired up in the next build.\nFor now, use Attach to inject directly.",
                "Vault", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}using System;
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
