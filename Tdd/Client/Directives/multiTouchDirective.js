//app.directive('multiTouch', ['$timeout', function($timeout) {
//    return {
//        restrict: 'A',
//        scope: {
//            shortTouch: '&',
//            longTouch: '&'
//        },
//        link: function($scope, $elm, $attrs) {
//            $elm.bind('pointerdown', function (evt) {
//                console.log('pointer down');
//                // Locally scoped variable that will keep track of the long press
//                $scope.longPress = true;

//                // We'll set a timeout for 400 ms for a long press
//                $scope.timeout = $timeout(function() {
//                    if ($scope.longPress) {
//                        $scope.timeout = null;
//                        $scope.longTouch();
//                    }
//                }, 600);
//            });

//            $elm.bind('pointerup', function (evt) {
//                console.log('pointer up');
//                // Prevent the onLongPress event from firing
//                $scope.longPress = false;

//                // If the timeout exists it hasn't fired, so short press, otherwise long press
//                if ($scope.timeout) {
//                    clearTimeout($scope.timeout);
//                    $scope.shortTouch();
//                }
//            });
//        }
//    };
//}]);