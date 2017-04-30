﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Comma_Dot_Visual_Language
{
    class CommandBlock : Block
    {
        public CommandBlock(Canvas canvas, PropertiesManager propertiesManager) : base(canvas, 1, propertiesManager)
        {
            Shape = new Rectangle()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.White),
                Width = 100,
                Height = 30
            };

            AddShapeToCanvas();
            AddTextBlockToCanvas();
            OnMouseLeftButtonDown(null, null);
        }

        public override Block Run()
        {
            throw new NotImplementedException();
        }
    }
}
