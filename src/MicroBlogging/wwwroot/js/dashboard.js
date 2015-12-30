(function() {
    "use strict";

    var dashboard = angular.module("mb.dashboard",
    ["mb.post", "mb.posts", "mb.account", "mb.session", "mb.follows"]);

    dashboard.controller("DashboardController",
    [
                "$scope", "Session",
        function($scope , Session) {
            $scope.posts = [];
            $scope.logout = function() {
                Session.destroy();
            };

        }
    ]);
})();
