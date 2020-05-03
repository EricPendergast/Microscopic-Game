using UnityEngine;

// The purpose of this class is so that Awake doesn't get called on prefabs
public abstract class AwakeOnce : MonoBehaviour {
    [SerializeField]
    private bool initialized = false;

    void Awake() {
        if (!initialized) {
            DoAwake();
        }
        initialized = true;
    }

    public abstract void DoAwake();
}
