angular.module("umbraco").controller("PaulAik.ClassicDataTypeGrid.PreValueColumnsController",
    function ($scope) {

        if (!$scope.model.value) {
            $scope.model.value = [];
        }

        $scope.openEditDialogue = function (rowIndex) {
            var dialogData = {};
            if (rowIndex >= 0) {
                dialogData = angular.copy($scope.model.value[rowIndex]);
            }

            // open a custom dialog
            $scope.overlay = {
                // set the location of the view
                view: "/App_Plugins/ClassicDataTypeGrid/prevalues/datatypedialog.html",
                title: "Data Type Editor",
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

    });
