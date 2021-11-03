namespace Common
{
    public interface IKillable
    {
        public int HitPoints { set; get; }
        public void TakeDamage(int damage);
        public void Die();
    }
}
