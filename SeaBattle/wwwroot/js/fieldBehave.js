var ownfield;
var enemyfield;
var gameStarted;
var myTurn;
$(document).ready(function () {
    var action_button = document.getElementById("action_button");
    let isFinding = false; 
    ownfield = new Field(getElement('ownfield'));
    enemyfield = new Field(getElement('enemyfield'));
    
    const hub = new hubHandler('/hub');
    hub.start();

    $('.own_field .bluetext').click(() =>{
        ownfield.randomLocationShips();
    });
    $(".player2_field .clickable").click(e =>{
        if(!myTurn) return;
        let coords = enemyfield.getCoordsByOffset(e.offsetX, e.offsetY);
        let res = enemyfield.hit(coords.x, coords.y);
        if(res != undefined){
            hub.hitEnemy(res.x, res.y, res.shipIndex);
        }
    });
    $(action_button).click(() =>{
        if(!isFinding && !gameStarted){
            hub.enterQueue();
            isFinding = true;
            action_button.innerText = "Отменить поиск";
        }
        else if(isFinding && !gameStarted){
            hub.escapeQueue();
            isFinding = false;
            action_button.innerText = "Найти противника";
        }
        else if(gameStarted){
            hub.onEnd();
            console.log("сдался");
        }
    });
});