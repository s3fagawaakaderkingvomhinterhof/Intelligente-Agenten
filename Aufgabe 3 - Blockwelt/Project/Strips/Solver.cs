using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strips
{
    public class Solver
    {
        private List<Rule> startWorld;
        private List<Rule> endWorld;

        public Solver(List<Rule> start, List<Rule> end)
        {
            startWorld = new List<Rule>(start);
            endWorld = new List<Rule>(end);
        }

        public List<Action> FindSolution()  //eigentliche Suche
        {
            List<ProblemState> open = new List<ProblemState>();                                         //Liste von offenen Problemen
            open.Add(new ProblemState(null, null, startWorld, 0, CalcDistance(startWorld, endWorld)));  //hinzufügen eines Element(Problem,Aktion,aktueller Zustand, Schritte,Kosten) 
            bool foundSolution = false;
            ProblemState solutionState = null;              //noch keine Lösung gefunden 
            while (open.Count != 0 && !foundSolution)       //solange Unterschied zw. Ziel und aktuellem Zustand und keine Lösung 
            {
                ProblemState best = open[0];                //nimm erstes Element von offenen Zielen(Prädikate des Zielzustandes)
                open.RemoveAt(0);                           //entferne dieses dann
                List<Action> options = GetActions(best.world);  //gib mögliche Aktionen zum erreichen dieses Ziel an
                foreach (Action op in options)
                {
                    List<Rule> newState = PerformAction(best.world, op); //wende Aktion auf offene Regel an und berechne den neuen Zustand
                    ProblemState successor = new ProblemState(best, op, newState, best.steps + 1, CalcDistance(newState, endWorld));//rekursiver Aufruf
                    if (StateEquals(newState, endWorld))    //Vergleich ob neuer Zustand Ziel ist
                    {
                        solutionState = successor;
                        foundSolution = true;
                        break;
                    }
                    open.Add(successor);                    //füge Nachfolger
                }
                open = Sort(open);                          //sortiere Liste nach Kosten, deswegen iist beste Option immer vorn
            }
            return MakeSolution(solutionState);             //drehe Liste um, damit Weg vom Start zum Ziel zeigt
        }

        public List<ProblemState> Sort(List<ProblemState> list) //sortieren der Liste nach Kostenabschätzung
        {
            bool run = true;
            while (run)
            {
                run = false;
                for (int i = 0; i < list.Count - 1; i++)
                    if (list[i].getCost() > list[i + 1].getCost())
                    {
                        ProblemState temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                        run = true;
                    }
            }
            return list;
        }

        public List<Rule> PerformAction(List<Rule> world, Action a) //wendet Aktion auf aktuelle Welt(en) an
        {
            List<Rule> addList;
            List<Rule> delList;
            a.GetChanges(out addList, out delList); //befüllen der aktuellen add und del Listen
            List<Rule> result = new List<Rule>();
            result.AddRange(world);                 //Zustand nach Aktion = (aktuell + add) - del
            result.AddRange(addList);
            for (int i = 0; i < delList.Count; i++)
                for (int j = 0; j < result.Count; j++)
                    if (result[j] == delList[i])
                    {
                        result.RemoveAt(j);
                        break;
                    }
            return result;
        }

        public List<Action> GetActions(List<Rule> world)    //finden der möglichen Aktionen abhängig von
        {                                                   //aktuellem Status der Welt
            List<Action> result = new List<Action>();
            char holds = ' ';                               
            foreach (Rule r in world)                       //suche nach allen Regeln für Arm
                if (r.type == Rule.RType.EMPTY)             //wenn leer dann hält er nicht
                    break;
                else if (r.type == Rule.RType.HOLDS)        //wenn holds zutrifft
                {                                           //dann speichere Parameter
                    holds = r.paramA;
                    break;
                }
            if (holds == ' ')                               //wenn holds leer nur 2 Optionen mögl.(unstack(nimm von Stapel) und pickup(nimm von Tisch))
            {
                //unstack 
                foreach (Rule r in world)
                    if (r.type == Rule.RType.ON)
                    {
                        char top = r.paramA;
                        bool clear = false;
                        foreach (Rule r2 in world)
                            if (r2.type == Rule.RType.CLEAR && r2.paramA == top)    //Suche nach oberstem Block und dieser muss zusätzl frei sein
                            {
                                clear = true;
                                break;
                            }
                        if (clear)
                            result.Add(new Action(Action.AType.UNSTACK, top, r.paramB));    //wenn oberster Block frei dann füge mögliche Aktion der Liste hinzu
                    }
                //pickup
                foreach (Rule r in world)                   //suche in allen Regeln
                    if (r.type == Rule.RType.ONTABLE)       //die ONTABLE sind
                    {
                        bool clear = false;
                        foreach (Rule r2 in world)
                            if (r2.type == Rule.RType.CLEAR && r2.paramA == r.paramA)       //alle ONTABLE Regeln müssen auch CLEAR sein
                            {
                                clear = true;
                                break;
                            }
                        if (clear)
                            result.Add(new Action(Action.AType.PICKUP, r.paramA, ' '));     //wenn ONTABLE Regel und CLEAR Regel ok dann füge Aktion hinzu
                    }
            }
            else
            {   //wenn Arm nicht frei, also Block wird gehalten(stack, putdown)
                //stack
                foreach (Rule r in world)                   //suche in allen regeln
                    if (r.type == Rule.RType.CLEAR)         //nach clear
                        result.Add(new Action(Action.AType.STACK, holds, r.paramA));//wenn ja füge unstack hinzu
                //putdown
                result.Add(new Action(Action.AType.PUTDOWN, holds, ' '));
            }
            return result;
        }
        public List<Action> MakeSolution(ProblemState end)//dreht einfach Liste um
        {
            List<Action> result = new List<Action>();
            ProblemState iter = end;
            while (iter != null && iter.parent != null)
            {
                result.Add(iter.actionTaken);
                iter = iter.parent;
            }
            result.Reverse();
            return result;
        }

        public int CalcDistance(List<Rule> a, List<Rule> b) //berechnet nötige Veränderungen von a bis Ziel(b)
        {
            int result = 0;
            foreach (Rule r in a)
            {
                foreach(Rule r2 in b)
                    if (r2 == r)
                    {
                        result++;
                        break;
                    }
            }
            return a.Count - result;
        }

        public bool StateEquals(List<Rule> a, List<Rule> b) //vergleicht Liste von Regeln
        {
            if (a.Count != b.Count)
                return false;
            foreach (Rule r in a)
            {
                bool found = false;
                foreach (Rule r2 in b)
                    if (r2 == r)
                    {
                        found = true;
                        break;
                    }
                if (!found)
                    return false;
            }
            return true;
        }
    }
}
