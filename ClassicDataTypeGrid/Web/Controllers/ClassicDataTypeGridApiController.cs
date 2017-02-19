using ClassicDataTypeGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace ClassicDataTypeGrid.Web.Controllers
{
    [PluginController("ClassicDataTypeGrid")]
    public class ClassicDataTypeGridApiController : UmbracoAuthorizedJsonController
    {
        [System.Web.Http.HttpGet]
        public IEnumerable<object> GetDataTypeEditors([FromUri]  int[] dataTypeIds)
        {
            List<ClassicGridDataTypeModel> dataTypeModels = new List<ClassicGridDataTypeModel>();

            List<IDataTypeDefinition> dataTypeDefinitions = new List<IDataTypeDefinition>();

            if (dataTypeIds != null && dataTypeIds.Count() > 0)
            {
                dataTypeDefinitions = Services.DataTypeService.GetAllDataTypeDefinitions()
                    .Where(x => dataTypeIds.Contains(x.Id))
                    .OrderBy(x => x.SortOrder)
                    .ToList();
            } else
            {
                dataTypeDefinitions = Services.DataTypeService.GetAllDataTypeDefinitions()
                    .OrderBy(x => x.SortOrder)
                    .ToList();
            }

            foreach(var dataTypeDefinition in dataTypeDefinitions)
            {
                var editor = PropertyEditorResolver.Current.GetByAlias(dataTypeDefinition.PropertyEditorAlias);
                var preVals = UmbracoContext.Current.Application.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dataTypeDefinition.Id);

                dataTypeModels.Add(new ClassicGridDataTypeModel()
                {
                    Alias = dataTypeDefinition.PropertyEditorAlias,
                    View = editor.ValueEditor.View,
                    Config = editor.PreValueEditor.ConvertDbToEditor(editor.DefaultPreValues, preVals),
                    Id = dataTypeDefinition.Id,
                    Name = dataTypeDefinition.Name
                });
            }

            return dataTypeModels;
        }
    }
}
