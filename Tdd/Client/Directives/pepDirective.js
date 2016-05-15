app.directive('pointerdown', ['$timeout', function ($timeout) {
    return {
        link: function (scope, element, attrs) {
            // We need the directive to compile before we can bind it
            $timeout(function () {
                element.bind('pointerdown', scope.$eval(attrs['pointerdown']));
            });
        }
    }
}]);

app.directive('pointerup', ['$timeout', function ($timeout) {
    return {
        link: function (scope, element, attrs) {
            // We need the directive to compile before we can bind it
            $timeout(function () {
                element.bind('pointerup', scope.$eval(attrs['pointerup']));
            });
        }
    }
}]);