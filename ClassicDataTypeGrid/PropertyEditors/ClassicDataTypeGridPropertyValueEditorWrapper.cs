using ClassicDataTypeGrid.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Editors;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.PropertyEditors;

namespace ClassicDataTypeGrid.PropertyEditors
{
    public class ClassicDataTypeGridPropertyValueEditorWrapper : PropertyValueEditorWrapper
    {
        public ClassicDataTypeGridPropertyValueEditorWrapper(PropertyValueEditor wrapped) : base(wrapped)
        {
        }

        public override object ConvertDbToEditor(Property property, PropertyType propertyType, IDataTypeService dataTypeService)
        {
            return base.ConvertDbToEditor(property, propertyType, dataTypeService);
        }

        public override object ConvertEditorToDb(ContentPropertyData editorValue, object currentValue)
        {
            // Get the prevalues
            var preValues = editorValue.PreValues.PreValuesAsDictionary;

            // Sort the column data to prevalues into something more usable..
            List<ClassicDataTypeGridColumnPreValue> columnPrevaluesArray = new List<ClassicDataTypeGridColumnPreValue>();
            for (var i = 1; i < preValues.Count; i++)
            {
                columnPrevaluesArray.Add(JsonConvert.DeserializeObject<ClassicDataTypeGridColumnPreValue>(preValues.ToList()[i].Value.Value));
            }

            var rowElements = new XElement("items");

            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", ""),
                rowElements
            );

            foreach (var row in JArray.Parse(editorValue.Value.ToString()))
            {
                // This is the row
                var rowElement = new XElement("item");

                // For each parameter, create a new child object in the XML
                foreach (JObject column in JArray.Parse(row.ToString()))
                {
                    var alias = column["alias"].Value<string>();

                    var preValue = columnPrevaluesArray.First(x => x.Alias == alias);

                    var rowData = new XElement(alias,
                         new XAttribute("nodeType", preValue.DataTypeId),
                         new XAttribute("nodeName", preValue.Name));

                    // THIS NEEDS TO BE CONVERTED BY THE CORRECT DATA TYPE CONVERTER!
                    if (column["value"] != null)
                    {
                        var dataType = UmbracoContext.Current.Application.Services.DataTypeService.GetDataTypeDefinitionById(preValue.DataTypeId);
                        var dataTypePreValues = UmbracoContext.Current.Application.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(preValue.DataTypeId);
                        var editor = PropertyEditorResolver.Current.GetByAlias(dataType.PropertyEditorAlias);

                        var editorToDb = editor.ValueEditor.ConvertEditorToDb(new ContentPropertyData(column["value"].ToString(), dataTypePreValues, dataType.AdditionalData), "");

                        rowData.Value = editorToDb.ToString();
                    }

                    rowElement.Add(rowData);
                }

                rowElements.Add(rowElement);
            }

            string strXML = string.Concat(doc.Declaration.ToString(), "\r\n",
                              doc.ToString());

            return base.ConvertEditorToDb(editorValue, currentValue);
        }
    }
}
