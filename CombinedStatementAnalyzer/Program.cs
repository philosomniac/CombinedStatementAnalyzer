using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CombinedStatementAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            QUERY FOR RETREIVING INPUT DATA

            SELECT j.UploadID, j.jobID, j.CreatedDate, cl.LOB, cl.Nickname, p.CustomerAccountID FROM tblJobs j
            JOIN tblPackets p ON j.JobID = p.JobID
            JOIN tblCustomerLOBs cl ON j.CustomerLOBID = cl.CustomerLOBID
            WHERE j.CustomerID = (SELECT CustomerID FROM tblCustomers WHERE Account = 'upm001')
            AND j.CreatedDate Between '2016-06-01' AND '2016-09-01'
            AND j.Type = 'Data'
            AND j.Status = 'Completed'

            */

            List< Statement > statementData = new List<Statement>();

            StreamReader myReader = File.OpenText("inputdata.csv");
            myReader.ReadLine();
            while (!myReader.EndOfStream)
            {
                string[] currentLine = myReader.ReadLine().Split(',');
                string CustomerAccountID = currentLine[5];
                DateTime createdDate = DateTime.Parse(currentLine[2]);

                statementData.Add(new Statement(CustomerAccountID, createdDate));
            }

            List<Simulation> Simulations = new List<Simulation>();
            List<TimeSpan> cycleLengthsToTest = new List<TimeSpan>()
            {
                new TimeSpan(1, 0, 0, 0, 0),
                new TimeSpan(2, 0, 0, 0, 0),
                new TimeSpan(3, 0, 0, 0, 0),
                new TimeSpan(4, 0, 0, 0, 0),
                new TimeSpan(5, 0, 0, 0, 0),
                new TimeSpan(6, 0, 0, 0, 0),
                new TimeSpan(7, 0, 0, 0, 0),
                new TimeSpan(10, 0, 0, 0, 0),
                new TimeSpan(12, 0, 0, 0, 0),
                new TimeSpan(15, 0, 0, 0, 0),
                new TimeSpan(31, 0, 0, 0, 0)
            };

            foreach(TimeSpan ts in cycleLengthsToTest)
            {
                Simulations.Add(RunSimulation(ts, statementData));
            }

            //Simulation mySim = RunSimulation(new TimeSpan(5, 0, 0, 0, 0), statementData);

            StreamWriter writer = new StreamWriter("outputdata.csv",false);
            writer.WriteLine("Simulation Number,CycleLength,StatementCount,CombinedStatementCount,CombinationRate");
            int simulationCount = 0;
            foreach(Simulation s in Simulations)
            {
                writer.WriteLine(simulationCount.ToString() + $",{s.cycleLength.Days.ToString()},{s.GetStatementCount()},{s.GetCombinedStatementCount()},{s.GetCombinationRate()}");
                simulationCount++;
            }
            writer.Close();
        }

        public static Simulation RunSimulation(TimeSpan cycleLength, List<Statement> data)
        {
            List<Statement> workingData = data.Select(s => new Statement(s.CustomerAccountID, s.CreatedDate)).ToList();
            List<StatementCycle> currentCycles = new List<StatementCycle>();
            var startDateQuery = from d in workingData
                            select workingData.Min(x => x.CreatedDate);
            DateTime startDate = startDateQuery.First();

            //var endDateQuery = from d in data
            //                     select data.Max(x => x.CreatedDate);
            //DateTime endDate = endDateQuery.First();

            while(workingData.Count > 0)
            {
                //increase start date by cycle length
                DateTime cycleEndDate = startDate + cycleLength;

                //get statements from data from that cycle
                var currentCycleStatements = from s in workingData
                                             where s.CreatedDate <= cycleEndDate
                                               select s;
                //count how many statements there are in total
                int statementCount = currentCycleStatements.Count();

                //count how many statements in there have matching CustomerAccountIDs
                var combinedStatementsQuery = from s in currentCycleStatements
                                         group s by s.CustomerAccountID into g
                                         select new { CustomerAccountID = g.Key, StatementCount = g.Count() };

                //TODO: Fix this to account for combined statements with >2
                int combinedStatementsCount = combinedStatementsQuery.Where(x => x.StatementCount > 1).Count();


                // create a statement cycle object to hold all of that data
                StatementCycle currentCycle = new StatementCycle(startDate, cycleEndDate, statementCount, combinedStatementsCount);

                // add the statement cycle to the current simulation
                currentCycles.Add(currentCycle);

                //remove data from source data
                workingData.RemoveAll(x => x.CreatedDate <= cycleEndDate);

                startDate = cycleEndDate;
            }

            return new Simulation(cycleLength, currentCycles);
        }
    }
}
