(function() {
    "use strict";

    angular.module("mb.follows", ["mb.filters", "mb.session"])
        .controller("FollowsController",
        [
                    "$scope", "$state", "$http", "Session",
            function($scope ,  $state ,  $http , Session) {

                var success = function(resp) {
                    $scope.follows = resp.data;
                };

                $http.get("/api/users").then(success);

                var success = function(user, follow) {
                  return function(resp) {
                    user.Following = follow;
                    $scope.saving = false;
                  };
                };

                var followError = function() {
                    $scope.saving = false;
                };

                var unfollow = function(target, data) {
                   $http.post("/api/users/unfollow", data)
                   .then(success(target, false), followError);
                };

                var follow = function(target, data) {
                   $http.post("/api/users/follow", data)
                   .then(success(target, true), followError);
                };

                $scope.view = function(target) {

                  if ($scope.saving)
                    return;

                  $scope.saving = true;

                  var data = {
                    target: target.Id,
                    source: Session.currentUser().Id
                  };
                  if (target.Following)
                    unfollow(target, data)
                  else
                    follow(target, data)
                };
            }
        ])
        .directive("mbFollows", function() {
            return {
                restrict: "E",
                templateUrl: "templates/follows.html",
                controller: "FollowsController",
                scope: {
                    follows: "="
                }
            };
        });
})();
