using UnityEngine;
using System.Collections;

public class ResizeBackground : MonoBehaviour
{
    
	void Start ()
    {
        resizeSprite();
    }

    void fixedUpdate()
    {
        if (Screen.fullScreen)
            resizeSprite();
    }
    void resizeSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height);
    }
}
