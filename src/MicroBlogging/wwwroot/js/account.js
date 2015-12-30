(function() {
    'use strict';

    angular.module('mb.account', ['ngFileUpload'])
      .controller('AccountController',
      [       '$scope', 'Upload',
      function($scope ,  Upload) {

        $scope.user  = _.cloneDeep($scope.currentUser);
        $scope.genders = ['M', 'F'];
        $scope.errors  = {};

        $scope.save = function(user) {
          Session.update(user);
        };

        var imageUploaded = function(resp) {
          $scope.uploading    = false;
          $scope.user.Picture = $scope.currentUser.Picture = resp.data
        };

        var imageUploadError = function() {
          $scope.uploading = false;
        };

        $scope.upload = function (file) {
           $scope.uploading = true;
           Upload.upload({
              url : '/api/users/uploadpicture',
              data: {file: file}
           }).then(imageUploaded,imageUploadError);
        };

      }])
      .directive('mbAccount' , function() {

        return {
          restrict   : 'E',
          templateUrl: 'templates/account.html',
          controller : 'AccountController'
        }
      });
})();
