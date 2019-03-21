using System;
using System.Collections.Generic;
using System.Windows;
using Comma_Dot_Visual_Language.Blocks;

namespace Comma_Dot_Visual_Language.Helpers
{
    class Runner
    {
        public static readonly Dictionary<string, object> Variables = new Dictionary<string, object>();

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
                catch (KeyNotFoundException)
                {
                    MessageBox.Show("Invalid variable name.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Invalid argument in command.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                catch(FormatException)
                {
                    MessageBox.Show("Invalid variable type.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Invalid command.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }
        }
    }
}
