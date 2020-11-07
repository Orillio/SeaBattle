class hubHandler{
    constructor(url){
        this.gameStarted = false;
        this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(url)
        .build();
        this.hubConnection.serverTimeoutInMilliseconds = 1000 * 60 * 60;
        this.hubConnection.on('Send', function(data){
            console.log(data);
        });
    }
    restartIfNotConnected(){
        if(this.hubConnection.connectionState != "Connected") this.start();
    }
    start(){
        this.hubConnection.start();
        this.onAllMethods();
    }
    enterQueue(){
        $.get({
            url: "/api/enterQueue",
        });
    }
    escapeQueue(){
        $.get({
            url: "/api/escapeQueue",
        });
    }
    giveUp(){
        $.get({
            url: "/api/giveUp",
        });
    }
    isGameBegan(){
        $.get({
            url: "/api/myGameBegan",
            success: function (began) {
                if(!began){
                    ownfield.randomLocationShips();
                }
                else{
                    document.getElementById("dark_rect").style.display = "none";
                }
            }
        });
    }
    hitEnemy(x, y, shipIndex){
        let data = JSON.stringify({
            x: x,
            y: y,
            shipIndex: shipIndex,
        });
        $.post({
            url: "/api/hitEnemy",
            data: "json=" + data
        });
    }
    isMyTurn(){
        myturn = false;
        $.get({
            url: "/api/isMyTurn",
            data: "json=" + data,
            success: function(data){
                myturn = data;
            }
        });
        return myturn
    }
    onAllMethods(){
        this.hubConnection.on('FindEnemy', function(data){});
        
        this.hubConnection.on('ReceiveEnemyField', function(enemyField){
            document.getElementById("dark_rect").style.display = "none";
            enemyfield.setEnemyField(enemyField);
            document.getElementById("action_button").innerText = "Сдаться";
            gameStarted = true;
            $.get({
                url: "/api/isMyTurn",
                success: data =>{
                    myTurn = data; 
                }
            });
        });
        this.hubConnection.on('ReceiveOwnField', function(ownField){
            ownfield.setOwnField(ownField);
            $.get({
                url: "/api/isMyTurn",
                success: data =>{
                    myTurn = data; 
                }
            });
        });
        
        this.hubConnection.on('SendField', function(data){
            var json = ownfield.toJSONstring();
            $.post({
                url: "/api/sendField",
                data: "json=" + json
            });
        });
        this.hubConnection.on('ChangeTurn', function(data){
            myTurn = data;
        });
        this.hubConnection.on('OnSurrender', function(data){
            gameStarted = false;
            action_button.innerText = "Найти противника";
            document.getElementById("dark_rect").style.display = "flex";
            ownfield.cleanField();
            enemyfield.cleanField();
            ownfield.randomLocationShips();
        });
        this.hubConnection.on('OnRedMessage', function(data){
            redMessage(data);
        });
        this.hubConnection.on('OnBlueMessage', function(data){
            normalMessage(data);
        });
        setTimeout(() => {
            this.isGameBegan();
        }, 150);
    }
}