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
