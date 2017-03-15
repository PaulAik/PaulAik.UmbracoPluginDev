using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.PropertyEditors;

namespace ClassicUrlPicker
{
    public class ClassicUrlPickerPreValueEditor : PreValueEditor
    {
        public ClassicUrlPickerPreValueEditor()
        {
            //create the fields
            Fields.Add(new PreValueField()
            {
                Description = "Select all modes which are allowed for this data type",
                Key = "allowedmodes",
                View = "/app_plugins/ClassicUrlPicker/prevalues/allowedmodes.html",
                Name = "Allowed modes",
            });

            Fields.Add(new PreValueField()
            {
                Description = "Choose a mode which is shown for empty instances of this data type",
                Key = "defaultmode",
                View = "/app_plugins/ClassicUrlPicker/prevalues/defaultmode.html",
                Name = "Default mode",
            });

            Fields.Add(new PreValueField()
            {
                Description = "To get at the data easily in .NET, you can use the method uComponents.DataTypes.UrlPicker.Dto.UrlPickerState.Deserialize and pass it the data in any format",
                Key = "dataformat",
                View = "/app_plugins/ClassicUrlPicker/prevalues/dataformat.html",
                Name = "Data format",
            });

            Fields.Add(new PreValueField()
            {
                Description = "User can specify a title for the link",
                Key = "allowLinkTitle",
                View = "boolean",
                Name = "Allow link title",
            });

            Fields.Add(new PreValueField()
            {
                Description = "User can specify link to open in new window",
                Key = "allowNewWindow",
                View = "boolean",
                Name = "Allow new window",
            });

        }
    }
}
