using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicDataTypeGrid.Models
{
    public class ClassicDataTypeGridColumnPreValue
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public bool Mandatory { get; set; }

        public bool Visible { get; set; }

        public string ValidationExpression { get; set; }

        public int DataTypeId { get; set; }
    }
}
