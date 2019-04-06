using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comma_Dot_Visual_Language.Helpers;

namespace Comma_Dot_Visual_Language.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BlockManager _blockManager;
        private readonly PropertiesManager _propertiesManager;
        private readonly Runner _runner;

        public MainWindow()
        {
            InitializeComponent();

            _propertiesManager = new PropertiesManager(LabelId, LabelOutputBlockPrimary, 
                LabelOutputBlockOptional, TextBoxCommand, VariableTypeComboBox, DockPanelVariableType, 
                PanelOptionalOutput, PanelPrimaryOutput);

            _blockManager = new BlockManager(CanvasBlocks, _propertiesManager);
            _propertiesManager.SelectedBlock = _blockManager.CreateBasicBlocks();
            _propertiesManager.Update();

            _runner = new Runner();
        }

        private void MenuCommandBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateCommandBlock();
        }

        private void MenuAddInputBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateInputBlock();
        }

        private void MenuAddOutputBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateOutputBlock();
        }

        private void MenuAddIfBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateIfBlock();
        }

        private void MenuAddEndBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateEndBlock();
        }

        private void MenuAddConnectionClick(object sender, RoutedEventArgs e)
        {
            BlockManager.IsAddConnectionMode = true;
            Mouse.OverrideCursor = Cursors.Pen;
        }

        private void TextBoxCommandTextChanged(object sender, TextChangedEventArgs e)
        {
            _propertiesManager.CommandChanged(TextBoxCommand.Text);
        }

        private void ButtonRemoveConnectionPrimaryClick(object sender, RoutedEventArgs e)
        {
            _blockManager.RemoveConnection(_propertiesManager.SelectedBlock, 0);
        }

        private void ButtonRemoveConnectionOptionalClick(object sender, RoutedEventArgs e)
        {
            _blockManager.RemoveConnection(_propertiesManager.SelectedBlock, 1);
        }

        private void MenuRunClick(object sender, RoutedEventArgs e)
        {
            _runner.SetStartBlock(_blockManager.StartBlock);

            var thread = new Thread(_runner.Run);
            thread.Start();
        }

        private void VariableTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _propertiesManager?.VariableTypeUpdate(VariableTypeComboBox.SelectedIndex);
        }

        private void MenuShowCommandsListClick(object sender, RoutedEventArgs e)
        {
            new CommandsListWindow().Show();
        }

        private void MenuCloseAppClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtnRemoveBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.RemoveBlock(_propertiesManager.SelectedBlock);
        }
    }
}
