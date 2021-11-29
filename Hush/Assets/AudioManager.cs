using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ASWalking;
    public AudioSource ASRunning;
    public AudioSource ASPlayerDamage;
    public AudioSource ASPlayerDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        ASWalking = gameObject.AddComponent<AudioSource>();
        ASRunning = gameObject.AddComponent<AudioSource>();
        ASPlayerDamage = gameObject.AddComponent<AudioSource>();
        ASPlayerDeath = gameObject.AddComponent<AudioSource>();
    }

    public void PlayDeath()
    {
        
    }
    
    public 

    // Update is called once per frame
    void Update()
    {
        
    }
}
