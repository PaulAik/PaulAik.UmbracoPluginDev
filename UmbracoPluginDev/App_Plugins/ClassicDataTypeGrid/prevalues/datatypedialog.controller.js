angular.module("umbraco").controller("PaulAik.ClassicDataTypeGrid.DataTypeDialogController",
    function ($scope, dataTypeResource) {
        var dialogOptions = $scope.model;

        $scope.dataTypeResources = [];

        $scope.model.target = {};

        if (dialogOptions.dialogData) {
            $scope.model.target = dialogOptions.dialogData;
        }

        dataTypeResource.getAll().then(function (dataTypeResources) {
            $scope.dataTypeResources = dataTypeResources;
        });

    });
