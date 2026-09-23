using System;
using System.Collections.ObjectModel;
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
        private readonly ObservableCollection<ScriptTab> _tabs = new ObservableCollection<ScriptTab>();
        private ScriptTab _activeTab;
        private bool _syncingEditor = false;

        public MainWindow()
        {
            InitializeComponent();

            if (!VaultApi.IsRobloxOpen())
            {
                StatusPill.Text = "Roblox not detected";
                StatusPill.Foreground = Brushes.IndianRed;
            }

            AddScriptTab("Welcome");
            AddScriptTab("Tab #1");
            RefreshScriptTabs();
        }

        // ---------- Window chrome ----------

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
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // ---------- Top nav ----------

        private void SetNav(Button active)
        {
            NavCodeBtn.Style = (Style)FindResource("TopIconBtn");
            NavAccountBtn.Style = (Style)FindResource("TopIconBtn");
            NavSettingsBtn.Style = (Style)FindResource("TopIconBtn");
            active.Style = (Style)FindResource("TopIconBtnActive");
        }

        private void ShowView(UIElement view)
        {
            ViewCode.Visibility = Visibility.Collapsed;
            ViewAccount.Visibility = Visibility.Collapsed;
            ViewSettings.Visibility = Visibility.Collapsed;
            view.Visibility = Visibility.Visible;
        }

        private void NavCode_Click(object sender, RoutedEventArgs e)
        {
            SetNav(NavCodeBtn);
            ShowView(ViewCode);
        }

        private void NavAccount_Click(object sender, RoutedEventArgs e)
        {
            SetNav(NavAccountBtn);
            ShowView(ViewAccount);
        }

        private void NavSettings_Click(object sender, RoutedEventArgs e)
        {
            SetNav(NavSettingsBtn);
            ShowView(ViewSettings);
        }

        // ---------- Script tabs ----------

        private void AddScriptTab(string name)
        {
            ScriptTab tab = new ScriptTab();
            tab.Name = name;
            tab.Content = "";
            _tabs.Add(tab);

            if (_activeTab != null)
                _activeTab.IsActive = false;

            _activeTab = tab;
            _activeTab.IsActive = true;

            _syncingEditor = true;
            ScriptBox.Text = tab.Content;
            _syncingEditor = false;
            UpdateLineNumbers();
        }

        private void RefreshScriptTabs()
        {
            ScriptTabsPanel.Children.Clear();

            foreach (ScriptTab tab in _tabs)
            {
                ScriptTab captured = tab;

                Grid row = new Grid();
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                TextBlock nameText = new TextBlock();
                nameText.Text = tab.Name;
                nameText.VerticalAlignment = VerticalAlignment.Center;
                nameText.FontSize = 12;
                nameText.FontWeight = tab.IsActive ? FontWeights.SemiBold : FontWeights.Normal;
                nameText.Foreground = tab.IsActive
                    ? (Brush)FindResource("Accent")
                    : (Brush)FindResource("TextMuted");
                Grid.SetColumn(nameText, 0);
                row.Children.Add(nameText);

                Button closeBtn = new Button();
                closeBtn.Style = (Style)FindResource("CloseDot");
                closeBtn.Content = "\u2715";
                closeBtn.Margin = new Thickness(10, 0, 0, 0);
                closeBtn.Tag = captured;
                closeBtn.Click += CloseTab_Click;
                Grid.SetColumn(closeBtn, 1);
                row.Children.Add(closeBtn);

                Button btn = new Button();
                btn.Style = (Style)FindResource(tab.IsActive ? "ScriptTabBtnActive" : "ScriptTabBtn");
                btn.Content = row;
                btn.Tag = captured;
                btn.Click += SelectTab_Click;

                ScriptTabsPanel.Children.Add(btn);
            }
        }

        private void SelectTab_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            ScriptTab tab = btn.Tag as ScriptTab;
            if (tab == null || tab == _activeTab) return;

            if (_activeTab != null)
                _activeTab.IsActive = false;

            _activeTab = tab;
            _activeTab.IsActive = true;

            _syncingEditor = true;
            ScriptBox.Text = tab.Content;
            _syncingEditor = false;
            UpdateLineNumbers();
            RefreshScriptTabs();
        }

        private void CloseTab_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            ScriptTab tab = btn.Tag as ScriptTab;
            if (tab == null) return;

            _tabs.Remove(tab);

            if (_tabs.Count == 0)
            {
                AddScriptTab("Welcome");
            }
            else if (tab == _activeTab)
            {
                _activeTab = _tabs[0];
                _activeTab.IsActive = true;
                _syncingEditor = true;
                ScriptBox.Text = _activeTab.Content;
                _syncingEditor = false;
                UpdateLineNumbers();
            }

            RefreshScriptTabs();
        }

        private void AddScriptTab_Click(object sender, RoutedEventArgs e)
        {
            AddScriptTab("Tab #" + (_tabs.Count + 1));
            RefreshScriptTabs();
        }

        // ---------- Editor ----------

        private void ScriptBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_syncingEditor && _activeTab != null)
                _activeTab.Content = ScriptBox.Text;
            UpdateLineNumbers();
        }

        private void ScriptBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int line = ScriptBox.GetLineIndexFromCharacterIndex(ScriptBox.CaretIndex);
            int col = ScriptBox.CaretIndex - ScriptBox.GetCharacterIndexFromLineIndex(line);
            CursorPosText.Text = "Ln " + (line + 1) + ", Col " + (col + 1);
        }

        private void UpdateLineNumbers()
        {
            if (LineNumbers == null || ScriptBox == null) return;

            int count = ScriptBox.LineCount;
            if (count < 1) count = 1;

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= count; i++)
            {
                sb.Append(i);
                if (i < count) sb.Append('\n');
            }
            LineNumbers.Text = sb.ToString();
        }

        // ---------- Toolbar ----------

        private void AttachButton_Click(object sender, RoutedEventArgs e)
        {
            AttachButton.IsEnabled = false;
            StatusPill.Text = "Attaching...";
            StatusPill.Foreground = Brushes.Gold;

            try
            {
                VaultApi.Inject();
                StatusPill.Text = "Injected";
                StatusPill.Foreground = Brushes.LimeGreen;

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
                StatusPill.Text = "Attach failed";
                StatusPill.Foreground = Brushes.IndianRed;
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

            try
            {
                VaultApi.Execute(ScriptBox.Text);
                StatusPill.Text = "Executed";
                StatusPill.Foreground = Brushes.LimeGreen;
            }
            catch (Exception ex)
            {
                StatusPill.Text = "Execution failed";
                StatusPill.Foreground = Brushes.IndianRed;
                MessageBox.Show(ex.Message, "Vault", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ScriptBox.Clear();
            if (_activeTab != null) _activeTab.Content = "";
            UpdateLineNumbers();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ScriptBox.Text))
            {
                try { Clipboard.SetText(ScriptBox.Text); } catch { }
            }
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
                    string content = File.ReadAllText(dlg.FileName);
                    ScriptBox.Text = content;
                    if (_activeTab != null)
                    {
                        _activeTab.Content = content;
                        _activeTab.Name = Path.GetFileName(dlg.FileName);
                    }
                    UpdateLineNumbers();
                    RefreshScriptTabs();
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
            dlg.FileName = (_activeTab != null ? _activeTab.Name : "script") + ".lua";

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(dlg.FileName, ScriptBox.Text);
                    if (_activeTab != null)
                    {
                        _activeTab.Name = Path.GetFileName(dlg.FileName);
                        RefreshScriptTabs();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save: " + ex.Message, "Vault", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DiscordSignIn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Discord sign-in will be wired up in the next build.",
                "Vault", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
