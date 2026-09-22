<Window x:Class="Vault.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Vault" Height="600" Width="960"
        WindowStartupLocation="CenterScreen"
        Background="#0A0A0C"
        Foreground="#E0E0E0"
        FontFamily="Segoe UI"
        ResizeMode="CanResizeWithGrip"
        WindowStyle="None"
        AllowsTransparency="True">

    <Window.Resources>
        <SolidColorBrush x:Key="BgDark" Color="#0A0A0C"/>
        <SolidColorBrush x:Key="BgPanel" Color="#141418"/>
        <SolidColorBrush x:Key="BgCard" Color="#1C1C22"/>
        <SolidColorBrush x:Key="BgEditor" Color="#0D0D10"/>
        <SolidColorBrush x:Key="AccentGold" Color="#D4AF37"/>
        <SolidColorBrush x:Key="AccentGoldHover" Color="#F0C75E"/>
        <SolidColorBrush x:Key="TextMuted" Color="#7A7A82"/>
        <SolidColorBrush x:Key="BorderColor" Color="#22222A"/>
        <SolidColorBrush x:Key="LineNumber" Color="#3A3A44"/>

        <Style x:Key="WinBtn" TargetType="Button">
            <Setter Property="Background" Value="Transparent"/>
            <Setter Property="Foreground" Value="{StaticResource TextMuted}"/>
            <Setter Property="BorderThickness" Value="0"/>
            <Setter Property="Width" Value="38"/>
            <Setter Property="Height" Value="28"/>
            <Setter Property="Cursor" Value="Hand"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Border Background="{TemplateBinding Background}">
                            <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center"/>
                        </Border>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter Property="Background" Value="#1F1F26"/>
                                <Setter Property="Foreground" Value="{StaticResource AccentGold}"/>
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <Style x:Key="CloseBtn" TargetType="Button" BasedOn="{StaticResource WinBtn}">
            <Style.Triggers>
                <Trigger Property="IsMouseOver" Value="True">
                    <Setter Property="Background" Value="#3A1414"/>
                    <Setter Property="Foreground" Value="#FF6B6B"/>
                </Trigger>
            </Style.Triggers>
        </Style>

        <Style x:Key="TabBtn" TargetType="Button">
            <Setter Property="Background" Value="Transparent"/>
            <Setter Property="Foreground" Value="{StaticResource TextMuted}"/>
            <Setter Property="BorderThickness" Value="0"/>
            <Setter Property="Padding" Value="16,6"/>
            <Setter Property="Cursor" Value="Hand"/>
            <Setter Property="FontSize" Value="12"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Border Background="{TemplateBinding Background}" CornerRadius="6" Margin="2,4">
                            <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center"/>
                        </Border>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter Property="Background" Value="#1A1A20"/>
                                <Setter Property="Foreground" Value="{StaticResource AccentGold}"/>
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <Style x:Key="TabBtnActive" TargetType="Button" BasedOn="{StaticResource TabBtn}">
            <Setter Property="Background" Value="{StaticResource BgEditor}"/>
            <Setter Property="Foreground" Value="{StaticResource AccentGold}"/>
            <Setter Property="FontWeight" Value="SemiBold"/>
        </Style>

        <Style x:Key="BarBtn" TargetType="Button">
            <Setter Property="Background" Value="Transparent"/>
            <Setter Property="Foreground" Value="#B0B0B8"/>
            <Setter Property="BorderThickness" Value="0"/>
            <Setter Property="Padding" Value="14,6"/>
            <Setter Property="Cursor" Value="Hand"/>
            <Setter Property="FontSize" Value="12"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Border Background="{TemplateBinding Background}" CornerRadius="5" Margin="2,0">
                            <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center"/>
                        </Border>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter Property="Background" Value="#1F1F26"/>
                                <Setter Property="Foreground" Value="#FFFFFF"/>
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <Style x:Key="PrimaryBarBtn" TargetType="Button" BasedOn="{StaticResource BarBtn}">
            <Setter Property="Foreground" Value="{StaticResource AccentGold}"/>
            <Style.Triggers>
                <Trigger Property="IsMouseOver" Value="True">
                    <Setter Property="Background" Value="#2A2410"/>
                    <Setter Property="Foreground" Value="{StaticResource AccentGoldHover}"/>
                </Trigger>
            </Style.Triggers>
        </Style>
    </Window.Resources>

    <Grid>
        <Border CornerRadius="10" Background="{StaticResource BgDark}" BorderBrush="{StaticResource BorderColor}" BorderThickness="1">
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="32"/>
                    <RowDefinition Height="34"/>
                    <RowDefinition Height="*"/>
                </Grid.RowDefinitions>

                <Grid Grid.Row="0" Background="Transparent" MouseLeftButtonDown="TitleBar_MouseDown">
                    <TextBlock Text="Vault" FontSize="12" FontWeight="SemiBold"
                               Foreground="{StaticResource TextMuted}"
                               VerticalAlignment="Center" Margin="14,0,0,0"/>

                    <StackPanel Orientation="Horizontal" HorizontalAlignment="Right" VerticalAlignment="Top">
                        <Button Style="{StaticResource WinBtn}" Click="Minimize_Click">
                            <TextBlock Text="&#x2014;" FontSize="11"/>
                        </Button>
                        <Button Style="{StaticResource WinBtn}" Click="Maximize_Click">
                            <TextBlock Text="&#x25A1;" FontSize="11"/>
                        </Button>
                        <Button Style="{StaticResource CloseBtn}" Click="Close_Click">
                            <TextBlock Text="&#x2715;" FontSize="11"/>
                        </Button>
                    </StackPanel>
                </Grid>

                <Grid Grid.Row="1" Background="{StaticResource BgPanel}">
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="*"/>
                        <ColumnDefinition Width="Auto"/>
                    </Grid.ColumnDefinitions>

                    <StackPanel Grid.Column="0" Orientation="Horizontal" Margin="8,0,0,0">
                        <Button x:Name="AccountTabBtn" Style="{StaticResource TabBtn}"
                                Click="AccountTab_Click" Content="Account"/>
                        <Button x:Name="ScriptTabBtn" Style="{StaticResource TabBtnActive}"
                                Click="ScriptTab_Click" Content="Script"/>
                        <Button Style="{StaticResource TabBtn}" Content="+" FontSize="14" Padding="12,4" Click="NewTab_Click"/>
                    </StackPanel>

                    <TextBlock Grid.Column="1" Text="&#x1F50D;" FontSize="12"
                               Foreground="{StaticResource TextMuted}"
                               VerticalAlignment="Center" Margin="0,0,16,0"/>
                </Grid>

                <Grid Grid.Row="2">

                    <ScrollViewer x:Name="AccountPanel" Visibility="Collapsed"
                                  VerticalScrollBarVisibility="Auto" Padding="28">
                        <StackPanel>
                            <TextBlock Text="Account" FontSize="20" FontWeight="Bold"
                                       Foreground="{StaticResource AccentGold}" Margin="0,4,0,20"/>

                            <Border Background="{StaticResource BgPanel}" CornerRadius="8"
                                    BorderBrush="{StaticResource BorderColor}" BorderThickness="1"
                                    Padding="18" Margin="0,0,0,12">
                                <StackPanel Orientation="Horizontal">
                                    <Border Width="44" Height="44" CornerRadius="22"
                                            Background="{StaticResource AccentGold}" Margin="0,0,14,0">
                                        <TextBlock Text="V" Foreground="#0A0A0C" FontWeight="Bold"
                                                   FontSize="18" HorizontalAlignment="Center" VerticalAlignment="Center"/>
                                    </Border>
                                    <StackPanel VerticalAlignment="Center">
                                        <TextBlock x:Name="AccountNameText" Text="Not signed in"
                                                   FontSize="15" FontWeight="SemiBold"/>
                                        <TextBlock x:Name="AccountStatusText" Text="Sign in with Discord to continue"
                                                   FontSize="11" Foreground="{StaticResource TextMuted}" Margin="0,2,0,0"/>
                                    </StackPanel>
                                </StackPanel>
                            </Border>

                            <Border Background="{StaticResource BgPanel}" CornerRadius="8"
                                    BorderBrush="{StaticResource BorderColor}" BorderThickness="1"
                                    Padding="18" Margin="0,0,0,12">
                                <StackPanel>
                                    <Grid Margin="0,0,0,10">
                                        <TextBlock Text="Plan" Foreground="{StaticResource TextMuted}" FontSize="12"/>
                                        <TextBlock Text="Vault Premium" HorizontalAlignment="Right" FontSize="12" FontWeight="SemiBold"/>
                                    </Grid>
                                    <Separator Background="{StaticResource BorderColor}" Margin="0,0,0,10"/>
                                    <Grid>
                                        <TextBlock Text="Status" Foreground="{StaticResource TextMuted}" FontSize="12"/>
                                        <TextBlock Text="Whitelisted" HorizontalAlignment="Right" FontSize="12"
                                                   FontWeight="SemiBold" Foreground="{StaticResource AccentGold}"/>
                                    </Grid>
                                </StackPanel>
                            </Border>

                            <Button Content="Sign in with Discord" Padding="16,10"
                                    HorizontalAlignment="Left" FontSize="12" FontWeight="SemiBold"
                                    Background="{StaticResource AccentGold}" Foreground="#0A0A0C"
                                    BorderThickness="0" Cursor="Hand" Click="DiscordSignIn_Click">
                                <Button.Template>
                                    <ControlTemplate TargetType="Button">
                                        <Border Background="{TemplateBinding Background}" CornerRadius="6" Padding="{TemplateBinding Padding}">
                                            <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center"/>
                                        </Border>
                                    </ControlTemplate>
                                </Button.Template>
                            </Button>
                        </StackPanel>
                    </ScrollViewer>

                    <Grid x:Name="ScriptPanel">
                        <Grid.RowDefinitions>
                            <RowDefinition Height="*"/>
                            <RowDefinition Height="42"/>
                        </Grid.RowDefinitions>

                        <Border Grid.Row="0" Background="{StaticResource BgEditor}" BorderBrush="{StaticResource BorderColor}" BorderThickness="0,0,0,1">
                            <Grid>
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="44"/>
                                    <ColumnDefinition Width="*"/>
                                </Grid.ColumnDefinitions>

                                <Border Grid.Column="0" Background="{StaticResource BgEditor}">
                                    <ScrollViewer x:Name="LineNumberScroller" VerticalScrollBarVisibility="Hidden"
                                                  HorizontalScrollBarVisibility="Hidden">
                                        <TextBlock x:Name="LineNumbers" FontFamily="Consolas" FontSize="13"
                                                   Foreground="{StaticResource LineNumber}"
                                                   TextAlignment="Right" Padding="0,12,8,12"
                                                   Text="1"/>
                                    </ScrollViewer>
                                </Border>

                                <TextBox x:Name="ScriptBox" Grid.Column="1"
                                         Background="Transparent"
                                         Foreground="#E8E8E8"
                                         CaretBrush="{StaticResource AccentGold}"
                                         BorderThickness="0"
                                         Padding="10,12,12,12"
                                         FontFamily="Consolas"
                                         FontSize="13"
                                         AcceptsReturn="True"
                                         AcceptsTab="True"
                                         TextWrapping="NoWrap"
                                         VerticalScrollBarVisibility="Auto"
                                         HorizontalScrollBarVisibility="Auto"
                                         TextChanged="ScriptBox_TextChanged"
                                         ScrollViewer.ScrollChanged="ScriptBox_ScrollChanged"/>
                            </Grid>
                        </Border>

                        <Border Grid.Row="1" Background="{StaticResource BgPanel}" BorderBrush="{StaticResource BorderColor}" BorderThickness="0">
                            <Grid>
                                <StackPanel Orientation="Horizontal" Margin="10,0,0,0" VerticalAlignment="Center">
                                    <Button x:Name="AttachButton" Style="{StaticResource PrimaryBarBtn}"
                                            Click="AttachButton_Click">
                                        <StackPanel Orientation="Horizontal">
                                            <TextBlock Text="&#x1F517;" FontSize="11" Margin="0,0,6,0"/>
                                            <TextBlock Text="Attach"/>
                                        </StackPanel>
                                    </Button>
                                </StackPanel>

                                <StackPanel Orientation="Horizontal" HorizontalAlignment="Right"
                                            Margin="0,0,10,0" VerticalAlignment="Center">
                                    <Button Style="{StaticResource BarBtn}" Click="Clear_Click">
                                        <StackPanel Orientation="Horizontal">
                                            <TextBlock Text="&#x2327;" FontSize="11" Margin="0,0,6,0"/>
                                            <TextBlock Text="Clear"/>
                                        </StackPanel>
                                    </Button>
                                    <Button Style="{StaticResource BarBtn}" Click="Open_Click">
                                        <StackPanel Orientation="Horizontal">
                                            <TextBlock Text="&#x1F4C2;" FontSize="11" Margin="0,0,6,0"/>
                                            <TextBlock Text="Open"/>
                                        </StackPanel>
                                    </Button>
                                    <Button Style="{StaticResource BarBtn}" Click="Save_Click">
                                        <StackPanel Orientation="Horizontal">
                                            <TextBlock Text="&#x1F4BE;" FontSize="11" Margin="0,0,6,0"/>
                                            <TextBlock Text="Save"/>
                                        </StackPanel>
                                    </Button>
                                    <Button x:Name="ExecuteButton" Style="{StaticResource PrimaryBarBtn}"
                                            Click="ExecuteButton_Click" Margin="6,0,0,0">
                                        <StackPanel Orientation="Horizontal">
                                            <TextBlock Text="&#x25B6;" FontSize="10" Margin="0,0,6,0"/>
                                            <TextBlock Text="Execute"/>
                                        </StackPanel>
                                    </Button>
                                </StackPanel>

                                <TextBlock x:Name="StatusText" Text="Not Attached"
                                           Foreground="{StaticResource TextMuted}" FontSize="11"
                                           VerticalAlignment="Center" HorizontalAlignment="Center"/>
                            </Grid>
                        </Border>
                    </Grid>
                </Grid>
            </Grid>
        </Border>

        <Popup x:Name="AttachedPopup"
               PlacementTarget="{Binding RelativeSource={RelativeSource AncestorType=Window}}"
               Placement="Top" HorizontalOffset="0" VerticalOffset="60"
               AllowsTransparency="True" StaysOpen="False">
            <Border Background="{StaticResource BgCard}" BorderBrush="{StaticResource AccentGold}"
                    BorderThickness="1" CornerRadius="8" Padding="16,12" Margin="10">
                <StackPanel Orientation="Horizontal">
                    <TextBlock Text="&#x1F512;" FontSize="18" Foreground="{StaticResource AccentGold}"
                               Margin="0,0,10,0" VerticalAlignment="Center"/>
                    <StackPanel>
                        <TextBlock Text="Attached to Client" FontWeight="Bold"
                                   Foreground="{StaticResource AccentGold}"/>
                        <TextBlock Text="Vault is ready." FontSize="11"
                                   Foreground="{StaticResource TextMuted}"/>
                    </StackPanel>
                </StackPanel>
            </Border>
        </Popup>
    </Grid>
</Window>
