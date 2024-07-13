using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteFlickerer : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Color startColor;
    [SerializeField]
    private Color endColor;

    [SerializeField]
    private float flickerSpeed;


    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Mathf.PingPong(Time.time, flickerSpeed);
        Color newColor = Color.Lerp(startColor, endColor, time);

        spriteRenderer.color = newColor;
    }
}
