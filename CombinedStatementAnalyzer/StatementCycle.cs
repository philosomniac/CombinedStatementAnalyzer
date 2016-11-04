using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombinedStatementAnalyzer
{
    class StatementCycle
    {
        public DateTime startDate;
        public DateTime endDate;
        public int statementCount;
        public int combinedStatementCount;
        public TimeSpan cycleLength;


        public StatementCycle(DateTime startdate, DateTime enddate, int statementcount, int combinedstatementcount)
        {
            startDate = startdate;
            endDate = enddate;
            statementCount = statementcount;
            combinedStatementCount = combinedstatementcount;
            cycleLength = startDate - endDate;
        }
    }
}
