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

    public Animator animator;

    public float hungerUsedChoppingWood = 20;



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
        animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    public void getHit() {
        animator.SetTrigger("shake");

        treeHealth -= 2;
        PlayerState.Instance.currentHunger -= hungerUsedChoppingWood;

        if(treeHealth <= 0) {
            treeIsDead();
        }
    }

    void treeIsDead() {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        //new Vector3(treePosition.x - 4, treePosition.y + 1, treePosition.z)
        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"), treePosition, Quaternion.Euler(0,0,0));
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
