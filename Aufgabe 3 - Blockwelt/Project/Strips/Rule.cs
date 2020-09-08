using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strips
{
    public class Rule
    {
        public enum RType
        {
            ONTABLE,
            ON,
            CLEAR,
            HOLDS,
            EMPTY
        }
        public RType type;
        public char paramA;
        public char paramB;
        public Rule(RType t, char a, char b)
        {
            type = t;
            paramA = a;
            paramB = b;
        }

        public static List<Rule> StringListConvertToRuleList(List<string> list)
        {
            List<Rule> result = new List<Rule>();

            //get on-table rules
            foreach (string s in list)
                if (s.Length > 0)
                    result.Add(new Rule(Rule.RType.ONTABLE, s[0], ' '));
            //get on rules
            foreach (string s in list)
                if (s.Length > 1)
                    for (int i = 0; i < s.Length-1; i++)
                        result.Add(new Rule(Rule.RType.ON, s[i + 1], s[i]));
            //get clear rules
            foreach (string s in list)
                if (s.Length > 0)
                    result.Add(new Rule(Rule.RType.CLEAR, s[s.Length - 1], ' '));

            result.Add(new Rule(Rule.RType.EMPTY, ' ', ' '));

            return result;
        }

        public static bool operator == (Rule a, Rule b)
        {
            return a.type == b.type && a.paramA == b.paramA && a.paramB == b.paramB;
        }
        public static bool operator !=(Rule a, Rule b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            switch (type)
            {
                case RType.ONTABLE:
                    return "Ontable(" + paramA + ")";
                case RType.ON:
                    return "On(" + paramA + "," + paramB + ")";
                case RType.CLEAR:
                    return "Clear(" + paramA + ")";
                case RType.HOLDS:
                    return "Holds(" + paramA + ")";
                case RType.EMPTY:
                    return "ArmEmpty";
            }
            throw new Exception("Unknown Rule Type");
        }
    }
}
