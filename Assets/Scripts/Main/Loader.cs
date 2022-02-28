using System;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public Text Status;

    private static ushort total = 7;
    private ushort completed;
    
    private void Awake()
    {
        Display("Prepairing");
    }

    private void Start()
    {
        GameManager.Singleton.Components.SetActive(true);
        
        UIManager.Singleton.Show();
        Destroy(gameObject);
    }

    public void Display(string status)
    {
        total++;
        var percentage = (byte)Math.Floor(completed / (total / 100f));
        Status.text = $"{status} {(percentage <= 0 ? 1 : percentage)}%";
    }

    public void Complete()
    {
        if (++completed == total) finished();
    }
    
    private void finished()
    {
        Destroy(gameObject);
    }
}