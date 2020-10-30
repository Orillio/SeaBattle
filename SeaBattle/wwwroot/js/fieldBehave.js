var ownfield;
var enemyfield;
$(document).ready(function () {
    ownfield = new Field(getElement('ownfield'));
    enemyfield = new Field(getElement('enemyfield'));

    ownfield.randomLocationShips();
    enemyfield.setEnemyField(ownfield);

    $('.own_field .bluetext').click(() =>{
        ownfield.randomLocationShips();
    });
    
});