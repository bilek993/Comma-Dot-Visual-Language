﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Comma_Dot_Visual_Language
{
    class InputBlock : Block
    {
        public InputBlock(Canvas canvas, PropertiesManager propertiesManager) : base(canvas, 1, propertiesManager)
        {
            Shape = new Polygon()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.White),
                Points = {new Point(0,0), new Point(20,30), new Point(100,30), new Point(80,0)}
            };

            AddShapeToCanvas();
            AddTextBlockToCanvas();
            OnMouseLeftButtonDown(null,null);
        }

        public override Block Run()
        {
            string variableValue;

            do
            {
                variableValue = Microsoft.VisualBasic.Interaction.InputBox("Enter variable value:", "Input");
            } while (String.IsNullOrWhiteSpace(variableValue));

            Runner.Variables[Command] = variableValue;

            return NextBlockPrimary;
        }
    }
}
