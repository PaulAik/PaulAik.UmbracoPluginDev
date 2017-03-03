angular.module("umbraco").controller("PaulAik.ClassicDataTypeGrid.PreValueColumnsController",
    function ($scope) {

        if (!$scope.model.value) {
            $scope.model.value = [];
        }

        $scope.sortableOptions = {
            handle: '.handle',
            update: function (e, ui) {
                // Get the new and old index for the moved element (using the URL as the identifier)
                var newIndex = ui.item.index();
                var movedRowAlias = ui.item.attr('data-rowalias');
                var originalIndex = getElementIndexByAlias(movedRowAlias);

                // Move the element in the model
                var movedElement = $scope.model.value[originalIndex];
                $scope.model.value.splice(originalIndex, 1);
                $scope.model.value.splice(newIndex, 0, movedElement);
            },
        };

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

                    if (!model.target.id) {
                        // Each row needs an ID so it can be tracked later
                        var id = 1;

                        // Get Max ID
                        if (!_.isEmpty($scope.model.value)) {
                            var maxIdRow = _.max($scope.model.value, function (row) { return row.id });

                            id = maxIdRow.id + 1;
                        }

                        model.target.id = id;
                    }

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

        $scope.deleteRow = function (rowIndex) {
            $scope.model.value.splice(rowIndex, 1);
        };

        function getElementIndexByAlias(alias) {
            for (var i = 0; i < $scope.model.value.length; i++) {
                if ($scope.model.value[i].alias == alias) {
                    return i;
                }
            }

            return -1;
        }

    });
