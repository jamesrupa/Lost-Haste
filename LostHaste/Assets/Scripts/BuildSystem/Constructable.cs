using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Constructable : MonoBehaviour
{
    // Validation Variables
    public bool isGrounded;
    public bool isOverlappingItems;
    public bool isValidToBeBuilt;
    public bool detectedGhostMemeber;
 
    // Materials Used
    private Renderer mRenderer; // first material
    public Material redMaterial;
    public Material greenMaterial;
    public Material defaultMaterial; // default meaning final material that is used when placed
 
    public List<GameObject> ghostList = new List<GameObject>();
 
    public BoxCollider solidCollider; // default box collider on 3d objects
    // needs to be set when making the modiel
 
    private void Start()
    {
        mRenderer = GetComponent<Renderer>();
 
        mRenderer.material = defaultMaterial;
        foreach (Transform child in transform)
        {
            ghostList.Add(child.gameObject);
        }
 
    }
    void Update()
    {
        if (isGrounded && isOverlappingItems == false)
        {
            isValidToBeBuilt = true;
        } 
        else
        {
            isValidToBeBuilt = false;
        }
    }
 
    private void OnTriggerEnter(Collider other)
    {
        // on the ground
        if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = true; 
        }
 
        // dont want overlap
        if (other.CompareTag("Tree") || other.CompareTag("pickable") && gameObject.CompareTag("activeConstructable"))
        {
 
            isOverlappingItems = true;
        }
 
        // ghost - duplicate of our foundation/constructable object
        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable"))
        {
            detectedGhostMemeber = true;
        }
    }
 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = false;
        }
 
        if (other.CompareTag("Tree") || other.CompareTag("pickable") && gameObject.CompareTag("activeConstructable"))
        {
            isOverlappingItems = false;
        }
 
        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable"))
        {
            detectedGhostMemeber = false;
        }
    }
 
    public void SetInvalidColor()
    {
        if(mRenderer != null)
        {
            mRenderer.material = redMaterial;
        }
    }
 
    public void SetValidColor()
    {
        mRenderer.material = greenMaterial;
    }
 
    public void SetDefaultColor()
    {
        mRenderer.material = defaultMaterial;
    }
 
    public void ExtractGhostMembers()
    {
        foreach (GameObject item in ghostList)
        {
            item.transform.SetParent(transform.parent, true);
            item.gameObject.GetComponent<GhostItem>().solidCollider.enabled = false;
            item.gameObject.GetComponent<GhostItem>().isPlaced = true;
        }
    }
}