(function() {
    "use strict";

    var filters = angular.module("mb.filters", []);

    filters.filter("timeAgo", function() {
            return function(input) {
                return moment(input).fromNow();
            };
        })
        .filter("capitalize", function() {
            return function(input) {
                return _.capitalize(input);
            };
        })
        .filter("picture", function() {
            return function(img) {
                if (img)
                    return "pictures/" + img;
            };
        })
        .filter("postPicture", function() {
            return function(img) {
                if (img)
                    return "posts/" + img;
            };
        })
        .filter('followStatus', function() {
          return function(user) {
            if (user.Following)
              return 'Following'
            else
              return 'Follow';
          }
        });

})();
