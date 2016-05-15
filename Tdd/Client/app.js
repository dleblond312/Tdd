var app = angular.module('tdd', ['ngRoute', 'ngCookies', 'ngTouch', 'ui.bootstrap']);

app.constant('CONSTANTS', {
    PROJECTILE_RATIO: 12,
    MINI_MAP_RATIO: 3,
});

app.config(['$routeProvider', '$locationProvider',
  function($routeProvider, $locationProvider) {
    $locationProvider.html5Mode(true);

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/Partial/Account/Login.html"
    }).when("/register", {
        controller: "registerController",
        templateUrl: "/Partial/Account/Register.html"
    }).when('/', {
        templateUrl: '/Partial/Home.html',
        controller: 'HomeController',
        reloadOnSearch: true
    });

}]);

app.run(['authService', function (authService) {
    authService.fillAuthData();
}])

app.config(['$httpProvider', function($httpProvider) {
    $httpProvider.interceptors.push('httpRequestInterceptor');
    $httpProvider.interceptors.push('authInterceptor');
}]);