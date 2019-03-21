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
        public static string[] ArgumentsSplitter(string arguments)
        {
            return arguments.Split(new [] { ',' }, StringSplitOptions.None);
        }

        public static void IncrementValues(string[] arguments)
        {
            foreach (var t in arguments)
            {
                switch (Runner.Variables[t])
                {
                    case int i:
                        Runner.Variables[t] = i + 1;
                        break;
                    case float f:
                        Runner.Variables[t] = f + 1;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        public static void DecrementValues(string[] arguments)
        {
            foreach (var t in arguments)
            {
                switch (Runner.Variables[t])
                {
                    case int i:
                        Runner.Variables[t] = i - 1;
                        break;
                    case float f:
                        Runner.Variables[t] = f - 1;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        public static void Math_Pi(string returnedVariable)
        {
            switch (Runner.Variables[returnedVariable])
            {
                case int _:
                    Runner.Variables[returnedVariable] = (int)Math.PI;
                    break;
                case float _:
                    Runner.Variables[returnedVariable] = Math.PI;
                    break;
                default:
                    Runner.Variables[returnedVariable] = Math.PI.ToString(CultureInfo.InvariantCulture);
                    break;
            }
        }

        public static void Math_e(string returnedVariable)
        {
            switch (Runner.Variables[returnedVariable])
            {
                case int _:
                    Runner.Variables[returnedVariable] = (int)Math.E;
                    break;
                case float _:
                    Runner.Variables[returnedVariable] = Math.E;
                    break;
                default:
                    Runner.Variables[returnedVariable] = Math.E.ToString(CultureInfo.InvariantCulture);
                    break;
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
