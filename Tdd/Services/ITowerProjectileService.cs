using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tdd.Models;

namespace Tdd.Services
{
    public interface ITowerProjectileService
    {
        void UpdateProjectiles(GameRoom room, GameRound round);
        void TickDots(Mob mob, GameRoom room, GameRound round);
    }
}
