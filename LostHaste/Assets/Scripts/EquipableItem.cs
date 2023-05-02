using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{

    public Animator animator;


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
        if(Input.GetMouseButtonDown(0) && InventorySystem.Instance.isOpen == false && CraftingSystem.Instance.isOpen == false && SelectionManager.Instance.handIsVisible == false) {
            
            GameObject selectedTree = SelectionManager.Instance.selectedTree;
            if(selectedTree != null) {
                selectedTree.GetComponent<ChopTree>().getHit();
            }
            
            animator.SetTrigger("swing");
        }


    }
}
