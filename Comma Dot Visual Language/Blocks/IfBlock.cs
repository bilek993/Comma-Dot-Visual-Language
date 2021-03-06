﻿using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Comma_Dot_Visual_Language.Helpers;

namespace Comma_Dot_Visual_Language.Blocks
{
    class IfBlock : Block
    {
        public IfBlock(Canvas canvas, PropertiesManager propertiesManager) : base(canvas, 2, propertiesManager)
        {
            Shape = ImageGenerator.GenerateBlockImage("if_block");

            AddShapeToCanvas();
            AddTextBlockToCanvas("if (", ")");
            OnMouseLeftButtonDown(null, null);
        }

        public IfBlock(Canvas canvas, PropertiesManager propertiesManager, int id) : base(canvas, 2, propertiesManager)
        {
            Id = id;
            _blocksCounter = Id + 1;

            Shape = ImageGenerator.GenerateBlockImage("if_block");

            AddShapeToCanvas();
            AddTextBlockToCanvas("if (", ")");
        }

        protected override void SetConnectionsPositions(double shapeLeft, double shapeTop)
        {
            ConnectionInputX = shapeLeft + width / 2;
            ConnectionInputY = shapeTop + 4f;

            ConnectionOutput1X = shapeLeft + 10.5f;
            ConnectionOutput1Y = shapeTop + height / 2 + 8.5f;

            ConnectionOutput2X = shapeLeft + width - 10.5f;
            ConnectionOutput2Y = shapeTop + height / 2 + 8.5f;
        }

        public override Block Run()
        {
            var regex = new Regex(@"^((([a-zA-Z][a-zA-Z0-9]*)|([0-9,]+))((==)|(>=)|(<=)|(>)|(<)|(!=))(([a-zA-Z][a-zA-Z0-9]*)|([0-9,]+)))$");
            var commandOptimized = Regex.Replace(Command, @"\s+", "");

            if (!regex.IsMatch(commandOptimized))
                throw new ArgumentException();

            var match = regex.Match(commandOptimized);

            var variable1 = match.Groups[3].Success ? Runner.Variables[match.Groups[3].Value] : ParseVariableValue(match.Groups[4].Value);
            var variable2 = match.Groups[13].Success ? Runner.Variables[match.Groups[13].Value] : ParseVariableValue(match.Groups[14].Value);

            return CalculateResult(match, variable1, variable2) ? NextBlockPrimary : NextBlockOptional;
        }

        private static object ParseVariableValue(string value)
        {
            if (int.TryParse(value, out var intResult))
            {
                return intResult;
            }

            if (float.TryParse(value, out var floatResult))
            {
                return floatResult;
            }

            return null;
        }
        
        private static bool CalculateResult(Match match, object variable1, object variable2)
        {
            var result = true;
            if (match.Groups[6].Success)
            {
                if (variable1 is int && variable2 is int)
                    result = (int)variable1 == (int)variable2;

                else if(variable1 is int && variable2 is float)
                    result = (int)variable1 == (float)variable2;

                else if (variable1 is float && variable2 is int)
                    result = (float)variable1 == (int)variable2;

                else if (variable1 is float && variable2 is float)
                    result = (float)variable1 == (float)variable2;
            }
            else if (match.Groups[7].Success)
            {
                if (variable1 is int && variable2 is int)
                    result = (int)variable1 >= (int)variable2;

                else if(variable1 is int && variable2 is float)
                    result = (int)variable1 >= (float)variable2;

                else if (variable1 is float && variable2 is int)
                    result = (float)variable1 >= (int)variable2;

                else if (variable1 is float && variable2 is float)
                    result = (float)variable1 >= (float)variable2;
            }
            else if (match.Groups[8].Success)
            {
                if (variable1 is int && variable2 is int)
                    result = (int)variable1 <= (int)variable2;

                else if (variable1 is int && variable2 is float)
                    result = (int)variable1 <= (float)variable2;

                else if (variable1 is float && variable2 is int)
                    result = (float)variable1 <= (int)variable2;

                else if (variable1 is float && variable2 is float)
                    result = (float)variable1 <= (float)variable2;
            }
            else if (match.Groups[9].Success)
            {
                if (variable1 is int && variable2 is int)
                    result = (int)variable1 > (int)variable2;

                else if (variable1 is int && variable2 is float)
                    result = (int)variable1 > (float)variable2;

                else if (variable1 is float && variable2 is int)
                    result = (float)variable1 > (int)variable2;

                else if (variable1 is float && variable2 is float)
                    result = (float)variable1 > (float)variable2;
            }
            else if (match.Groups[10].Success)
            {
                if (variable1 is int && variable2 is int)
                    result = (int)variable1 < (int)variable2;

                else if (variable1 is int && variable2 is float)
                    result = (int)variable1 < (float)variable2;

                else if (variable1 is float && variable2 is int)
                    result = (float)variable1 < (int)variable2;

                else if (variable1 is float && variable2 is float)
                    result = (float)variable1 < (float)variable2;
            }
            else if (match.Groups[11].Success)
            {
                if (variable1 is int && variable2 is int)
                    result = (int)variable1 != (int)variable2;

                else if (variable1 is int && variable2 is float)
                    result = (int)variable1 != (float)variable2;

                else if (variable1 is float && variable2 is int)
                    result = (float)variable1 != (int)variable2;

                else if (variable1 is float && variable2 is float)
                    result = (float)variable1 != (float)variable2;
            }

            return result;
        }
    }
}
