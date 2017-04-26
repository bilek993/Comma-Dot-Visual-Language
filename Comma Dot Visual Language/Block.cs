using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Comma_Dot_Visual_Language
{
    abstract class Block
    {
        public int Id { get; private set; }
        public string Command { get; set; }
        public static int BlocksCounter = 0;
        public Block NextBlockPrimary { get; set; }
        public Block NextBlockOptional { get; set; }

        protected Canvas CanvasBlocks;
        protected Polygon _shape;
        protected bool _isPressed;

        protected Block(Canvas canvas)
        {
            Id = BlocksCounter++;
            Command = "";
            CanvasBlocks = canvas;
        }

        public abstract string Run();
    }
}
