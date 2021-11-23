using Common.Interfaces;

namespace Enemies
{
    public interface IEnemy : IKillable
    {
        public void PerformAttack();
    }
}

