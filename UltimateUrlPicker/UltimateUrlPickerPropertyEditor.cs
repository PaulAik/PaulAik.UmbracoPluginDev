using ClientDependency.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace UltimateUrlPicker
{
    [PropertyEditorAsset(ClientDependencyType.Javascript, "../App_Plugins/UltimateUrlPicker/urlpicker.controller.js")]
    [PropertyEditor("PaulAik.UltimateUrlPicker", "Ultimate URL Picker", "../App_Plugins/UltimateUrlPicker/urlpicker.html")]
    public class UltimateUrlPickerPropertyEditor : PropertyEditor
    {
        public UltimateUrlPickerPropertyEditor()
        {
            _defaultPreVals = new Dictionary<string, object>
                {
                    {"allowedmodes", "URL" },
                };
        }

        private IDictionary<string, object> _defaultPreVals;

        protected override PropertyValueEditor CreateValueEditor()
        {
            return new UltimateUrlPickerPropertyValueEditorWrapper(base.CreateValueEditor());
        }

        protected override PreValueEditor CreatePreValueEditor()
        {
            return new UltimateUrlPickerPreValueEditor();
        }

        public override IDictionary<string, object> DefaultPreValues
        {
            get { return _defaultPreVals; }
            set { _defaultPreVals = value; }
        }
    }
}
