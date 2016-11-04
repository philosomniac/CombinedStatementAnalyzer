using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombinedStatementAnalyzer
{
    class Statement
    {
        public int UploadID;
        public int jobID;
        public DateTime CreatedDate;
        public string LOB;
        public string LOBNickname;
        public string CustomerAccountID;

        public Statement(string customeraccountid, DateTime createddate)
        {
            CustomerAccountID = customeraccountid;
            CreatedDate = createddate;
        }
    }
}
