(function() {
    "use strict";

    angular.module("mb.posts", ["mb.filters"])
        .controller("PostsController",
        [
                    "$scope", "$state", "$http",
            function($scope ,  $state , $http) {
                var success = function(resp) {
                    $scope.posts = resp.data;
                };
                $http.get("/api/posts").then(success);
            }
        ])
        .directive("mbPosts", function() {
            return {
                restrict: "E",
                templateUrl: "templates/posts.html",
                controller: "PostsController",
                scope: {
                    posts: "="
                }
            };
        });
})();
