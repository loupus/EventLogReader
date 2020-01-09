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

        /*
         https://docs.microsoft.com/en-us/windows/security/threat-protection/auditing/event-5145#table-of-file-access-codes


%%4416  -   ReadData (or ListDirectory)	0x1
%%4417  -   WriteData (or AddFile)	0x2,
%%4418  -   AppendData (or AddSubdirectory or CreatePipeInstance)	0x4,
%%4419  -   ReadEA	0x8,
%%4420  -   WriteEA	0x10,
%%4421  -   Execute/Traverse	0x20,
%%4422  -   DeleteChild	0x40,
%%4423  -   ReadAttributes	0x80,
%%4424  -   WriteAttributes	0x100,
%%1537  -   DELETE	0x10000,
%%1538  -   READ_CONTROL	0x20000,
%%1539  -   WRITE_DAC	0x40000,
%%1540  -   WRITE_OWNER	0x80000,
%%1541  -   SYNCHRONIZE	0x100000
%%1542  -   ACCESS_SYS_SEC	0x1000000,


"AO"	Account operators	
"RU"	Alias to allow previous Windows 2000	
"AN"	Anonymous logon
"AU"	Authenticated users	
"BA"	Built-in administrators
"BG"	Built-in guests	
"BO"	Backup operators	
"BU"	Built-in users	
"CA"	Certificate server administrators	
"CG"	Creator group	
"CO"	Creator owner	
"DA"	Domain administrators	
"DC"	Domain computers	
"DD"	Domain controllers	
"DG"	Domain guests	
"DU"	Domain users	
"EA"	Enterprise administrators	
"ED"	Enterprise domain controllers	
"WD"	Everyone	
"PA"	Group Policy administrators
"IU"	Interactively logged-on user
"LA"	Local administrator
"LG"	Local guest
"LS"	Local service account
"SY"	Local system
"NU"	Network logon user
"NO"	Network configuration operators
"NS"	Network service account
"PO"	Printer operators
"PS"	Personal self
"PU"	Power users
"RS"	RAS servers group
"RD"	Terminal server users
"RE"	Replicator
"RC"	Restricted code
"SA"	Schema administrators
"SO"	Server operators
"SU"	Service logon user

G: = Primary Group.
D: = DACL Entries.
S: = SACL Entries.

  ace_type:
"A" - ACCESS ALLOWED
"D" - ACCESS DENIED
"OA" - OBJECT ACCESS ALLOWED: only applies to a subset of the object(s).
"OD" - OBJECT ACCESS DENIED: only applies to a subset of the object(s).
"AU" - SYSTEM AUDIT
"A" - SYSTEM ALARM
"OU" - OBJECT SYSTEM AUDIT
"OL" - OBJECT SYSTEM ALARM

            ace_flags:
"CI" - CONTAINER INHERIT: Child objects that are containers, such as directories, inherit the ACE as an explicit ACE.
"OI" - OBJECT INHERIT: Child objects that are not containers inherit the ACE as an explicit ACE.
"NP" - NO PROPAGATE: only immediate children inherit this ace.
"IO" - INHERITANCE ONLY: ace doesn’t apply to this object, but may affect children via inheritance.
"ID" - ACE IS INHERITED
"SA" - SUCCESSFUL ACCESS AUDIT
"FA" - FAILED ACCESS AUDIT

         */

        void Match()
        {
            fsArgument temp = Globals.GetFirstFs();
            List<ewArgument> tempList = null;
            if (temp == null) return;

            // delete match
            if(temp.ChangeType == (int)WatcherChangeTypes.Deleted)
            {
                tempList = Globals.EwArgs.FindAll(x => x.EventID == 4660 || x.EventID == 5145);

                foreach(ewArgument e in tempList)
                {
                  // if()
                }

            }
           
        }
    }
}
