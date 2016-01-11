app.controller('CommandCardController', ['$scope', 'buildOptionsService', function ($scope, buildOptionsService) {
    $scope.towers = buildOptionsService.getAllTowers();
}]);