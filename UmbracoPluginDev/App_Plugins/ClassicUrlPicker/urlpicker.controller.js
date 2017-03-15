angular.module("umbraco")
    .controller("PaulAik.ClassicUrlPicker", function ($scope, dialogService) {

       function openEditDialogue() {
            // open a custom dialog
            $scope.overlay = {
                // set the location of the view
                view: "/App_Plugins/ClassicUrlPicker/urlpickerdialog.html",
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
                },
                close: function (oldModel) {
                    $scope.overlay.show = false;
                    $scope.overlay = null;
                },
            };
        };

        $scope.edit = function() {
            openEditDialogue();
        };

        $scope.add = function() {
            openEditDialogue();
        }

        $scope.remove = function () {
            $scope.model.value = null;
        }
    });