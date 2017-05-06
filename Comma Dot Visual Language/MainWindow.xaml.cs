using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Comma_Dot_Visual_Language
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

            _propertiesManager = new PropertiesManager(LabelId, LabelOutputBlockPrimary, LabelOutputBlockOptional, TextBoxCommand);

            _blockManager = new BlockManager(CanvasBlocks, _propertiesManager);
            _blockManager.CreateBasicBlocks();

            _runner = new Runner();
        }

        private void MenuCommandBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateCommandBlock();
        }

        private void MenuAdddInputBlockClick(object sender, RoutedEventArgs e)
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
            BlockManager.IsAddConectionMode = true;
            Mouse.OverrideCursor = Cursors.Pen;
        }

        private void TextBoxCommandTextChanged(object sender, TextChangedEventArgs e)
        {
            _propertiesManager.CommandChanged(TextBoxCommand.Text);
        }

        private void MenuRunClick(object sender, RoutedEventArgs e)
        {
            _runner.SetStartBlock(_blockManager.StartBlock);

            Thread thread = new Thread(_runner.Run);
            thread.Start();
        }
    }
}
