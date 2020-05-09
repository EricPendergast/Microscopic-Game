using UnityEngine;
using System.Collections.Generic;

class MaterialTimeUpdater : MonoBehaviour {
    public List<Material> materials;
    
    void Update() {
        foreach (var mat in materials) {
            mat.SetFloat("time", Time.time);
        }
    }
}
