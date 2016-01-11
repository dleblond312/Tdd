app.service('buildOptionsService', [function () {
    towers = [
        {
            id: 1,
            text: 'Simple Tower',
            damage: 10,
            speed: 100
        }, {
            id: 2,
            text: 'Slow Tower',
            damage: 20,
            speed: 175
        }
    ]
    this.getAllTowers = function () {
        return towers;
    }

    this.getTower = function (id) {
        var tower = towers.filter(function (tower) {
            return tower.id === parseInt(id);
        });
        if (tower && tower.length > 0) {
            return tower[0];
        }
        return null;
    }
}]);