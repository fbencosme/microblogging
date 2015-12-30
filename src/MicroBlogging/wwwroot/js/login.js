(function() {
    "use strict";

    var login = angular.module("mb.login", ["mb.dashboard"]);

    login.controller("LoginController",
    [
                "$scope", "$state", "$http", "Session",
        function($scope ,  $state ,  $http , Session) {
            $scope.errors = {};
            $scope.user = {};

            var success = function(resp) {
                Session.create(resp.data);
                $scope.loading = false;
            };

            var error = function(error) {
                $scope.errors = error.data;
                $scope.notFound = error.data === "entity not found";
                $scope.loading = false;
            };

            $scope.continue = function(user) {
                $scope.loading = true;
                $http.post("/api/users/signin", user).then(success, error);
            };
        }
    ]);
})();
