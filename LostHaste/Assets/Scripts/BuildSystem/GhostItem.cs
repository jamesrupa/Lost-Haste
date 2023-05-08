using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class GhostItem : MonoBehaviour
{
    public BoxCollider solidCollider; // set manually
 
    public Renderer mRenderer;
    private Material semiTransparentMat; // Used for debug - insted of the full trasparent
    private Material fullTransparentMat;
    private Material selectedMaterial;
 
    public bool isPlaced;
 
    // A flag for the deletion algorithm
    public bool hasSamePosition = false;
    private void Start()
    {
        mRenderer = GetComponent<Renderer>();
        // We get them from the manager, because this way the referece always exists.
        semiTransparentMat = ConstructionManager.Instance.ghostSemiTransparentMat;
        fullTransparentMat = ConstructionManager.Instance.ghostFullTransparentMat;
        selectedMaterial = ConstructionManager.Instance.ghostSelectedMat;
 
        mRenderer.material = fullTransparentMat; //change to semi if in debug else full
        // We disable the solid box collider - while it is not yet placed
        // (unless we are in construction mode - see update method)
        solidCollider.enabled = false;
    }
 
    private void Update()
    {

        if(ConstructionManager.Instance.inConstructionMode) {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), ConstructionManager.Instance.player.GetComponent<Collider>());
        }

        // We need the solid collider so the ray cast will detect it
        if (ConstructionManager.Instance.inConstructionMode && isPlaced)
        {
            solidCollider.enabled = true;
        }
 
        if (!ConstructionManager.Instance.inConstructionMode)
        {
            solidCollider.enabled = false;
        }
 
        // Triggering the material
        if(ConstructionManager.Instance.selectedGhost == this.gameObject)
        {
            mRenderer.material = selectedMaterial;
        }
        else
        {
            mRenderer.material = fullTransparentMat; //change to semi if in debug else full
        }
    }
}