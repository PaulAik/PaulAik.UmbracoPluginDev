angular.module("umbraco").controller("PaulAik.ClassicDataTypeGrid.DialogController",
    function ($scope, dataTypeResource, umbRequestHelper, $http) {
        var dialogOptions = $scope.model;

        $scope.rows = [];

        // If we had values passed in, set them as the target (sent back in the callback).
        //if (dialogOptions.dialogData) {
        //    $scope.rows = dialogOptions.dialogData;
        //}

        var properties = dialogOptions.config.columns;

        var url = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath + "/backoffice/ClassicDataTypeGrid/ClassicDataTypeGridApi/GetDataTypeEditors";

        var dataTypeIds = _.pluck(properties, 'dataTypeId');

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
            //$scope.rows = [];
            angular.forEach(properties, function (value, key) {
                var dataTypeEditor = _.findWhere(data, { id: value.dataTypeId });

                // Need to copy the data type editor here or all data type editors of the same type will share values
                var row = { name: value.name, editor: angular.copy(dataTypeEditor), alias: value.alias };

                $scope.rows.push(row);
            });

            //$scope.dataTypeResources = data;
        });


        var unsubscribe = $scope.$on("formSubmitting", function (ev, args) {
            console.log($scope.rows);
            $scope.model.target = [];
            angular.forEach($scope.rows, function (row, key) {
                $scope.model.target.push({
                    value: row.editor.value,
                    alias: row.alias
                });
            });
        });

    });
