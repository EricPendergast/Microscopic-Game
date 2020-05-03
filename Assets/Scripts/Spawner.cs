using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject cellToSpawn;
    public GameObject currentCell = null;

    void Start() {
        currentCell = null;
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine() {
        while (true) {
            Debug.Log("Trying to spawn");
            if (currentCell == null || currentCell.gameObject.GetComponentInChildren<Nucleus>() == null) {
                currentCell = Instantiate(cellToSpawn, transform.position, transform.rotation);
            }
            yield return new WaitForSeconds(5);
        }
    }
}
