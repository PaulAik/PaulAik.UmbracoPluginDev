using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.PropertyEditors;

namespace ClassicDataTypeGrid.PropertyEditors
{
    public class ClassicDataTypeGridPreValueEditor : PreValueEditor
    {
        public ClassicDataTypeGridPreValueEditor()
        {
            //create the fields
            Fields.Add(new PreValueField()
            {
                Description = "Show datatype name above grid",
                Key = "showLabel",
                View = "boolean",
                Name = "Show Label",
            });

            Fields.Add(new PreValueField()
            {
                Description = "Show grid header with search box",
                Key = "showGridHeader",
                View = "boolean",
                Name = "Show Grid Header",
            });

            Fields.Add(new PreValueField()
            {
                Description = "Show grid footer with paging and total rows",
                Key = "showGridFooter",
                View = "boolean",
                Name = "Show Grid Footer",
            });

            Fields.Add(new PreValueField()
            {
                Description = "Lock the grid for editing",
                Key = "readOnly",
                View = "boolean",
                Name = "Read Only",
            });

            Fields.Add(new PreValueField()
            {
                Description = "The grid height",
                Key = "gridHeight",
                View = "number",
                Name = "Table Height",
            });

            Fields.Add(new PreValueField()
            {
                Description = "",
                Key = "columns",
                View = "/app_plugins/ClassicDataTypeGrid/prevalues/prevaluecolumns.html",
                Name = "Columns",
            });
        }
    }
}
