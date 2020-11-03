class hubHandler{
    #myTurn = true;
    constructor(url){
        this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(url)
        .build();
        this.hubConnection.serverTimeoutInMilliseconds = 1000 * 60 * 60;
        this.hubConnection.on('Send', function(data){
            console.log(data);
        });
    }
    start(){
        this.hubConnection.start();
        this.onAllMethods();
    }
    findEnemy(){
        $.get({
            url: "/api/findEnemy",
        });
    }
    onAllMethods(){
        this.hubConnection.on('FindEnemy', function(data){
            console.log(data);
        });
        this.hubConnection.on('ReceiveField', function(data){
            console.log(data);
            enemyfield.setEnemyField(data);
        });
        this.hubConnection.on('SendField', function(data){
            var json = ownfield.toJSONstring();
            $.post({
                url: "/api/sendField",
                data: "json=" + json
            });
        });
    }
    changeMove(){
        this.hubConnection.invoke("IsMyTurn")
        this.#myTurn = !this.#myTurn;
    }
}