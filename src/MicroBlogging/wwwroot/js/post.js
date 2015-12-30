(function() {
    'use strict';

    angular.module('mb.post', ['mb.filters', 'ngFileUpload', 'mb.session'])
      .controller('PostController',
      [       '$scope', '$state', '$http', 'Upload', 'Session',
      function($scope ,  $state ,  $http ,  Upload ,  Session) {
         $scope.post = {};

         $scope.focus = function() {
           $scope.post.focus = true;
         };

         $scope.cancel = function() {
            $scope.post = {};
         };

         var success = function(p) {
           $scope.post  = {};
           $scope.posts.splice(0, 0, p.data);
           $scope.posting = false;
         };

         var error = function(e) {
           $scope.posting = false;
           debugger;
         };

         var imageUploaded = function(post) {
           return function(picture) {
              post.Picture = picture.data;
              post.User    = Session.currentUser();
              success({data: post});
           };
         };

         var imageUploadError = function(error) {

         };

         var upload = function (file) {
           return function(resp) {
              $scope.uploading = true;
              var post = resp.data;
              Upload.upload({
                 url : '/api/posts/uploadpicture',
                 data: {file: file, postId: post.Id}
              }).then(imageUploaded(post), imageUploadError);
            }
        };

         var doPost = function(post) {
           return $http.post('/api/posts', post);
         }

         var createPost = function (post) {
           $scope.posting = true;
           return doPost(post).then(success, error);
         };

         var createImagePost = function(post) {
           doPost(post).then(upload(post.file), error)
         };

         $scope.create = function(post) {
           if (post.file) {
             createImagePost(post)
           } else {
             createPost(post)
           }
         };
      }])
      .directive('mbPost' , function() {
        return {
          restrict   : 'E',
          templateUrl: 'templates/post.html',
          controller : 'PostController',
          scope      : {
            posts: '='
          }
        }
      });
})();
