using UnityEngine;
using UnityEngine.Assertions;

// Contains constants relating to the balance of the game
public class CellPartBalance : MonoBehaviour {
    public static CellPartBalance i;

    public float friction = .99f;
    public float springDist = 1.5f;
    public float springFreq = 1;
    public float springBreakForce = 10;
    public float springMaxDist = 1;
    public float springUpdateTime = 1;

    public static void ConfigureJointConstants(SpringJoint2D joint) {
        joint.distance = CellPartBalance.i.springDist;
        joint.frequency = CellPartBalance.i.springFreq;
        joint.breakForce = CellPartBalance.i.springBreakForce;
        joint.autoConfigureDistance = false;
    }

    CellPartBalance() {
        if (i == null) {
            Debug.Log("New CellPartBalance created");
        } else {
            Debug.Log("CellPartBalance replaced");
        }
        i = this;
    }
}
