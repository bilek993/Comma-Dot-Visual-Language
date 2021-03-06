﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Comma_Dot_Visual_Language.Blocks;

namespace Comma_Dot_Visual_Language.Helpers
{
    public class BlockManager
    {
        private static readonly Vector BeginStartPosition = new Vector(350, 100);

        private readonly List<Block> _blocks;
        private readonly Canvas _canvasBlocks;
        private readonly PropertiesManager _propertiesManager;

        public static bool IsAddConnectionMode = false;
        public static Block FirstBlockForConnection;
        public static Block SecondBlockForConnection;

        public Block StartBlock { get; private set; }

        public BlockManager(Canvas canvasBlocks, PropertiesManager propertiesManager)
        {
            _blocks = new List<Block>();
            _canvasBlocks = canvasBlocks;
            _propertiesManager = propertiesManager;
        }

        public List<Block> GetBlocks()
        {
            return _blocks;
        }

        public void AddBlock(Block block)
        {
            _blocks.Add(block);
        }

        public Block CreateBasicBlocks()
        {
            _blocks.Add(new BeginBlock(_canvasBlocks, _propertiesManager));
            _blocks[_blocks.Count - 1].Command = "Start";
            _blocks[_blocks.Count - 1].SetPosition(BeginStartPosition.X, BeginStartPosition.Y);

            StartBlock = _blocks[_blocks.Count - 1];

            return _blocks[_blocks.Count - 1];
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
            IsAddConnectionMode = false;
        }

        public void RemoveConnection(Block block, int connectionId)
        {
            block.RemoveLine(connectionId);
            _propertiesManager.Update();
        }

        public void RemoveBlock(Block block)
        {
            if (block is BeginBlock)
            {
                MessageBox.Show("Begin block cannot be removed, because new instance of this block cannot be added from menu.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RemoveConnection(block, 0);
            RemoveConnection(block, 1);

            foreach (var previousBlock in block.PreviousBlocks)
            {
                var index = -1;
                if (previousBlock.NextBlockPrimary == block)
                {
                    index = 0;
                }
                else if (previousBlock.NextBlockOptional == block)
                {
                    index = 1;
                }
                RemoveConnection(previousBlock, index);
            }

            block.RemoveFromCanvas();
            _blocks.Remove(block);

            _propertiesManager.SelectedBlock = StartBlock;

            _propertiesManager.Update();
        }

        public Block FindBlockById(int id)
        {
            return _blocks.Find(block => block.Id == id);
        }
    }
}
