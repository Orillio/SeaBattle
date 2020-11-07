using Microsoft.AspNetCore.SignalR;
using SeaBattle.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#region pragmas
#pragma warning disable CS4014
#endregion
namespace SeaBattle.Models
{
    public class Session
    {
        Random r = new Random();
        public enum MessageType { Blue = 1, Red = 2 }
        public IHubContext<GameHub> HubContext { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public string CurrentTurn => Player1.MyTurn ? Player1.Name : Player2.Name;

        public Session(IHubContext<GameHub> hc, Player player1, Player player2)
        {
            HubContext = hc;
            Player1 = player1;
            Player2 = player2;
            string leadPlayer = "";
            if(r.Next(0, 2) == 0)
            {
                leadPlayer = player1.Name;
                player1.MyTurn = true;
                player2.MyTurn = false;
            }
            else
            {
                leadPlayer = player2.Name;
                player1.MyTurn = false;
                player2.MyTurn = true;
            }
            hc.Clients.Users(Player1.Name, Player2.Name).SendCoreAsync("SendField", new object[1]);
            //Timer timer = new Timer(callback, null, 0, 100);
            Message(Player1, $"Игра началась! Сейчас ход игрока - {leadPlayer}", MessageType.Blue);
            Message(Player2, $"Игра началась! Сейчас ход игрока - {leadPlayer}", MessageType.Blue);
        }
        public async Task Message(Player player, string data, MessageType type)
        {
            if(type == MessageType.Blue)
                await HubContext.Clients.User(player.Name)
                   .SendCoreAsync("OnBlueMessage", new object[] { data });
            else
                await HubContext.Clients.User(player.Name)
                   .SendCoreAsync("OnRedMessage", new object[] { data });
        }
        public void ChangeTurn()
        {
            Player1.MyTurn = !Player1.MyTurn;
            Player2.MyTurn = !Player2.MyTurn;
            HubContext.Clients.User(Player1.Name).SendCoreAsync("ChangeTurn", new object[] { Player1.MyTurn });
            HubContext.Clients.User(Player2.Name).SendCoreAsync("ChangeTurn", new object[] { Player2.MyTurn });
        }
    }
}
