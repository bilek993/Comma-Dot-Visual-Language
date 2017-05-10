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
        private PropertiesManager _propertiesManager;

        public static bool IsAddConectionMode = false;
        public static Block FirstBlockForConnection;
        public static Block SecondBlockForConnection;

        public Block StartBlock { get; private set; }

        public BlockManager(Canvas canvasBlocks, PropertiesManager propertiesManager)
        {
            _blocks = new List<Block>();
            _canvasBlocks = canvasBlocks;
            _propertiesManager = propertiesManager;
        }

        public void CreateBasicBlocks()
        {
            _blocks.Add(new BeginBlock(_canvasBlocks, _propertiesManager));
            _blocks[_blocks.Count - 1].Command = "Start";
            _blocks[_blocks.Count - 1].SetPositon(_beginStartPositon.X, _beginStartPositon.Y);

            StartBlock = _blocks[_blocks.Count - 1];
        }

        public void CreateCommandBlock()
        {
            _blocks.Add(new CommandBlock(_canvasBlocks, _propertiesManager));
            _blocks[_blocks.Count - 1].Command = "Example(a)";
        }

        public void CreateInputBlock()
        {
            _blocks.Add(new InputBlock(_canvasBlocks, _propertiesManager));
            _blocks[_blocks.Count - 1].Command = "A";
        }

        public void CreateOutputBlock()
        {
            _blocks.Add(new OutputBlock(_canvasBlocks, _propertiesManager));
            _blocks[_blocks.Count - 1].Command = "A";
        }

        public void CreateIfBlock()
        {
            _blocks.Add(new IfBlock(_canvasBlocks, _propertiesManager));
            _blocks[_blocks.Count - 1].Command = "2==2";
        }

        public void CreateEndBlock()
        {
            _blocks.Add(new EndBlock(_canvasBlocks, _propertiesManager));
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
