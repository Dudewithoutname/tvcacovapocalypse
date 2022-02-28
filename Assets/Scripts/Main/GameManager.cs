using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameType Gametype = GameType.SINGLEPLAYER;
    public static GameManager Singleton
    {
        get => _singleton;
        private set => _singleton ??= value;
    }
    private static GameManager _singleton;

    public Loader Loader;
    public GameObject Components;

    public void Awake()
    {
        Singleton = this;
        Loader.gameObject.SetActive(true);        
    }
    private void OnDestroy() => _singleton = null;

}

public enum GameType : byte
{
    SINGLEPLAYER,
    MULTIPLAYER,
    HOST
}