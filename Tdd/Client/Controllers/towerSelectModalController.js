app.controller('TowerSelectModalController', ['$scope', 'buildOptionsService', function ($scope, buildOptionsService) {
    $scope.fetching = true;

    buildOptionsService.getAllTowers().then(function (success) {
        $scope.towers = success.data;
    }).finally(function () {
        $scope.fetching = false;
    });
}]);