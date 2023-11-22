using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.DB
{
    internal class SQLTable
    {
        public SQLTable()
        {
            columns = new List<SQLColumnInfo>();
            rows = new List<SQLRow>();
        }

        public void ExecuteAndRead(MySqlCommand commandSql)
        {
            columns = new List<SQLColumnInfo>();
            rows = new List<SQLRow>();

            MySqlDataReader reader = commandSql.ExecuteReader();

            LastInsertedId = commandSql.LastInsertedId;

            if (!reader.HasRows)
            {
                reader.Close();
                return;
            }

            int rowIndex = 0;
            while (reader.Read())
            {
                rows.Add(new SQLRow());

                if (rowIndex == 0)
                {
                    for (int field = 0; field < reader.FieldCount; field++)
                    {
                        columns.Add(new SQLColumnInfo(reader.GetName(field), reader.GetFieldType(field)));
                    }
                }

                for (int j = 0; j < reader.FieldCount; j++)
                    rows[rowIndex].values.Add(reader.IsDBNull(j) ? null : reader.GetString(j));

                rowIndex++;
            }
            reader.Close();
        }

        public override string ToString()
        {
            string endl = Environment.NewLine;

            string result = $"{endl}TableSchema:{endl}" + string.Join(",", columns);
            result += $"{endl}Rows:{endl}";
            rows.ForEach(x => result += x.ToString() + endl);
            return result;
        }

        public long LastInsertedId;
        public List<SQLColumnInfo> columns;
        public List<SQLRow> rows;
    }
}
