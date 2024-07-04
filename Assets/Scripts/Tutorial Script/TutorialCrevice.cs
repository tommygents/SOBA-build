using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCrevice : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public bool hasTouchedPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            hasTouchedPlayer = true;
        }
    }

    public IEnumerator BlinkSprite(int blinkCount, float duration)
    {
        float blinkTime = duration / (blinkCount * 2); // Calculate the time each blink should last

        for (int i = 0; i < blinkCount; i++)
        {
            // Toggle the renderer to make the sprite appear or disappear
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkTime);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkTime);
        }
    }


}
