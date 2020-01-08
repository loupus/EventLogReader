using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EventLogReader
{
    public class Matcher
    {
        public Matcher()
        {

        }

        ~Matcher()
        {

        }

        void Match()
        {
            fsArgument temp = Globals.GetFirstFs();
            List<ewArgument> tempList = null;
            if (temp == null) return;

            // delete match
            if(temp.ChangeType == (int)WatcherChangeTypes.Deleted)
            {
                tempList = Globals.EwArgs.FindAll(x => x.EventID == 4660 || x.EventID == 5145);

            }
           
        }
    }
}
