using ClientDependency.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace ClassicDataTypeGrid.PropertyEditors
{
    [PropertyEditorAsset(ClientDependencyType.Javascript, "../App_Plugins/ClassicDataTypeGrid/classicdatatypegrid.controller.js")]
    [PropertyEditor("PaulAik.ClassicDataTypeGrid", "Classic Data Type Grid", "../App_Plugins/ClassicDataTypeGrid/classicdatatypegrid.html")]
    public class ClassicDataTypeGridPropertyEditor : PropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new ClassicDataTypeGridPreValueEditor();
        }
    }
}
