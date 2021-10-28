using UnityEngine;

public class HideableObjectController : MonoBehaviour {

    public void OnFOVEnter() {
        // TODO Trigger reveal shader
        GetComponent<Renderer>().enabled = true;
    }
    
    public void OnFOVLeave() {
        // TODO Trigger hide shader
        GetComponent<Renderer>().enabled = false;
    }
}
