using System;
using System.Collections.Generic;
using UnityEngine;

public class BigRoof : MonoBehaviour
{
    [SerializeField] private float opacity;
    [SerializeField] private List<SpriteRenderer> srList;
    private List<Color> defaultOpacity;

    private void Awake()
    {
        defaultOpacity = new List<Color>();
        foreach (var sr in srList) defaultOpacity.Add(sr.color);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        for (var i = 0; i < srList.Count; i++)
        {
            srList[i].color = new Color(defaultOpacity[i].r, defaultOpacity[i].g, defaultOpacity[i].b, opacity);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        for (var i = 0; i < srList.Count; i++)
        {
            srList[i].color = defaultOpacity[i];
        }
    }
}
