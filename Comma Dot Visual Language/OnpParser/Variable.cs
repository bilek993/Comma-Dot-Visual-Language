using Comma_Dot_Visual_Language.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comma_Dot_Visual_Language.OnpParser
{
    public class Variable : ExpressionNode
    {
        private string _variableName;

        public Variable(string variableName)
        {
            _variableName = variableName;
        }

        public override float calculateValue()
        {
            if (Runner.Variables[_variableName].GetType() == typeof(int))
            {
                Value = (int)Runner.Variables[_variableName];
            }
            else if (Runner.Variables[_variableName].GetType() == typeof(float))
            {
                Value = (float)Runner.Variables[_variableName];
            }

            return Value;
        }
    }
}
