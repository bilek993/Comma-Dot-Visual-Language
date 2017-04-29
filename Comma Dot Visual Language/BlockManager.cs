using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Comma_Dot_Visual_Language
{
    public class BlockManager
    {
        private static Vector _beginStartPositon = new Vector(350, 100);
        private readonly List<Block> _blocks;
        private readonly Canvas _canvasBlocks;

        public static bool IsAddConectionMode = false;
        public static Block FirstBlockForConnection;
        public static Block SecondBlockForConnection;

        public BlockManager(Canvas canvasBlocks)
        {
            _blocks = new List<Block>();
            _canvasBlocks = canvasBlocks;
        }

        public void CreateBasicBlocks()
        {
            _blocks.Add(new BeginBlock(_canvasBlocks));
            _blocks[_blocks.Count - 1].Command = "Start";
            _blocks[_blocks.Count - 1].SetPositon(_beginStartPositon.X, _beginStartPositon.Y);
        }

        public void CreateInputBlock()
        {
            _blocks.Add(new InputBlock(_canvasBlocks));
            _blocks[_blocks.Count - 1].Command = "Input: A";
        }

        public void CreateOutputBlock()
        {
            _blocks.Add(new OutputBlock(_canvasBlocks));
            _blocks[_blocks.Count - 1].Command = "Output: A";
        }

        public void CreateEndBlock()
        {
            _blocks.Add(new EndBlock(_canvasBlocks));
            _blocks[_blocks.Count - 1].Command = "End";
        }

        public static void AddConnection()
        {
            FirstBlockForConnection.AddConnection(SecondBlockForConnection);

            FirstBlockForConnection = null;
            SecondBlockForConnection = null;
            IsAddConectionMode = false;
        }
    }
}
