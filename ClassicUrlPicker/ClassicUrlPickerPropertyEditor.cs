using ClientDependency.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace ClassicUrlPicker
{
    [PropertyEditorAsset(ClientDependencyType.Javascript, "../App_Plugins/ClassicUrlPicker/urlpicker.controller.js")]
    [PropertyEditor("PaulAik.ClassicUrlPicker", "Classic URL Picker", "../App_Plugins/ClassicUrlPicker/urlpicker.html")]
    public class ClassicUrlPickerPropertyEditor : PropertyEditor
    {
        public ClassicUrlPickerPropertyEditor()
        {
            _defaultPreVals = new Dictionary<string, object>
                {
                    {"allowedmodes", "1" },
                };
        }

        private IDictionary<string, object> _defaultPreVals;

        protected override PropertyValueEditor CreateValueEditor()
        {
            return new ClassicUrlPickerPropertyValueEditorWrapper(base.CreateValueEditor());
        }

        protected override PreValueEditor CreatePreValueEditor()
        {
            return new ClassicUrlPickerPreValueEditor();
        }

        public override IDictionary<string, object> DefaultPreValues
        {
            get { return _defaultPreVals; }
            set { _defaultPreVals = value; }
        }
    }
}
