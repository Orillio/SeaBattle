class Field{
    #ships = [];
    #matrix;
    #shipsCounter = [0, 4, 3, 2, 1];
    #cellSize = 30;
    get matrix(){return this.#matrix}
    get ships(){return this.#ships}
    setEnemyField(data){
        let parsedJSON = JSON.parse(data);
        this.#matrix = parsedJSON.matrix;
        this.#ships = parsedJSON.ships;

        for (let y = 0; y < this.#matrix.length; y++) {
            for (let x = 0; x < this.#matrix[y].length; x++) {
                if(this.#matrix[y][x] == 2) this.placeHitOnField(x, y);
                else if(this.#matrix[y][x] == 3) this.placeDotOnField(x, y);
            }
        }
    }
    setOwnField(data){
        this.cleanField();
        let parsedJSON = JSON.parse(data);
        this.#matrix = parsedJSON.matrix;
        this.#ships = parsedJSON.ships;
        for (let i = 0; i < this.#ships.length; i++) {
            this.field.appendChild(this.createShip(this.#ships[i]));
        }
        for (let y = 0; y < this.#matrix.length; y++) {
            for (let x = 0; x < this.#matrix[y].length; x++) {
                if(this.#matrix[y][x] == 2) this.placeHitOnField(x, y);
                else if(this.#matrix[y][x] == 3) this.placeDotOnField(x, y);
            }
        }
    }
    toJSONstring(){
        return JSON.stringify(this.toJSON());
    }
    toJSON(){
        var e = this.matrix;
        var obj = {
            matrix: this.matrix, 
            ships: this.ships,
            cellSize: this.#cellSize
        }
        return obj;
    }
    getCoordsByOffset(offsetX, offsetY){
        let x = Math.floor(offsetX / this.#cellSize);
        let y = Math.floor(offsetY / this.#cellSize);
        return {x, y};
    }
    placeDotOnField(x, y){
        var div = document.createElement('div');
        div.classList.add('dot');
        div.style.left = `${x * this.#cellSize + 13}px`;
        div.style.top = `${y * this.#cellSize + 13}px`;

        this.field.appendChild(div);
    }
    placeHitOnField(x, y){
        var div = document.createElement('div');
        div.classList.add('cross');
        div.style.left = `${x * this.#cellSize + 4}px`;
        div.style.top = `${y * this.#cellSize + 5}px`;

        this.field.appendChild(div);
    }
    hit(x, y){ 
        if(this.#matrix == undefined) return;
        if(this.#matrix[y][x] == 3 || this.#matrix[y][x] == 2) return;

        let shipIndex = -1;
        for (let i = 0; i < this.#ships.length; i++) {
            if(this.#ships[i].kx == 1){
                for (let shipInd = this.#ships[i].x; shipInd <= this.#ships[i].x + this.#ships[i].decks - 1; shipInd++) {
                    if(x == shipInd && y == this.#ships[i].y){
                        shipIndex = i;
                        this.#ships[i].hits++;
                        this.placeHitOnField(x, y);
                        this.#matrix[y][x] = 2;
                        return {x, y, shipIndex};
                    }
                }
            }
            else{
                for (let shipInd = this.#ships[i].y; shipInd <= this.#ships[i].y + this.#ships[i].decks - 1; shipInd++) {
                    if(y == shipInd && x == this.#ships[i].x){
                        shipIndex = i;
                        this.#ships[i].hits++;
                        this.placeHitOnField(x, y);
                        this.#matrix[y][x] = 2;
                        return {x, y, shipIndex};
                    }
                }
            }
        }
        this.placeDotOnField(x, y);
        this.#matrix[y][x] = 3;
        return {x, y, shipIndex};
    }
    cleanField(){
        this.#ships = [];
        this.field.innerHTML = '';
    }
    randomLocationShips(){
        this.cleanField();
        this.#matrix = createMatrix();
        for (let i = 1; i < this.#shipsCounter.length; i++) {
            for (let j = 0; j < i; j++) {
                let coords = this.getRandomCoords(this.#shipsCounter[i]);
                let ship = new Ship(coords.x, coords.y, coords.kx, coords.ky, coords.decks);
                this.fillShipInMatrix(ship);
                this.field.appendChild(this.createShip(ship));
                this.#ships.push(ship);
            }
        }
    }
    createShip(ship){
        let div = document.createElement('div');
        div.classList.add('ship');
        
        div.style.left = `${ship.x * this.#cellSize}px`;
        div.style.top = `${ship.y * this.#cellSize}px`;

        
        if(ship.kx == 1){
            if(ship.decks == 4){
                div.classList.add('fourdeck');
            }
            else if(ship.decks == 3){
                div.classList.add('threedeck');
            } 
            else if(ship.decks == 2){
                div.classList.add('twodeck');
            } 
            else if(ship.decks == 1){
                div.classList.add('onedeck');
            }
        }
        else{
            div.classList.add('rotated');
            if(ship.decks == 4){
                div.classList.add('fourdeck-rotated');
            }
            else if(ship.decks == 3){
                div.classList.add('threedeck-rotated');
            } 
            else if(ship.decks == 2){
                div.classList.add('twodeck-rotated');
            } 
            else if(ship.decks == 1){
                div.classList.add('onedeck-rotated');
            }
        }
        return div;
    }
    fillShipInMatrix(ship){
        if(ship.kx == 1){
            for (let i = ship.x; i < ship.x + ship.decks; i++) {
                this.#matrix[ship.y][i] = 1;
            }
        } else{
            for (let i = ship.y; i < ship.y + ship.decks; i++) {
                this.#matrix[i][ship.x] = 1;
            }
        }
    }
    checkCoords(kx, ky, decks, x, y){
        let fromX, fromY, toX, toY;
        fromX = x == 0 ? x : x - 1;
        if(x + kx * decks == 10 && kx == 1) toX = 9;
        else if(x + kx * decks < 10 && kx == 1) toX = x + kx * decks;
        else if(x == 9 && kx == 0) toX = x;
        else if(x < 9 && kx == 0) toX = x + 1;

        fromY = y == 0 ? y : y - 1;
        if(y + ky * decks == 10 && ky == 1) toY = 9;
        else if(y + ky * decks < 10 && ky == 1) toY = y + ky * decks;
        else if(y == 9 && ky == 0) toY = y;
        else if(y < 9 && ky == 0) toY = y + 1;

        for (let i = fromY; i <= toY; i++) {
            for (let j = fromX; j <= toX; j++) {
                if (this.#matrix[i][j] == 1) return false;
            }
        }
        return true;

    }
    getRandomCoords(decks){
        var kx, ky, x, y;
        if(getRandom(2) == 1) { // horizontal
            kx = 1; 
            ky = 0;
        } else { // vertical
            ky = 1;
            kx = 0; 
        }
        if (kx == 0){
            x = getRandom(10);
            y = getRandom(11 - decks);
        } else{
            x = getRandom(11 - decks);
            y = getRandom(10);
        }
        if (!this.checkCoords(kx, ky, decks, x, y)) return this.getRandomCoords(decks);
        return {kx, ky, x, y, decks}
    }
    constructor(field) {
        this.field = field;
    }
}
class Ship{
    constructor(x, y, kx, ky, decks){
        this.x = x;
        this.y = y;
        this.kx = kx;
        this.ky = ky;
        this.decks = decks;
        this.hits;
    }
}