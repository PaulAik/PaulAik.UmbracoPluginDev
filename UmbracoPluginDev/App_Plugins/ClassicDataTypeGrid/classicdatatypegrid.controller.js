angular.module("umbraco").controller("PaulAik.ClassicDataTypeGrid.Controller",
    function ($scope, umbRequestHelper, $http) {
        $scope.properties = $scope.model.config.columns;

        var url = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath + "/backoffice/ClassicDataTypeGrid/ClassicDataTypeGridApi/GetDataTypeEditors";

        var dataTypeIds = _.pluck($scope.properties, 'dataTypeId');

        umbRequestHelper.resourcePromise(
                    $http({
                        method: 'GET',
                        url: url,
                        params: {
                            'dataTypeIds[]': dataTypeIds
                        }
                    }),
                    'Failed to retrieve content types'
        ).then(function (data) {
            $scope.dataTypeResources = data;
        });

        if (!$scope.model.value || $scope.model.value == "") {
            $scope.model.value = [];
        }

        $scope.openEditDialogue = function (rowIndex) {
            var dialogData;
            if (rowIndex >= 0) {
                dialogData = angular.copy($scope.model.value[rowIndex]);
            }

            // open a custom dialog
            $scope.overlay = {
                // set the location of the view
                view: "/App_Plugins/ClassicDataTypeGrid/classicdatatypegriddialog.html",
                title: "Classic Data Type Grid",
                show: true,
                config: $scope.model.config,
                dialogData: dialogData,
                submit: function (model) {
                    $scope.overlay.show = false;
                    $scope.overlay = null;

                    if (rowIndex >= 0) {
                        $scope.model.value[rowIndex] = model.target;
                    } else {
                        $scope.model.value.push(model.target);
                    }
                },
                close: function (oldModel) {
                    $scope.overlay.show = false;
                    $scope.overlay = null;
                },
            };
        };

        $scope.displayValueForColumn = function(columnId, value) {
            var column = _.findWhere($scope.properties, { id: columnId });
            var dataType = _.findWhere($scope.dataTypeResources, { id: column.dataTypeId });

            switch (dataType.alias) {
                case "Umbraco.Textbox":
                    return value;
                case "PaulAik.UltimateUrlPicker": //TODO: Rename!
                    return value.url;
            }

            return "";
        }

    });
