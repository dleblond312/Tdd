app.service('gameDataService', ['$http', function ($http) {
    this.getTowerTypes = function () {
        return $http.get('/api/gamedata/gettowertypes');
    }
    
}]);