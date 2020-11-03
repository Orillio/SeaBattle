using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.Models
{
    public class Ship
    {
        public int x { get; set; }
        public int y { get; set; }
        public int kx { get; set; }
        public int ky { get; set; }
        public int decks { get; set; }
        public int[] hits { get; set; }

    }
}
