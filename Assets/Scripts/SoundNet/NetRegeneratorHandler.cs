using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetRegeneratorHandler : MonoBehaviour
{
    public static NetRegeneratorHandler instance;
    [SerializeField] private float lineSpawnDelay = 3f;
    
    private void Awake()
    {
        instance = this;
    }
    
    private IEnumerator EnableAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(lineSpawnDelay);
        obj.SetActive(true);
    }
    
    public void ActivateAfterDelay(GameObject obj)
    {
        StartCoroutine(EnableAfterDelay(obj));
    }
}
