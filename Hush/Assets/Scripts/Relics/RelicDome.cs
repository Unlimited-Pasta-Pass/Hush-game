using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.Events;

public class RelicDome : MonoBehaviour, IKillable
{
    public int keysNeededToUnlock = 0;
    public UnityEvent onDestroyDome = new UnityEvent();
    [SerializeField] private int hitPoints = 150;
    [SerializeField] private int maxHitPoints = 150;
     
    
    public int HitPoints
    {
        get => hitPoints; 
        set
        {
            hitPoints = value;
            if (hitPoints <= 0)
                Die();
        }
    }

    void OnTriggerEnter(Collider collider) {
        
        if (collider.gameObject.CompareTag(Tags.Player) && GameMaster.keysInPossession >= keysNeededToUnlock) {

            gameObject.SetActive(false);
            
            GameMaster.ResetKeys(GameMaster.keysInPossession -keysNeededToUnlock);
        }
    }
    public void OpenDome() {
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            hitPoints = 0;
            Die();
        }
        if (hitPoints < maxHitPoints)
        {
            OnDestroyDome();
        }
    }

    public void Die()
    {
        // breaking animation
        Destroy(gameObject);
    }

    public void OnDestroyDome()
    {
        onDestroyDome.Invoke();
    }
    
    
}
