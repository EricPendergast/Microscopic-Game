//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class CellGroup : MonoBehaviour {
    public bool addFlagella = false;
    public bool addLump = false;

    public void Start() {
    }

    public void Update() {
        if (addLump) {
            addLump = false;
            GameObject.Instantiate(Refs.inst.lump, transform);
        }
        if (addFlagella) {
            addFlagella = false;
            GameObject.Instantiate(Refs.inst.flagella, transform);
        }
    }
}
