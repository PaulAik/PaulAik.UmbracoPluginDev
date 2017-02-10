using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UltimateUrlPicker.Models;
using Umbraco.Core.Models.Editors;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace UltimateUrlPicker
{
    public class UltimateUrlPickerPropertyValueEditorWrapper : PropertyValueEditorWrapper
    {
        public UltimateUrlPickerPropertyValueEditorWrapper(PropertyValueEditor wrapped) : base(wrapped)
        {

        }

        public override object ConvertDbToEditor(Property property, PropertyType propertyType, IDataTypeService dataTypeService)
        {
            var preValues = dataTypeService.GetPreValuesByDataTypeId(propertyType.DataTypeDefinitionId);

            var preValueCollection = dataTypeService.GetPreValuesCollectionByDataTypeId(propertyType.DataTypeDefinitionId);

            var dataFormat = preValueCollection.PreValuesAsDictionary["dataformat"].Value;

            return base.ConvertDbToEditor(property, propertyType, dataTypeService);
        }

        public override object ConvertEditorToDb(ContentPropertyData editorValue, object currentValue)
        {
            if (editorValue.Value == null || string.IsNullOrEmpty(editorValue.Value.ToString()))
            {
                return "";
            }

            var dataFormat = editorValue.PreValues.PreValuesAsDictionary["dataformat"].Value;

            // Grab the angular formatted value
            var adminModel = JsonConvert.DeserializeObject<AdminLinkModel>(editorValue.Value.ToString());

            // Serialized return string
            string serializedData;

            switch (dataFormat)
            {
                case "xml":

                    serializedData = new XElement("url-picker",
                                        new XAttribute("mode", adminModel.ContentTypeOption),
                                        new XElement("new-window", adminModel.Target),
                                        new XElement("node-id", adminModel.Id),
                                        new XElement("url", adminModel.Url),
                                        new XElement("link-title", adminModel.Title)
                                    ).ToString();

                    break;

                case "csv":
                    // Making sure to escape commas:
                    serializedData = adminModel.ContentTypeOption + "," +
                                         adminModel.Target + "," +
                                        adminModel.Id + "," +
                                        adminModel.Url.Replace(",", "&#45;") + "," +
                                         adminModel.Title.Replace(",", "&#45;");

                    break;

                case "json":

                    // TODO!
                    //var jss = new JavaScriptSerializer();
                    //serializedData = jss.Serialize(this);

                    break;

                default:
                    throw new NotImplementedException();
            }

            return editorValue.Value.ToString();
        }
    }
}
