using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    AutoScaleBackgroundToCamera autoScaleBackground;

    float[] _bounds;

    public float[] Bounds {get => _bounds; } // minX, maxX, minY, maxY;

    void Awake()
    {
        _bounds = null;
        autoScaleBackground = GetComponent<AutoScaleBackgroundToCamera>();
        if (autoScaleBackground) {
            StartCoroutine(doActionsWhenAutoScaleBackgroundIsEnding());
        }
    }

    IEnumerator doActionsWhenAutoScaleBackgroundIsEnding() {
        if (autoScaleBackground.IsAutoScaling) {
            yield return null;
        }

        // Calculate all the screen limits.
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height / Screen.height * Screen.width;
        float x = Camera.main.transform.position.x;
        float y = Camera.main.transform.position.y;

        _bounds = new float[]{
            x - (width / 2), // minX
            (width / 2) + x, // maxX
            y - (height / 2), // minY
            (height / 2) + y // maxY
        };
    }
}
