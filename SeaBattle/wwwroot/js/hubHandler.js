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
    }
    findEnemy(){
        this.hubConnection.on('FindEnemy', function(data){
            console.log(data);
        });
        $.get({
            url: "/api/findEnemy",
            success: () => console.log("Worked")
        });
    }
    on(method, func){
        this.hubConnection.on(method, func);
    }
    invoke(method, jsonObj){
        this.hubConnection.invoke(method, jsonObj.toJSON());
    }
    changeMove(){
        this.hubConnection.invoke("IsMyTurn")
        this.#myTurn = !this.#myTurn;
    }
}