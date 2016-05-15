Plot = function (stage, elm) {

    this.setDimensions = function (x, y) {
        this.elm.style.width = x + 'px';
        this.elm.style.height = y + 'px';
        this.width = x;
        this.height = y;
    };
    this.position = function (x, y) {
        var xoffset = arguments[2] ? 0 : this.width / 2;
        var yoffset = arguments[2] ? 0 : this.height / 2;
        this.elm.style.left = (x - xoffset) + 'px';
        this.elm.style.top = (y - yoffset) + 'px';
        this.x = x;
        this.y = y;
    };
    this.setBackground = function (col) {
        this.elm.style.background = col;
    };
    this.kill = function () {
        stage.removeChild(this.elm);
    };
    this.rotate = function (str) {
        this.elm.style.webkitTransform = this.elm.style.MozTransform =
        this.elm.style.OTransform = this.elm.style.transform =
        'rotate(' + str + ')';
    };
    this.content = function (content) {
        this.elm.innerHTML = content;
    };
    this.round = function (round) {
        this.elm.style.borderRadius = round ? '50%/50%' : '';
    };
    this.elm = elm;
    this.elm.style.position = 'absolute';
    this.elm.style.display = "block";
};

app.directive('distributeCircular', ['$window', '$timeout', 'gameService', function ($window, $timeout, gameService) {
    return {
        link: function (scope, element, attrs) {
            $timeout(function () {
                var nodes = element.find('.circle-node');
                var plots = nodes.length,
                increase = Math.PI * 2 / plots,
                angle = 0,
                x = 0,
                y = 0;

                var dimensions = gameService.getGameRatio();

                for (var i = 0; i < plots; i++) {
                    var p = new Plot(element[0], nodes[i]);
                    p.setBackground('green');
                    p.setDimensions(dimensions, dimensions);
                    x = (dimensions * 3 * Math.cos(angle)) + ($(window).width() / 2);
                    y = (dimensions * 3 * Math.sin(angle)) + ($(window).height() / 2);
                    p.position(x, y);
                    angle += increase;
                }
            });
        }
    }
}]);
