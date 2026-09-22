using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    public float moveAmount = 12f;
    public float zoomAmount = 0.025f;
    public float speed = 0.15f;

    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Vector3 startScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        startPosition = rectTransform.anchoredPosition;
        startScale = rectTransform.localScale;
    }

    void Update()
    {
        if (rectTransform == null)
            return;

        float time = Time.time * speed;

        float moveX = Mathf.Sin(time) * moveAmount;
        float moveY = Mathf.Cos(time * 0.8f) * (moveAmount * 0.5f);

        rectTransform.anchoredPosition =
            startPosition + new Vector2(moveX, moveY);

        float zoom =
            1f + Mathf.Sin(time * 0.7f) * zoomAmount;

        rectTransform.localScale =
            startScale * zoom;
    }
}