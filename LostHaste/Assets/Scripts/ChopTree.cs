using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class ChopTree : MonoBehaviour
{


    //variables
    public bool playerInRange;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeHealth;



    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            playerInRange = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        treeHealth = treeMaxHealth;
    }

    public void getHit() {
        StartCoroutine(hit());
    }

    public IEnumerator hit() {
        yield return new WaitForSeconds(0.6f);
        treeHealth -= 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(canBeChopped) {
            GlobalState.Instance.resourceHealth = treeHealth;
            GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
        }
    }
}
