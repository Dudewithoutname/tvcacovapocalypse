using UnityEngine;

public class Roof : MonoBehaviour
{
    [SerializeField] private float opacity;
    private SpriteRenderer sr;
    private Color defaultColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        sr.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, opacity);  
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        sr.color = defaultColor;
    }
}
