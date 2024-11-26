using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Kettera_console
{
    public class UI
    {
        private GymManagement gm;
        private UI()
        {
            gm = new GymManagement();
        }
        private static UI instance = null;
        public static UI Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UI();
                }
                return instance;
            }
        }
        //Metodeja, joilla ohjataan ohjelman suoritusta:
        public void ContinuePrompt()
        {
            Console.WriteLine("\nPaina ENTER jatkaaksesi.");
            Console.ReadLine();
            Console.Clear();
        }

        public void Run()
        { }

        public void ManualSelect(int value)
        {
            
            switch(value)
            {
                case 1:

                    break;
                case 2:

                    break;
            }
        }

        private string MainMenuRules()
        {
            string text;
            text = "1: Hallitse asiakkaita.\n";
            text += "2: Hallitse henkilökuntaa.\n";
            text += "3: Hallitse ryhmäliikuntatunteja.\n";
            text += "4: Hallitse varauksia.\n";
            text += "5: Hallitse valmentaja- ja asiakastietoja\n";
            text += "6: Hallitse kalentereja.\n";
            return text;
        }
    }   
}
