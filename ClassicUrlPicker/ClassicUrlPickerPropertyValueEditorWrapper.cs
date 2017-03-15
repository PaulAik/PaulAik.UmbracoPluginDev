using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ClassicUrlPicker.Models;
using Umbraco.Core.Models.Editors;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace ClassicUrlPicker
{
    public class ClassicUrlPickerPropertyValueEditorWrapper : PropertyValueEditorWrapper
    {
        public ClassicUrlPickerPropertyValueEditorWrapper(PropertyValueEditor wrapped) : base(wrapped)
        {

        }

        public override object ConvertDbToEditor(Property property, PropertyType propertyType, IDataTypeService dataTypeService)
        {
            if (property.Value == null || property.Value.ToString() == String.Empty)
            {
                return null;
            }

            var dataFormat = "";

            // Imply data format from the formatting of the serialized state
            if (property.Value.ToString().StartsWith("<"))
            {
                dataFormat = "xml";
            }
            else if (property.Value.ToString().StartsWith("{"))
            {
                dataFormat = "json";
            }
            else
            {
                dataFormat = "csv";
            }

            var state = new UrlPickerState();

            switch(dataFormat.ToLower())
            {
                case "xml":
                    XElement dataNode = null;
                    try
                    {
                         dataNode = XElement.Parse(property.Value.ToString());
                    } catch(Exception ex)
                    {
                        return null;
                    }

                    var modeAttribute = dataNode.Attribute("mode");
                    if (modeAttribute != null)
                    {
                        state.Mode = (UrlPickerMode)Enum.Parse(typeof(UrlPickerMode), modeAttribute.Value, false);
                    }

                    var newWindowElement = dataNode.Element("new-window");
                    if (newWindowElement != null)
                    {
                        state.NewWindow = bool.Parse(newWindowElement.Value);
                    }

                    var nodeIdElement = dataNode.Element("node-id");
                    if (nodeIdElement != null)
                    {
                        int nodeId;
                        if (int.TryParse(nodeIdElement.Value, out nodeId))
                        {
                            state.NodeId = nodeId;
                        }
                    }

                    var urlElement = dataNode.Element("url");
                    if (urlElement != null)
                    {
                        state.Url = urlElement.Value;
                    }

                    var linkTitleElement = dataNode.Element("link-title");
                    if (linkTitleElement != null && !string.IsNullOrEmpty(linkTitleElement.Value))
                    {
                        state.Title = linkTitleElement.Value;
                    }
                    break;
                case "csv":
                    var parameters = property.Value.ToString().Split(',');

                    if (parameters.Length > 0)
                    {
                        state.Mode = (UrlPickerMode)Enum.Parse(typeof(UrlPickerMode), parameters[0], false);
                    }
                    if (parameters.Length > 1)
                    {
                        state.NewWindow = bool.Parse(parameters[1]);
                    }
                    if (parameters.Length > 2)
                    {
                        int nodeId;
                        if (int.TryParse(parameters[2], out nodeId))
                        {
                            state.NodeId = nodeId;
                        }
                    }
                    if (parameters.Length > 3)
                    {
                        state.Url = parameters[3].Replace("&#45;", ",");
                    }
                    if (parameters.Length > 4)
                    {
                        if (!string.IsNullOrEmpty(parameters[4]))
                        {
                            state.Title = parameters[4].Replace("&#45;", ",");
                        }
                    }
                    break;
                case "json":
                    state = JsonConvert.DeserializeObject<UrlPickerState>(property.Value.ToString());
                    break;
            }

            property.Value = JsonConvert.SerializeObject(state);

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
            var adminModel = JsonConvert.DeserializeObject<UrlPickerState>(editorValue.Value.ToString());

            // Serialized return string
            string serializedData = null;

            switch (dataFormat.ToLower())
            {
                case "xml":

                    serializedData = new XElement("url-picker",
                                        new XAttribute("mode", adminModel.Mode),
                                        new XElement("new-window", adminModel.NewWindow),
                                        new XElement("node-id", adminModel.NodeId),
                                        new XElement("url", adminModel.Url),
                                        new XElement("link-title", adminModel.Title)
                                    ).ToString();

                    break;

                case "csv":
                    // Making sure to escape commas:
                    serializedData = adminModel.Mode + "," +
                                        adminModel.NewWindow + "," +
                                        adminModel.NodeId + "," +
                                        adminModel.Url.Replace(",", "&#45;") + "," +
                                        adminModel.Title.Replace(",", "&#45;");
                    break;

                case "json":
                    serializedData = JsonConvert.SerializeObject(adminModel);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return serializedData;
        }
    }
}
