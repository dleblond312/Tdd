app.directive('menuAction', ['$rootScope', function ($rootScope) {
    return {
        restrict: 'A',
        scope: {
            actionText: '@',
            actionType: '@',
            actionValue: '@'
        },
        templateUrl: '/Partial/Directives/MenuAction.html',
        link: function (scope, element, attrs) {
            scope.performAction = function () {
                $rootScope.$broadcast('menuAction', {
                    type: scope.actionType,
                    value: scope.actionValue
                });
            }
        }
    }
}]);