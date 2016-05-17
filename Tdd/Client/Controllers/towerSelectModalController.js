app.controller('TowerSelectModalController', ['$scope', '$uibModalInstance', 'buildOptionsService', function ($scope, $uibModalInstance, buildOptionsService) {
    $scope.fetching = true;

    buildOptionsService.getAllTowers().then(function (success) {
        $scope.towers = success.data;
    }).finally(function () {
        $scope.fetching = false;
    });

    $scope.selectTower = function (tower) {
        buildOptionsService.setSelectedTower(tower);
        $uibModalInstance.close(tower);
    }
}]);