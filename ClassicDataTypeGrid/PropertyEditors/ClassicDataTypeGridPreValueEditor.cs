using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
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
                Description = "The table height",
                Key = "tableHeight",
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

        public override IDictionary<string, object> ConvertDbToEditor(IDictionary<string, object> defaultPreVals, PreValueCollection persistedPreVals)
        {
            IDictionary<string, PreValue> editorValues = new Dictionary<string, PreValue>();

            var preValues = persistedPreVals.PreValuesAsArray.ToList();

            for(var i = 0; i < preValues.Count(); i++)
            {
                // Header
                if(i == 0)
                {
                    JObject headerObject = JObject.Parse(preValues[i].Value);

                    editorValues.Add("showLabel", new PreValue(headerObject.Value<bool>("ShowLabel") ? "1" : "0"));
                    editorValues.Add("showGridHeader", new PreValue(headerObject.Value<bool>("ShowGridHeader") ? "1" : "0"));
                    editorValues.Add("showGridFooter", new PreValue(headerObject.Value<bool>("ShowGridFooter") ? "1" : "0"));
                    editorValues.Add("readOnly", new PreValue(headerObject.Value<bool>("ReadOnly") ? "1" : "0"));
                    editorValues.Add("tableHeight", new PreValue(headerObject.Value<string>("TableHeight")));
                    editorValues.Add("mandatory", new PreValue(headerObject.Value<string>("Mandatory"))); // TODO: wat dis?
                }
            }

            return base.ConvertDbToEditor(defaultPreVals, new PreValueCollection(editorValues));
        }

        public override IDictionary<string, PreValue> ConvertEditorToDb(IDictionary<string, object> editorValue, PreValueCollection currentValue)
        {


            return base.ConvertEditorToDb(editorValue, currentValue);
        }
    }
}
