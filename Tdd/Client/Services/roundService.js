
app.service('roundService', ['$rootScope', function ($rootScope) {
    var round = {};

    this.getRound = function () {
        return round;
    }

    $rootScope.$on('propertyUpdated', function (event, model) {
        if (model.type.indexOf('GameRound') === 0) {
            round = model.value;
        }
    });
}]);