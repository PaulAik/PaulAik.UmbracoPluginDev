using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.Editors;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace UltimateUrlPicker
{
    public class UltimateUrlPickerPropertyValueEditorWrapper : PropertyValueEditorWrapper
    {
        public UltimateUrlPickerPropertyValueEditorWrapper(PropertyValueEditor wrapped) : base(wrapped)
        {

        }

        public override object ConvertEditorToDb(ContentPropertyData editorValue, object currentValue)
        {
            if (editorValue.Value == null || string.IsNullOrEmpty(editorValue.Value.ToString()))
            {
                return "";
            }

            return editorValue.Value.ToString();
        }
    }
}
