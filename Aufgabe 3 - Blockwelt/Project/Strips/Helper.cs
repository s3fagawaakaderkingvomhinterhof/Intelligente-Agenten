using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strips
{
    public static class Helper
    {
        public static void PrintRulesAsCols(string name, List<Rule> rules)
        {
            List<string> list = new List<string>();
            foreach (Rule r in rules)
                if (r.type == Rule.RType.ONTABLE)
                    list.Add("" + r.paramA);
            List<Rule> left = new List<Rule>();
            foreach (Rule r in rules)
                if (r.type == Rule.RType.ON)
                    left.Add(r);
            while (left.Count > 0)
            {
                for (int i = 0; i < left.Count; i++)
                {
                    Rule r = left[i];
                    for (int j = 0; j < list.Count; j++)
                    {
                        string col = list[j];
                        char lastChar = col[col.Length - 1];
                        if (lastChar == r.paramB)
                        {
                            col += r.paramA;
                            list[j] = col;
                            left.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                }
            }
            Console.WriteLine("Printing Columns for : " + name);
            foreach (string col in list)
                Console.WriteLine(" |" + col);
            Console.WriteLine("Printing Rules");
            foreach (Rule r in rules)
                Console.WriteLine(r);
            Console.WriteLine();
        }
    }
}
