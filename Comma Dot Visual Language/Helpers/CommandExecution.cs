using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comma_Dot_Visual_Language.Helpers
{
    class CommandExecution
    {
        public static string[] ArgumentsSpliter(string arguments)
        {
            return arguments.Split(new char[] { ',' }, StringSplitOptions.None);
        }

        public static void IncrementValues(string[] arguments)
        {
            for (int i = 0; i < arguments.Length; ++i)
            {
                if (Runner.Variables[arguments[i]].GetType() == typeof(int))
                {
                    Runner.Variables[arguments[i]] = (int)Runner.Variables[arguments[i]] + 1;
                }
                else if (Runner.Variables[arguments[i]].GetType() == typeof(float))
                {
                    Runner.Variables[arguments[i]] = (float)Runner.Variables[arguments[i]] + 1;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public static void DecrementValues(string[] arguments)
        {
            for (int i = 0; i < arguments.Length; ++i)
            {
                if (Runner.Variables[arguments[i]].GetType() == typeof(int))
                {
                    Runner.Variables[arguments[i]] = (int)Runner.Variables[arguments[i]] - 1;
                }
                else if (Runner.Variables[arguments[i]].GetType() == typeof(float))
                {
                    Runner.Variables[arguments[i]] = (float)Runner.Variables[arguments[i]] - 1;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
    }
}
