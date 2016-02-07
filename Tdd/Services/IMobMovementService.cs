using Tdd.Models;

namespace Tdd.Services
{
    public interface IMobMovementService
    {
        void RemoveMobsAtEnding(Mob mob, GameRoom room, GameRound round);

        void UpdateMobLocation(Mob mob, GameRoom room, GameRound round);
    }
}