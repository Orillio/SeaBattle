using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace SeaBattle.Models
{
    public class Field
    {
        public int cellSize { get; set; }
        public int[][] matrix { get; set; }
        public Ship[] ships { get; set; }

        // {"Name":"Yes","Age":10,"Matrix":[1,2,3,4,5,6]}

        //{"matrix":[[0,0,0,0,0,0,0,1,0,0],[0,0,1,1,0,0,0,0,0,0],[1,0,0,0,0,0,1,1,1,1]
        //,[0,0,0,0,0,0,0,0,0,0],[1,1,1,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,1,0,0],[0,0,0,0,0,0,0,1,0,1]
        //,[0,1,1,0,0,1,0,0,0,0],[0,0,0,0,0,1,0,1,0,0],[0,0,0,0,0,1,0,0,0,0]],"ships":[{"x":6,"y":2,"kx":1,"ky":0,"decks":4,"hits":[]},
        //{"x":5,"y":7,"kx":0,"ky":1,"decks":3,"hits":[]},{"x":0,"y":4,"kx":1,"ky":0,"decks":3,"hits":[]},
        //{"x":2,"y":1,"kx":1,"ky":0,"decks":2,"hits":[]},{"x":1,"y":7,"kx":1,"ky":0,"decks":2,"hits":[]},
        //{"x":7,"y":5,"kx":0,"ky":1,"decks":2,"hits":[]},{"x":9,"y":6,"kx":1,"ky":0,"decks":1,"hits":[]},
        //{"x":7,"y":8,"kx":0,"ky":1,"decks":1,"hits":[]},{"x":0,"y":2,"kx":1,"ky":0,"decks":1,"hits":[]},
        //{"x":7,"y":0,"kx":0,"ky":1,"decks":1,"hits":[]}],"cellSize":30}

        public static Field DeserializeJson(string json) =>
            JsonConvert.DeserializeObject<Field>(json);
        public string SerializeJson() =>
            JsonConvert.SerializeObject(this);

        public void Hit(int x, int y, int shipIndex)
        {
            if (shipIndex != -1)
            {
                if (ships[shipIndex].decks == ships[shipIndex].hits) return;
                ships[shipIndex].hits++;
            }
            if (matrix[y][x] == 1) matrix[y][x] = 2;
            else if (matrix[y][x] == 0) matrix[y][x] = 3;
        }
    }
}
