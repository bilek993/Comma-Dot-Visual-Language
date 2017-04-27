using System;
using System.Collections.Generic;
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

namespace Comma_Dot_Visual_Language
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Block> _blocks;

        public MainWindow()
        {
            InitializeComponent();

            _blocks = new List<Block>();
            CreateBasicBlocks();
        }

        private void CreateBasicBlocks()
        {
            _blocks.Add(new BeginBlock(CanvasBlocks));
            _blocks[_blocks.Count - 1].Command = "Start";
            _blocks[_blocks.Count - 1].SetPositon(300,10); // TODO: Fix magic numbers and position not setting correctly
        }

        private void MenuAdddInputBlockClick(object sender, RoutedEventArgs e)
        {
            _blocks.Add(new InputBlock(CanvasBlocks));
            _blocks[_blocks.Count - 1].Command = "Input: A";
        }
    }
}
