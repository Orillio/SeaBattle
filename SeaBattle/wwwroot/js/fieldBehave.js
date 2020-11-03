var ownfield;
var enemyfield;
$(document).ready(function () {
    ownfield = new Field(getElement('ownfield'));
    enemyfield = new Field(getElement('enemyfield'));

    ownfield.randomLocationShips();
    // enemyfield.setEnemyField(ownfield);

    const hub = new hubHandler('/hub');
    hub.start();

    $('.own_field .bluetext').click(() =>{
        ownfield.randomLocationShips();
    });
    $(".player2_field .clickable").click(e =>{
        let res = enemyfield.getCoordsByOffset(e.offsetX, e.offsetY);
        enemyfield.hit(res.x, res.y);
    });
    $('.find_enemy').click(() =>{
        hub.findEnemy();
    });
});