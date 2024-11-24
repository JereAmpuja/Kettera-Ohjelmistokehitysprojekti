using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kettera_console
{
    public class Trainer
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Trainer()
        {
            ID = 0;
            Name = "";
        }

        public Trainer(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public override string ToString()
        {
            return $"{ID,-5}{Name,-20}";
        }
    }
}
