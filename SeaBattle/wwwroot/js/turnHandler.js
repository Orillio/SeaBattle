
$(document).ready(function () {
    // var myTurn = true;
    // var enemyTurn = false;

    console.log(enemyfield);
    console.log(ownfield);


    $('.enemy_field .field_markup').click(e => {
        console.log(e);
        let result = enemyfield.getCoordsByOffset(e.offsetX, e.offsetY);
        enemyfield.hit(result.x, result.y);
        //     enemyTurn = !enemyTurn;
        //     myTurn = !myTurn;
        
    });
});