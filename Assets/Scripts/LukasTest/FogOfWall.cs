using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallFade : MonoBehaviour
{

    [SerializeField] private SpriteRenderer wallSprite;
    [SerializeField] private float fadeDuration;

    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wallSprite.DOFade(0f, fadeDuration);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wallSprite.DOFade(1f, fadeDuration);
        }
    }
}
