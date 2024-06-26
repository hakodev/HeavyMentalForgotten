using UnityEngine;

public class MaterialChildChange : MonoBehaviour
{
    [SerializeField] private Material newMaterial; // Assign this in the Inspector


    private GameObject parentGameObject;
    private bool executed;

    private void Start()
    {
        // Get the parent object (assuming this script is attached to a child)
        parentGameObject = transform.gameObject;

        
    }

    private void Update()
    {
        if (executed == false)
        {
            LineRenderer[] allLineRenderers = parentGameObject.GetComponentsInChildren<LineRenderer>();

            // Change the material for each child line renderer
            foreach (var lineRenderer in allLineRenderers)
            {
                lineRenderer.material = newMaterial;
                executed = true;
            }
        }
        
    }
}