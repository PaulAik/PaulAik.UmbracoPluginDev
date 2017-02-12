//used for the media picker dialog
angular.module("umbraco").controller("PaulAik.UltimateUrlPickerDialog.Controller",
	function ($scope, eventsService, dialogService, entityResource, contentResource, mediaHelper, userService, localizationService) {
	    var dialogOptions = $scope.model;

	    $scope.modes = [
            { id: 1, name: 'URL' },
            { id: 2, name: 'Content' },
            { id: 3, name: 'Media' }
	    ];

	    $scope.model.target = { mode: 1 };
	    $scope.dialogTreeEventHandler = $({});

	    if (dialogOptions.dialogData) {
	        $scope.model.target = dialogOptions.dialogData;
	    }

	    $scope.allowNewWindow = dialogOptions.config.allowNewWindow == "1";
	    $scope.allowLinkTitle = dialogOptions.config.allowLinkTitle == "1";

	    function nodeSelectHandler(ev, args) {
	        args.event.preventDefault();
	        args.event.stopPropagation();

	        if ($scope.currentNode) {
	            //un-select if there's a current one selected
	            $scope.currentNode.selected = false;
	        }

	        $scope.currentNode = args.node;
	        $scope.currentNode.selected = true;
	        $scope.model.target.nodeId = args.node.id;
	        $scope.model.target.name = args.node.name;
	        $scope.model.target.title = args.node.name;
	        $scope.model.target.path = null;

	        entityResource.getPath($scope.model.target.nodeId, "Document").then(function (path) {
	            $scope.model.target.path = path;
	            $scope.dialogTreeEventHandler.syncTree({ path: $scope.model.target.path, tree: "content" });
	        });

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

	    function treeLoadedHandler(ev, args) {
	        tree = args.tree;

	        if ($scope.model.target.nodeId) {

	            if (!$scope.model.target.path) {
	                entityResource.getPath($scope.model.target.nodeId, "Document").then(function (path) {
	                    $scope.model.target.path = path;
	                    //now sync the tree to this path
	                    $scope.dialogTreeEventHandler.syncTree({ path: $scope.model.target.path, tree: "content" });
	                });
	            }
	        }
	    }

	    $scope.dialogTreeEventHandler.bind("treeNodeSelect", nodeSelectHandler);
	    $scope.dialogTreeEventHandler.bind("treeLoaded", treeLoadedHandler);

	    $scope.switchToMediaPicker = function () {
	        userService.getCurrentUser().then(function (userData) {
	            $scope.mediaPickerOverlay = {
	                view: "mediapicker",
	                startNodeId: userData.startMediaId,
	                show: true,
	                submit: function (model) {
	                    var media = model.selectedImages[0];

	                    $scope.model.target.nodeId = media.id;
	                    $scope.model.target.isMedia = true;
	                    $scope.model.target.name = media.name;
	                    $scope.model.target.title = media.name;
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