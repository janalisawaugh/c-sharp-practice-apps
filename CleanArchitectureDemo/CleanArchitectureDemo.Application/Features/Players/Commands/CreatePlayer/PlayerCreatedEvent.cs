using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Features.Players.Commands.CreatePlayer
{
    public class PlayerCreatedEvent
    {
        public Player Player { get; }
        public PlayerCreatedEvent (Player player)
        { Player = player; }
    }
}
