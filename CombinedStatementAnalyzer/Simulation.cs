using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombinedStatementAnalyzer
{
    class Simulation
    {
        public TimeSpan cycleLength;
        public List<StatementCycle> cycleData;
        //public int StatementCount;
        //public int CombinedStatementCount;
        //public double combinationRate;

        public Simulation(TimeSpan cyclelength, List<StatementCycle> cycledata)
        {
            cycleLength = cyclelength;
            cycleData = cycledata;
        }

        public int GetStatementCount()
        {
            return cycleData.Sum(x => x.statementCount);
        }

        public int GetCombinedStatementCount()
        {
            return cycleData.Sum(x => x.combinedStatementCount);
        }

        public double GetCombinationRate()
        {
            return cycleData.Sum(x => x.combinedStatementCount) / cycleData.Sum(x => x.statementCount);
        }

    }
}
