using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Web.PropertyEditors;

namespace ClassicDataTypeGrid.PropertyEditors
{
    public class ClassicDataTypeGridPropertyValueEditorWrapper : PropertyValueEditorWrapper
    {
        public ClassicDataTypeGridPropertyValueEditorWrapper(PropertyValueEditor wrapped) : base(wrapped)
        {
        }

        public override string ConvertDbToString(Property property, PropertyType propertyType, IDataTypeService dataTypeService)
        {
            

            return base.ConvertDbToString(property, propertyType, dataTypeService);
        }
    }
}
