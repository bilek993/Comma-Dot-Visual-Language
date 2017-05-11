using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Comma_Dot_Visual_Language
{
    class Runner
    {
        public static Dictionary<string, object> Variables = new Dictionary<string, object>();

        private Block _currentBlock;

        public Runner()
        {

        }

        public void SetStartBlock(Block block)
        {
            _currentBlock = block;
        }

        public void Run()
        {
            Variables.Clear();

            while (_currentBlock != null)
            {
                try
                {
                    _currentBlock = _currentBlock.Run();
                }
                catch (KeyNotFoundException e)
                {
                    MessageBox.Show("Invalid variable name.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show("Invalid argument in command.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                catch(FormatException e)
                {
                    MessageBox.Show("Invalid variable type.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }
        }
    }
}
