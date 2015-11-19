app.service('mobService', ['$rootScope', function ($rootScope) {
    var mobs = [];

    this.getMobs = function () {
        return mobs;
    }

    $rootScope.$on('propertyUpdated', function (event, model) {
        if (model.type.indexOf('GameRound') === 0) {
            mobs = model.value;
        }
    });
}]);