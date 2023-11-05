using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.DB
{
    internal class SQLRow
    {
        internal SQLRow()
        {
            values = new List<object>();
        }

        public override string ToString()
        {
            return string.Join(" ", values);
        }

        public List<object> values;
    }
}
