using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {
    void Update() {
        gameObject.GetComponent<SpriteRenderer>().material.SetVector("cameraPosition", transform.parent.position);
    }
}
