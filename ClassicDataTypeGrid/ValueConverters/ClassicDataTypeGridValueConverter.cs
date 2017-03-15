using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core;
using uComponents.DataTypes.DataTypeGrid.Model;

namespace ClassicDataTypeGrid.ValueConverters
{
    [PropertyValueType(typeof(GridRowCollection))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class ClassicDataTypeGridValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            var isConverter = propertyType.PropertyEditorAlias.InvariantEquals("PaulAik.ClassicDataTypeGrid");

           if(isConverter)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            // TODO: Temp until we get our own XML de-serialiser on the go
            return new GridRowCollection(source.ToString());

            //return base.ConvertDataToSource(propertyType, source, preview);
        }
    }
}
