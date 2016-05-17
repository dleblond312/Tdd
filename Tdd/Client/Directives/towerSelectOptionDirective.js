app.directive('towerSelectOption', ['$timeout', 'buildOptionsService', function ($timeout, buildOptionsService) {
    return {
        scope: {
            towerSelectOption: '=',
        },
        link: function (scope, element, attrs, controller) {
            element.bind('pointerup', function () {
                buildOptionsService.setSelectedTower(scope.towerSelectOption);
                scope.$parent.$parent.close();
            })
        }
    }
}]);