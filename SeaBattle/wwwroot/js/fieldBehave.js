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
        try{
            enemyfield.hit(res.x, res.y);
        }
        catch{
            console.log("catched");
        }
    });
    $('.find_enemy').click(() =>{
        hub.findEnemy();
    });
});