using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Destroying line renderer object.");
        this.gameObject.SetActive(false);
    }
}
