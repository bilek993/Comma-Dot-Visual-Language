using System;
using System.Collections.Generic;
using System.Globalization;
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
            foreach (string t in arguments)
            {
                if (Runner.Variables[t].GetType() == typeof(int))
                {
                    Runner.Variables[t] = (int)Runner.Variables[t] + 1;
                }
                else if (Runner.Variables[t].GetType() == typeof(float))
                {
                    Runner.Variables[t] = (float)Runner.Variables[t] + 1;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public static void DecrementValues(string[] arguments)
        {
            foreach (string t in arguments)
            {
                if (Runner.Variables[t].GetType() == typeof(int))
                {
                    Runner.Variables[t] = (int)Runner.Variables[t] - 1;
                }
                else if (Runner.Variables[t].GetType() == typeof(float))
                {
                    Runner.Variables[t] = (float)Runner.Variables[t] - 1;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public static void Math_Pi(string returnedVariable)
        {
            if (Runner.Variables[returnedVariable].GetType() == typeof(int))
            {
                Runner.Variables[returnedVariable] = (int)Math.PI;
            }
            else if (Runner.Variables[returnedVariable].GetType() == typeof(float))
            {
                Runner.Variables[returnedVariable] = Math.PI;
            }
            else
            {
                Runner.Variables[returnedVariable] = Math.PI.ToString(CultureInfo.InvariantCulture);
            }
        }

        public static void Math_e(string returnedVariable)
        {
            if (Runner.Variables[returnedVariable].GetType() == typeof(int))
            {
                Runner.Variables[returnedVariable] = (int)Math.E;
            }
            else if (Runner.Variables[returnedVariable].GetType() == typeof(float))
            {
                Runner.Variables[returnedVariable] = Math.E;
            }
            else
            {
                Runner.Variables[returnedVariable] = Math.E.ToString(CultureInfo.InvariantCulture);
            }
        }

        public static void Modulo(string returnedVariable, string[] arguments)
        {
            if (arguments.Length != 2 || Runner.Variables[returnedVariable].GetType() != typeof(int))
                throw new ArgumentException();

            if (Runner.Variables[arguments[0]].GetType() != typeof(int) || Runner.Variables[arguments[1]].GetType() != typeof(int))
                throw new ArgumentException();

            Runner.Variables[returnedVariable] = (int)Runner.Variables[arguments[0]] % (int)Runner.Variables[arguments[1]];
        }
    }
}
