using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Data.OleDb;
using System.Xml;
using System.Windows.Input;
using System.Xml.Linq;
using System.Net.NetworkInformation;

namespace Kettera_console
{     
    internal class Program
    {
        static void Main(string[] args)
        {
            UI ui = UI.Instance;
            ui.Run();
        }      
    }
}
