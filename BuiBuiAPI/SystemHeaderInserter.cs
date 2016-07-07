using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuiBuiAPI
{
    class SystemHeaderInserter
    {
        DataGridView _grid;
        public SystemHeaderInserter(DataGridView grid)
        {
            _grid = grid;
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public void Insert()
        {
            _grid.Rows.Add(Name,"Header", Value);
        }

        public void SafeInsert()
        {
            _grid.SafeCall(Insert);
        }

        public void Click(object sender, EventArgs e)
        {
            SafeInsert();
        }
    }
}
