angular.module("umbraco")
    .controller("PaulAik.UltimateUrlPicker",
    function ($scope, dialogService) {
        //alert("The controller has landed");

        $scope.openEditDialog = function () {
            // open a custom dialog
            $scope.overlay = {
                // set the location of the view
                view: "/App_Plugins/UltimateUrlPicker/urlpickerdialog.html",
                title: "Hello",
                show: true,
                // function called when dialog is closed
                callback: function (value) {

                }

            };
        };

    });