function getElement(id) {
	return document.getElementById(id);
}
function findByClass(cl) {
	return document.getElementsByClassName(cl)[0];
}
function getRandom(n) { 
    return Math.floor(Math.random() * n);
}
function createMatrix(){
    let matrix = [10];
    for (let i = 0; i < 10; i++) {
        matrix[i] = [10]
        for (let j = 0; j < 10; j++) {
            matrix[i][j] = 0;
        }
    }
    return matrix;
}