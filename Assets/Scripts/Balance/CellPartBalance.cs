using UnityEngine;
using UnityEngine.Assertions;

// Contains constants relating to the balance of the game
public class CellPartBalance : MonoBehaviour {
    public static CellPartBalance i;

    public float friction = .99f;
    public float springDist = 1.5f;
    public float springFreq = 1;
    public float springMaxDist = 1;
    public float springUpdateTime = 1;

    CellPartBalance() {
        Assert.IsNull(i);
        i = this;
    }
}
