//used for the media picker dialog
angular.module("umbraco").controller("PaulAik.UltimateUrlPickerDialog.Controller",
	function ($scope, eventsService, dialogService, entityResource, contentResource, mediaHelper, userService, localizationService) {
	    var dialogOptions = $scope.model;

	    $scope.contentTypeOptions = [
            { id: 'url', name: 'URL' },
            { id: 'content', name: 'Content' },
            { id: 'media', name: 'Media' }
	    ];

	    // TODO: from passed-in data.
	    $scope.contentTypeOption = ''; 

	    $scope.model.target = {};

	    //if (dialogOptions.currentTarget) {
	    //    $scope.model.target = dialogOptions.currentTarget;

	    //    //if we have a node ID, we fetch the current node to build the form data
	    //    if ($scope.model.target.id) {

	    //        if (!$scope.model.target.path) {
	    //            entityResource.getPath($scope.model.target.id, "Document").then(function (path) {
	    //                $scope.model.target.path = path;
	    //                //now sync the tree to this path
	    //                $scope.dialogTreeEventHandler.syncTree({ path: $scope.model.target.path, tree: "content" });
	    //            });
	    //        }

	    //        contentResource.getNiceUrl($scope.model.target.id).then(function (url) {
	    //            $scope.model.target.url = url;
	    //        });
	    //    }
	    //}

	    $scope.dialogTreeEventHandler = $({});

	    function nodeSelectHandler(ev, args) {
	        args.event.preventDefault();
	        args.event.stopPropagation();

	        if ($scope.currentNode) {
	            //un-select if there's a current one selected
	            $scope.currentNode.selected = false;
	        }

	        $scope.currentNode = args.node;
	        $scope.currentNode.selected = true;
	        $scope.model.target.id = args.node.id;
	        $scope.model.target.name = args.node.name;

	        if (args.node.id < 0) {
	            $scope.model.target.url = "/";
	        }
	        else {
	            contentResource.getNiceUrl(args.node.id).then(function (url) {
	                $scope.model.target.url = url;
	            });
	        }

	        if (!angular.isUndefined($scope.model.target.isMedia)) {
	            delete $scope.model.target.isMedia;
	        }
	    }

	    $scope.dialogTreeEventHandler.bind("treeNodeSelect", nodeSelectHandler);

	    $scope.switchToMediaPicker = function () {
	        userService.getCurrentUser().then(function (userData) {
	            $scope.mediaPickerOverlay = {
	                view: "mediapicker",
	                startNodeId: userData.startMediaId,
	                show: true,
	                submit: function (model) {
	                    var media = model.selectedImages[0];

	                    $scope.model.target.id = media.id;
	                    $scope.model.target.isMedia = true;
	                    $scope.model.target.name = media.name;
	                    $scope.model.target.url = mediaHelper.resolveFile(media);

	                    $scope.mediaPickerOverlay.show = false;
	                    $scope.mediaPickerOverlay = null;
	                }
	            };
	        });
	    };

	    $scope.$on('$destroy', function () {
	        $scope.dialogTreeEventHandler.unbind("treeNodeSelect", nodeSelectHandler);
	    });
	});