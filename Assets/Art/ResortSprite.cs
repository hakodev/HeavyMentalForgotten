using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class ResortSprite : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SortingGroup sortGroup;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortGroup = GetComponent<SortingGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sortGroup)
            sortGroup.sortingOrder = (int)(-10 * transform.position.z);

        else
            spriteRenderer.sortingOrder = (int)(-10 * transform.position.z);
    }
}
