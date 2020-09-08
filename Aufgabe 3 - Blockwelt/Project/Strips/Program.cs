using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strips
{
    class Program
    {
        static List<Rule> startWorld = new List<Rule>();
        /*
        {   //for testing
            new Rule(Rule.RType.ONTABLE,'A', ' '),
            new Rule(Rule.RType.ONTABLE,'B', ' '),
            new Rule(Rule.RType.ON,'D', 'A'),
            new Rule(Rule.RType.ON,'C', 'D'),
            new Rule(Rule.RType.CLEAR,'C', ' '),
            new Rule(Rule.RType.CLEAR,'B', ' '),
            new Rule(Rule.RType.EMPTY,' ', ' '),
        };*/
        static List<Rule> endWorld = new List<Rule>();
        /*
        {   //for testing
            new Rule(Rule.RType.ONTABLE,'B', ' '),
            new Rule(Rule.RType.ONTABLE,'C', ' '),
            new Rule(Rule.RType.ON,'D', 'B'),
            new Rule(Rule.RType.ON,'A', 'D'),
            new Rule(Rule.RType.CLEAR,'A', ' '),
            new Rule(Rule.RType.CLEAR,'C', ' '),
            new Rule(Rule.RType.EMPTY,' ', ' '),
        };*/

        static void Main(string[] args)
        {
            /*
             * Input Code
             */
            List<Rule> inputStartState = new List<Rule>();
            List<Rule> inputGoalState = new List<Rule>();

            List<string> startStateAsString = new List<string>();
            List<string> goalStateAsString = new List<string>();
            string tableSizeAsString, columnString;
            Console.Write("Enter size of table: ");
            tableSizeAsString = Console.ReadLine();

            int numberAsInt = Convert.ToInt32(tableSizeAsString);
            for (int i = 0; i < numberAsInt; i++)
            {
                Console.Write("Enter " + (i+1) + " column of start-state: ");
                columnString = Console.ReadLine();
                startStateAsString.Add(columnString);
            }
            inputStartState = Rule.StringListConvertToRuleList(startStateAsString);

            for (int i = 0; i < numberAsInt; i++)
            {
                Console.Write("Enter " + (i + 1) + " column of goal-state: ");
                columnString = Console.ReadLine();
                goalStateAsString.Add(columnString);
            }
            inputGoalState = Rule.StringListConvertToRuleList(goalStateAsString);

            startWorld = inputStartState;
            endWorld = inputGoalState;
            
            /*
             * here starts Logic 
             */
            Helper.PrintRulesAsCols("Start", startWorld);
            Solver solver = new Solver(startWorld, endWorld);
            List<Action> solution = solver.FindSolution();
            foreach (Action step in solution)
            {
                Helper.PrintRulesAsCols("Doing Action : " + step, startWorld);
                startWorld = solver.PerformAction(startWorld, step);                
            }
            Helper.PrintRulesAsCols("Target", endWorld);
            Console.ReadLine();
            
        }
    }
}
