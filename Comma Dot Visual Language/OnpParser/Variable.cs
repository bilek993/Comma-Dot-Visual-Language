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
        private readonly string _variableName;

        public Variable(string variableName)
        {
            _variableName = variableName;
        }

        public override float CalculateValue()
        {
            switch (Runner.Variables[_variableName])
            {
                case int i:
                    Value = i;
                    break;
                case float f:
                    Value = f;
                    break;
            }

            return Value;
        }
    }
}
