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

            List<PreValue> preValues = null;

            // The original value will be array-based, but after saving in U7 it'll be dictionary based...
            if (!persistedPreVals.IsDictionaryBased)
            {
                preValues = persistedPreVals.PreValuesAsArray.ToList();
            } else
            {
                preValues = persistedPreVals.PreValuesAsDictionary.Values.ToList();
            }

            JArray controlDataArray = new JArray();

            for (var i = 0; i < preValues.Count(); i++)
            {
                JObject rowObject = JObject.Parse(preValues[i].Value);

                if (i == 0)
                {
                    // Property Values Row
                    editorValues.Add("showLabel", new PreValue(rowObject.Value<bool>("ShowLabel") ? "1" : "0"));
                    editorValues.Add("showGridHeader", new PreValue(rowObject.Value<bool>("ShowGridHeader") ? "1" : "0"));
                    editorValues.Add("showGridFooter", new PreValue(rowObject.Value<bool>("ShowGridFooter") ? "1" : "0"));
                    editorValues.Add("readOnly", new PreValue(rowObject.Value<bool>("ReadOnly") ? "1" : "0"));
                    editorValues.Add("tableHeight", new PreValue(rowObject.Value<string>("TableHeight")));
                    editorValues.Add("mandatory", new PreValue(rowObject.Value<string>("Mandatory"))); // TODO: wat dis?
                } else
                {
                    // Data Rows
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
            IDictionary<string, object> newEditorValue = new Dictionary<string, object>();

            // Row 1: Property Values
            JObject propertyValuesRow = new JObject();
            propertyValuesRow.Add("ShowLabel", int.Parse(editorValue["showLabel"].ToString()) == 1 ? true : false);
            propertyValuesRow.Add("ShowGridHeader", int.Parse(editorValue["showGridHeader"].ToString()) == 1 ? true : false);
            propertyValuesRow.Add("ShowGridFooter", int.Parse(editorValue["showGridFooter"].ToString()) == 1 ? true : false);
            propertyValuesRow.Add("ReadOnly", int.Parse(editorValue["readOnly"].ToString()) == 1 ? true : false);
            if (editorValue["tableHeight"] != null)
            {
                propertyValuesRow.Add("TableHeight", int.Parse(editorValue["tableHeight"].ToString()));
            }
           // propertyValuesRow.Add("mandatory", int.Parse(editorValue["mandatory"].ToString()) == 1 ? true : false);

            newEditorValue.Add("properties", propertyValuesRow);

            // Rows 2-N: Column values
            if (editorValue["columns"] != null)
            {
                JArray columnValuesRow = JArray.Parse(editorValue["columns"].ToString());

                for(var i = 0; i < columnValuesRow.Count; i++)
                {
                    var row = columnValuesRow[i];

                    JObject columnObject = new JObject();
                    columnObject.Add("Name", row["name"].Value<string>());
                    columnObject.Add("Alias", row["alias"].Value<string>());
                    columnObject.Add("Mandatory", false);
                    columnObject.Add("Visible", true);
                    columnObject.Add("ValidationExpression", "");
                    columnObject.Add("DataTypeId", row["dataTypeId"].Value<int>());

                    newEditorValue.Add($"row{i}", columnObject);
                }
            }

            return base.ConvertEditorToDb(newEditorValue, currentValue);
        }
    }
}
