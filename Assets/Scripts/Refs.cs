using UnityEngine;
using UnityEngine.Assertions;

public class Refs : MonoBehaviour {
    public static Refs inst;

    public GameObject flagella;
    public GameObject lump;


    public void Start() {
        Assert.IsNull(inst);
        inst = this;
    }
}
