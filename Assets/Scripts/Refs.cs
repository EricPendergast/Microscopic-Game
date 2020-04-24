using UnityEngine;
using UnityEngine.Assertions;

public class Refs : MonoBehaviour {
    public static Refs inst;

    public GameObject flagella;
    public GameObject lump;
    public GameObject membrane;
    public GameObject cytoskeleton;

    public void Start() {
        Assert.IsNull(inst);
        inst = this;
    }
}
