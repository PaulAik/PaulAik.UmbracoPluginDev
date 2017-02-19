angular.module("umbraco").controller("PaulAik.ClassicDataTypeGrid.DataTypeDialogController",
    function ($scope, dataTypeResource, umbRequestHelper, $http) {
        var dialogOptions = $scope.model;

        $scope.dataTypeResources = [];

        $scope.model.target = {};

        if (dialogOptions.dialogData) {
            $scope.model.target = dialogOptions.dialogData;
        }

        var url = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath + "/backoffice/ClassicDataTypeGrid/ClassicDataTypeGridApi/GetDataTypeEditors";

        umbRequestHelper.resourcePromise(
                    $http.get(url),
                    'Failed to retrieve content types'
        ).then(function (data) {
            $scope.dataTypeResources = data;
        });

    });
