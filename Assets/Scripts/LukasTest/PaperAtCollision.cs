using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperAtCollision : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            collider2D.enabled = false;
            rigidbody2D.isKinematic = true; //disables physics
            spriteRenderer.color = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
