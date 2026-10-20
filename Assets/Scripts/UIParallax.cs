using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIParallax : MonoBehaviour
{
    [Tooltip("Scroll speed. Use negative values to move visual elements to the right.")]
    public float scrollSpeed = -0.1f;

    private RawImage rawImage;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        Rect uvRect = rawImage.uvRect;
        uvRect.x += scrollSpeed * Time.deltaTime;
        rawImage.uvRect = uvRect;
    }
}