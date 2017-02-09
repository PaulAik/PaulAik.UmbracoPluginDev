//used for the media picker dialog
angular.module("umbraco").controller("PaulAik.UltimateUrlPickerDialog.Controller",
	function ($scope, eventsService, dialogService, entityResource, contentResource, mediaHelper, userService, localizationService) {
	    var dialogOptions = $scope.model;

	    $scope.contentTypeOptions = [
            { id: 'url', name: 'URL' },
            { id: 'content', name: 'Content' },
            { id: 'media', name: 'Media' }
	    ];

	    $scope.model.target = {};
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
	        $scope.model.target.id = args.node.id;
	        $scope.model.target.name = args.node.name;
	        $scope.model.target.title = args.node.name;
	        $scope.model.target.path = null;

	        entityResource.getPath($scope.model.target.id, "Document").then(function (path) {
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

	    function nodeExpandedHandler(ev, args) {
	        if (angular.isArray(args.children)) {

	            //iterate children
	            _.each(args.children, function (child) {
	                //check if any of the items are list views, if so we need to add a custom
	                // child: A node to activate the search
	                if (child.metaData.isContainer) {
	                    child.hasChildren = true;
	                    child.children = [
	                        {
	                            level: child.level + 1,
	                            hasChildren: false,
	                            name: searchText,
	                            metaData: {
	                                listViewNode: child,
	                            },
	                            cssClass: "icon umb-tree-icon sprTree icon-search",
	                            cssClasses: ["not-published"]
	                        }
	                    ];
	                }
	            });
	        }
	    }

	    function treeLoadedHandler(ev, args) {
	        tree = args.tree;

	        if ($scope.model.target.id) {

	            if (!$scope.model.target.path) {
	                entityResource.getPath($scope.model.target.id, "Document").then(function (path) {
	                    $scope.model.target.path = path;
	                    //now sync the tree to this path
	                    $scope.dialogTreeEventHandler.syncTree({ path: $scope.model.target.path, tree: "content" });
	                });
	            } else {
	                $scope.dialogTreeEventHandler.syncTree({ path: $scope.model.target.path, tree: "content" });
	            }
	        }
	    }

	    $scope.dialogTreeEventHandler.bind("treeNodeSelect", nodeSelectHandler);
	    $scope.dialogTreeEventHandler.bind("treeLoaded", treeLoadedHandler);
	    $scope.dialogTreeEventHandler.bind("treeNodeExpanded", nodeExpandedHandler);

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