var app = angular.module('tdd', ['ngRoute']);

app.config(['$routeProvider', '$locationProvider',
  function($routeProvider, $locationProvider) {
    $locationProvider.html5Mode(true);

    $routeProvider.
        when('/', {
            templateUrl: '/Partial/Home.html',
            controller: 'HomeController'
        });
}]);

app.config(['$httpProvider', function($httpProvider) {
    $httpProvider.interceptors.push('httpRequestInterceptor');
}]);