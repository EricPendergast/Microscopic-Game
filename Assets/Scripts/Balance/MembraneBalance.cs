using UnityEngine;
using UnityEngine.Assertions;

// Contains constants relating to the balance of the game
public class MembraneBalance : MonoBehaviour {
    public static MembraneBalance i;

    //public float friction = .99f;
    public float immediateSpringDist = 1f;
    public float immediateSpringFreq = 1;
    public float awaySpringDist = 3f;
    public float awaySpringFreq = .5f;
    public float immediateMaxDist = 3;
    public float awayMaxDist = 3;

    MembraneBalance() {
        if (i == null) {
            Debug.Log("New MembraneBalance created");
        } else {
            Debug.Log("MembraneBalance replaced");
        }
        i = this;
    }
}
