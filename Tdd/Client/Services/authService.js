app.service('authService', ['$http', '$q', '$location', '$cookies', function ($http, $q, $location, $cookies) {

    var serviceBase = 'https://' + $location.host() + ':' + $location.port() + '/';
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: ""
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });
    };

    var _login = function (loginData) {

        var data = "grant_type=password&username=" +
        loginData.userName + "&password=" + loginData.password;

        var deferred = $q.defer();

        $http.post(serviceBase + '/api/account/token', data, {
            headers:
            { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).success(function (response) {

            $cookies.putObject('authorizationData', { token: response.access_token, userName: loginData.userName });

            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _logOut = function () {

        $cookies.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";

        $location.path('/');
    };

    var _fillAuthData = function () {

        var authData = $cookies.getObject('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
        }
    }

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;

    return authServiceFactory;
}]);