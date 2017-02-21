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

            JArray controlDataArray = new JArray();

            for (var i = 0; i < preValues.Count(); i++)
            {
                JObject rowObject = JObject.Parse(preValues[i].Value);

                if (i == 0)
                {
                    // Header
                    editorValues.Add("showLabel", new PreValue(rowObject.Value<bool>("ShowLabel") ? "1" : "0"));
                    editorValues.Add("showGridHeader", new PreValue(rowObject.Value<bool>("ShowGridHeader") ? "1" : "0"));
                    editorValues.Add("showGridFooter", new PreValue(rowObject.Value<bool>("ShowGridFooter") ? "1" : "0"));
                    editorValues.Add("readOnly", new PreValue(rowObject.Value<bool>("ReadOnly") ? "1" : "0"));
                    editorValues.Add("tableHeight", new PreValue(rowObject.Value<string>("TableHeight")));
                    editorValues.Add("mandatory", new PreValue(rowObject.Value<string>("Mandatory"))); // TODO: wat dis?
                } else
                {
                    // Rows
                    JObject jsonRowObject = new JObject();

                    jsonRowObject.Add("name", rowObject.Value<string>("Name"));
                    jsonRowObject.Add("alias", rowObject.Value<string>("Alias"));
                    jsonRowObject.Add("mandatory", rowObject.Value<bool>("Mandatory") ? "1" : "0");
                    jsonRowObject.Add("visible", rowObject.Value<bool>("Visible") ? "1" : "0");
                    jsonRowObject.Add("validationExpression", rowObject.Value<string>("ValidationExpression"));
                    jsonRowObject.Add("dataTypeId", rowObject.Value<int>("DataTypeId"));

                    controlDataArray.Add(jsonRowObject);
                }
            }

            editorValues.Add("columns", new PreValue(controlDataArray.ToString()));

            return base.ConvertDbToEditor(defaultPreVals, new PreValueCollection(editorValues));
        }

        public override IDictionary<string, PreValue> ConvertEditorToDb(IDictionary<string, object> editorValue, PreValueCollection currentValue)
        {


            return base.ConvertEditorToDb(editorValue, currentValue);
        }
    }
}
