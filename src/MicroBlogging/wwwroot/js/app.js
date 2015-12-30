(function() {
    "use strict";

    angular.module("MicroBlogger", [
            "ui.router",
            "ngAnimate",
            "ngMaterial",
            "mb.home",
            "mb.session"
        ])
        .run(
        [
                    "$rootScope", "$state", "$stateParams", "Session",
            function($rootScope ,  $state ,  $stateParams , Session) {

                Session.init();

                $rootScope.$state = $state;
                $rootScope.$stateParams = $stateParams;

                $rootScope.$on("$sessionCreated", function() {
                    $state.go("dashboard");
                });

                $rootScope.$on("$sessionDestroyed", function() {
                    $state.go("home");
                });

                $rootScope.go = function(to) {
                    $state.go(to);
                };

                $rootScope.$on("$stateChangeSuccess", function(event, toState) {
                    if (toState.data && toState.data.blur)
                        $(".background").addClass("blur");
                    else
                        $(".background").removeClass("blur");
                });
            }
        ])
        .config(
        [
            "$stateProvider", "$urlRouterProvider", "$mdThemingProvider",
            function($stateProvider, $urlRouterProvider, $mdThemingProvider) {

                $mdThemingProvider.theme("default")
                    .primaryPalette("blue")
                    .accentPalette("light-blue");

                $urlRouterProvider.otherwise("/");

                $stateProvider.state("home", {
                    url: "/",
                    templateUrl: "templates/home.html",
                    controller: "HomeController"
                }).state("login", {
                    data: { blur: true },
                    url: "/login",
                    templateUrl: "templates/login.html",
                    controller: "LoginController"
                }).state("signup", {
                    data: { blur: true },
                    url: "/signup",
                    templateUrl: "templates/signup.html",
                    controller: "SignupController"
                }).state("dashboard", {
                    url: "/dashboard",
                    templateUrl: "templates/dashboard.html",
                    controller: "DashboardController"
                });
            }
        ]);

})();
