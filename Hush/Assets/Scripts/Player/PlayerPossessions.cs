using UnityEngine;

public class PlayerPossessions : MonoBehaviour
{
    public float keysInPossession;
    public bool possessesRelic = true; // TODO: modify this value when the relic is actually obtained

    void Start()
    {
        keysInPossession = 0;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Key")
        {
            keysInPossession += 1;
        }
    }
}
