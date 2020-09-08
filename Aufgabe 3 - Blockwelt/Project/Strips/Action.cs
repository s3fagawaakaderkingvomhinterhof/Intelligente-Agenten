using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strips
{
    public class Action
    {
        public enum AType
        {
            STACK,
            UNSTACK,
            PICKUP,
            PUTDOWN
        }
        public AType type;
        public char paramA;
        public char paramB;

        public Action(AType t, char a, char b)
        {
            type = t;
            paramA = a;
            paramB = b;
        }

        public void GetChanges(out List<Rule> addList, out List<Rule> deleteList)
        {
            addList = new List<Rule>();
            deleteList = new List<Rule>();
            switch (type)
            {
                case AType.STACK:
                    addList.AddRange(new List<Rule>()
                    {
                        new Rule(Rule.RType.ON, paramA,paramB),
                        new Rule(Rule.RType.CLEAR, paramA,' '),                        
                        new Rule(Rule.RType.EMPTY, ' ',' ')
                    });
                    deleteList.AddRange(new List<Rule>()
                    {
                        new Rule(Rule.RType.HOLDS, paramA,' '), 
                        new Rule(Rule.RType.CLEAR, paramB,' '), 
                    });
                    break;
                case AType.UNSTACK:
                    addList.AddRange(new List<Rule>()
                    {
                        new Rule(Rule.RType.HOLDS, paramA,' '),
                        new Rule(Rule.RType.CLEAR, paramB,' '),
                    });
                    deleteList.AddRange(new List<Rule>()
                    { 
                        new Rule(Rule.RType.ON, paramA, paramB),
                        new Rule(Rule.RType.CLEAR, paramA,' '),
                        new Rule(Rule.RType.EMPTY, ' ',' '),  
                    });
                    break;
                case AType.PICKUP:
                    addList.AddRange(new List<Rule>()
                    {
                        new Rule(Rule.RType.HOLDS, paramA,' '),
                    });
                    deleteList.AddRange(new List<Rule>()
                    { 
                        new Rule(Rule.RType.ONTABLE, paramA, ' '),
                        new Rule(Rule.RType.CLEAR, paramA,' '),
                        new Rule(Rule.RType.EMPTY, ' ',' '),  
                    });
                    break;
                case AType.PUTDOWN:
                    addList.AddRange(new List<Rule>()
                    { 
                        new Rule(Rule.RType.ONTABLE, paramA, ' '),
                        new Rule(Rule.RType.CLEAR, paramA,' '),
                        new Rule(Rule.RType.EMPTY, ' ',' '),  
                    });
                    deleteList.AddRange(new List<Rule>()
                    {
                        new Rule(Rule.RType.HOLDS, paramA,' '),
                    });
                    break;
            }
        }


        public override string ToString()
        {
            switch (type)
            {
                case AType.STACK:
                    return "Stack(" + paramA + "," + paramB + ")";
                case AType.UNSTACK:
                    return "Unstack(" + paramA + "," + paramB + ")";
                case AType.PICKUP:
                    return "Pickup(" + paramA + ")";
                case AType.PUTDOWN:
                    return "Putdown(" + paramA + ")";
            }
            throw new Exception("Unknown Action Type");
        }

        public static bool operator ==(Action a, Action b)
        {
            return a.type == b.type && a.paramA == b.paramA && a.paramB == b.paramB;
        }

        public static bool operator !=(Action a, Action b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
