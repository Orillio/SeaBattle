var ownfield;
var enemyfield;
$(document).ready(function () {
    ownfield = new Field(getElement('ownfield'));
    enemyfield = new Field(getElement('enemyfield'));

    ownfield.randomLocationShips();
    enemyfield.setEnemyField(ownfield);

    $('.own_field .bluetext').click(() =>{
        ownfield.randomLocationShips();
        hubConnection.invoke('Send', 'Hello There');
    });

    const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('/hub')
    .build();

    hubConnection.start();

    hubConnection.on('Send', function(data) {
        console.log(data);
    });
    
});