using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.DB
{
    internal class SQLColumnInfo
    {

        internal SQLColumnInfo(string columnName, Type columnType)
        {
            this.name = columnName;
            this.type = columnType;
        }
        internal SQLColumnInfo()
        {

        }

        public override string ToString()
        {
            //return $"{{{name}, {type.Name}}}";
            return name;
        }

        public string name;
        public Type type;
    }
}
