public interface IEnemy
{   
    // TODO: Not sure we need an ID
    public int ID { get; set; }
    void Die();
    void TakeDamage(int amount);
    void PerformAttack();
}
