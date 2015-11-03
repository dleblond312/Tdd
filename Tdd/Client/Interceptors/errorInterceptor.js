
// Catches errors and logs them to console / redirects to error page
app.factory('httpRequestInterceptor', ['$q', function($q) {
    return {

        'responseError': function(error) {
            console.log(error);
            return $q.reject(error);
        }
    }
}]);