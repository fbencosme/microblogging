(function() {
    "use strict";

    var session = angular.module("mb.session", ["ngCookies"]);

    session.factory("Session",
    [
                "$rootScope", "$location", "$cookieStore", "$http",
        function($rootScope ,  $location ,  $cookieStore ,  $http) {

            var modelProps, userCookieModel;
            modelProps = ["id", "username", "gender", "fullName"];

            userCookieModel = function(it) {
                return _.pick(it, modelProps);
            };

            return {
                cookieName: function() {
                    return $location.host() + ":" + $location.port() + "-MBCurrentUser";
                },

                init: function() {

                    var props, this$ = this;

                    $rootScope.$on("authorizationFailed", function() {
                        this$.destroy();
                    });

                    props = $rootScope.currentUser || $cookieStore.get(this.cookieName());

                    if (props != null && !_.isEmpty(props)) {
                        return this.create(props);
                    } else {

                        var create = function(it) {
                            this$.create(it.data);
                        };

                        var unauthorize = function() {
                            $rootScope.$broadcast("$sessionDestroyed", null);
                        };
                        $http.get("/api/users/current").then(create).catch(unauthorize);
                    }
                },

                isLoggedIn: function() {
                    return $rootScope.currentUser != null;
                },

                currentUser: function() {
                    return $rootScope.currentUser;
                },

                create: function(it) {
                    this.update(it);
                    $rootScope.$broadcast("$sessionCreated", it);
                },

                destroy: function() {
                    var this$ = this;

                    var doDestroy = function(it) {
                        var prevUser;
                        prevUser = $rootScope.currentUser;
                        $cookieStore.remove(this$.cookieName());
                        $cookieStore.remove("AuthToken");
                        $rootScope.currentUser = null;
                        $rootScope.$broadcast("$sessionDestroyed", prevUser);
                    };

                    $http.post("/api/users/logout").then(doDestroy);
                },

                extendUser: function(it) {
                    ng.extend($rootScope.currentUser, it);
                },

                update: function(it) {
                    $cookieStore.put(this.cookieName(), userCookieModel(it));
                    $rootScope.currentUser = it;
                    $rootScope.$broadcast("$sessionUpdated", it);
                }
            };
        }
    ]);
})();
