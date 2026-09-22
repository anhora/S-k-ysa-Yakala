using UnityEngine;

public class CharacterMenuMovement : MonoBehaviour
{
    public float moveAmount = 30f;
    public float moveSpeed = 2f;

    private RectTransform rectTransform;
    private Vector2 startPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        rectTransform.anchoredPosition = startPosition + new Vector2(x, 0);
    }
}