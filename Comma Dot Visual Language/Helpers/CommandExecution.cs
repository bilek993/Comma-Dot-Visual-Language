﻿using System;
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
    }
}
