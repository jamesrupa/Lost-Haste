using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{

    public Animator animator;
    private bool swingWait = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
        // left mouse button - interact with equipped item
        // when no menus are open
        if(Input.GetMouseButtonDown(0) && 
        InventorySystem.Instance.isOpen == false && 
        CraftingSystem.Instance.isOpen == false && 
        SelectionManager.Instance.handIsVisible == false && 
        swingWait == false && 
        !ConstructionManager.Instance.inConstructionMode) {
            swingWait = true;
            StartCoroutine(SwingSoundDelay());

            animator.SetTrigger("swing");
            StartCoroutine(newSwingDelay());
        }

    }

    IEnumerator newSwingDelay() {
        swingWait = false;
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator SwingSoundDelay() {
        yield return new WaitForSeconds(0.2f);

        SoundManager.Instance.playSound(SoundManager.Instance.toolSwingSound);

    }

    public void GetHit() {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;
            if(selectedTree != null) {
                SoundManager.Instance.playSound(SoundManager.Instance.chopSound);
                selectedTree.GetComponent<ChopTree>().getHit();
            }
    }
}
