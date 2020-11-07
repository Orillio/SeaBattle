using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.Models
{
    public class Player
    {
        // событие на метод set у Field, когда оно меняется, то вызывается событие,
        // которое отправляет новую информацию о поле другому игроку
        public string Name { get; set; }
        public Field Field { get; set; }
        public bool MyTurn { get; set; }
    }
}