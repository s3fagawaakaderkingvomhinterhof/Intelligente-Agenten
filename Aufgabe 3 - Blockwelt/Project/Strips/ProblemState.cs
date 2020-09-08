using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strips
{
    public class ProblemState
    {
        public ProblemState parent;
        public Action actionTaken;
        public List<Rule> world;
        public int steps;
        public int distance;

        public ProblemState(ProblemState p, Action a, List<Rule> w, int s, int d)
        {
            parent = p;
            actionTaken = a;
            world = w;
            steps = s;
            distance = d;
        }

        public int getCost() //Gesamtkosten berechnen sich nach Gesamtschritten + der aktuellen Distanz
        {
            return steps + distance;
        }
    }


}
