using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comma_Dot_Visual_Language.OnpParser
{
    public class ExpressionNode
    {
        protected float Value;

        public ExpressionNode(float value = 0)
        {
            Value = value;
        }

        public virtual float calculateValue()
        {
            return Value;
        }
    }
}
