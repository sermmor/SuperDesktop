
using UnityEngine;

public class AutoScaleMaskToCamera : MonoBehaviour
{
    void Start()
    {
        SpriteMask sm = GetComponent<SpriteMask>();
        transform.localScale = new Vector3(1, 1, transform.localScale.z);
        Vector2 newScale = getScaleForSpriteAutoScale(sm);
        maximize(newScale);
    }   

    Vector2 getScaleForSpriteAutoScale(SpriteMask sm)
    {
        float width = sm.sprite.bounds.size.x;
        float height = sm.sprite.bounds.size.y;
        
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        return new Vector2(
            worldScreenWidth / width,
            worldScreenHeight / height
        );
    }

    void maximize(Vector2 newScale) => transform.localScale = new Vector3(newScale.x, newScale.y, transform.localScale.z);
}
