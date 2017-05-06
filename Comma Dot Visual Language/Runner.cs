using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comma_Dot_Visual_Language
{
    class Runner
    {
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
            while (_currentBlock != null)
            {
                _currentBlock = _currentBlock.Run();
            }
        }
    }
}
