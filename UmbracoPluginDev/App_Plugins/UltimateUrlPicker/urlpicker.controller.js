angular.module("umbraco")
    .controller("PaulAik.UltimateUrlPicker", function ($scope, dialogService) {


       function openEditDialogue() {
            // open a custom dialog
            $scope.overlay = {
                // set the location of the view
                view: "/App_Plugins/UltimateUrlPicker/urlpickerdialog.html",
                title: "Configure URL",
                show: true,
                config: $scope.model.config,
                dialogData: angular.copy($scope.model.value),
                submit: function (model) {
                    $scope.overlay.show = false;
                    $scope.overlay = null;

                    if (model.target != null) {
                        $scope.model.value = model.target;
                    }

                    console.log("Submit");
                },
                close: function (oldModel) {
                    $scope.overlay.show = false;
                    $scope.overlay = null;
                    console.log("Close");
                },
            };
        };

        $scope.edit = function () {
            openEditDialogue();
        };
    });