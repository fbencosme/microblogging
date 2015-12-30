(function() {
    "use strict";

    var signup = angular.module("mb.signup", ["mb.dashboard"]);

    signup.controller("SignupController",
    [
                "$scope", "$state", "$http", "Session",
        function($scope ,  $state ,  $http ,  Session) {

            $scope.genders = ["M", "F"];
            $scope.errors = {};
            $scope.user = {};

            var success = function(resp) {
                Session.create(resp.data);
            };

            var error = function(error) {
                $scope.errors = error.data;
                $scope.emailUsed = error.data === "EmailUsed";
            };

            $scope.continue = function(user) {
                $http.post("/api/users/signup", user).then(success, error);
            };
        }
    ]);
})();
