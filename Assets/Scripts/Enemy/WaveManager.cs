using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public const ushort MAX_ENEMIES_ALIVE = 32;
    public static Difficulty Difficulty = Difficulties.List[DifficultyType.NORMAL];
    public static WaveManager Singleton
    {
        get => _singleton;
        private set => _singleton ??= value;
    }
    private static WaveManager _singleton;

    public Light2D EnviromentLight;
    public RoundState RoundState;
    public byte Wave;

    public ushort Enemies
    {
        get => _enemies;
        set
        {
            _enemies = value;
            if (UIManager.Singleton.WaveEnemies.text != null) UIManager.Singleton.WaveEnemies.text = $"Enemies: <color=#ffffff>{_enemies}</color>";
            if (_enemies == 0) waveEnd();
        }
    }
    
    private ushort _enemies;

    private void Awake() => Singleton = this;
    private void OnDestroy() => _singleton = null;
    
    private void Start()
    { 
        EnemyManager.Singleton.OnEnemyKilled += _ => Enemies--;
        UIManager.Singleton.WaveEnemies.gameObject.SetActive(false);
        MapManager.Singleton.SetLights(false);

        RoundState = RoundState.WAITING;
        StartCoroutine(freeTime(8));
    }

    private void waveStart()
    {
        Wave++;
        RoundState = RoundState.WAVE;
        Enemies = getEnemyCountByRound();
        
        UIManager.Singleton.WaveEnemies.gameObject.SetActive(true);
        UIManager.Singleton.WaveInfo.text = $"Wave {Wave}";

        if (UIManager.Singleton.GUIShop.activeSelf) ShopManager.Singleton.ShopTrigger();
        
        Player.Singleton.PlayAudio(EffectLibrary.AudioClips["upozornujem"], true);
        MapManager.Singleton.RespawnItems();
        
        StartCoroutine(changeLightning());
        StartCoroutine(waveLoop());
    }
    
    private void waveEnd()
    {
        RoundState = RoundState.FREETIME;
        UIManager.Singleton.WaveEnemies.gameObject.SetActive(false);

        StartCoroutine(changeLightning());
        StartCoroutine(freeTime(25));
        StartCoroutine(sayDelmocia());
    }

    private IEnumerator sayDelmocia()
    {
        yield return new WaitForSeconds(1f);
        Player.Singleton.PlayAudio(EffectLibrary.AudioClips["delmocia"], true);
    }

    private IEnumerator freeTime(byte seconds)
    {
        var time = seconds;
        while (time > 1)
        {
            time--;
            UIManager.Singleton.WaveInfo.text = $"Get Ready\n{time}";
            
            yield return new WaitForSeconds(1f);
        }
        waveStart();
    }

    private IEnumerator changeLightning()
    {
        var speed = 0.01f;

        if (EnviromentLight.intensity < 1.0f) // to day
        {
            var pos = 0f;
            while (EnviromentLight.intensity < 1.0f)
            {
                EnviromentLight.intensity = Mathf.Lerp(pos, 1.0f, speed);
                pos += speed;

                if (Player.Singleton.FlashLight.intensity > 0.9f && EnviromentLight.intensity >= 0.2f) 
                    Player.Singleton.FlashLight.intensity = 0f;

                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            EnviromentLight.intensity = 1.0f;
        }
        else // to night
        {
            var pos = 1f;
            while (EnviromentLight.intensity > 0f)
            {
                EnviromentLight.intensity = Mathf.Lerp(pos, 0.0f, speed);
                pos -= speed;

                if (Player.Singleton.FlashLight.intensity < 0.9f && EnviromentLight.intensity <= 0.2f) 
                    Player.Singleton.FlashLight.intensity = 1f;

                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            EnviromentLight.intensity = 0f;
        }
        
        MapManager.Singleton.SetLights(EnviromentLight.intensity < 1.0f);
    }
    
    private IEnumerator waveLoop()
    {
        var queue = new List<string>(genWaveEnemies());
        
        while (RoundState == RoundState.WAVE)
        {
            var newEnemies = MAX_ENEMIES_ALIVE - EnemyManager.Singleton.LiveEnemies.Count;

            while (newEnemies > 0 && queue.ElementAtOrDefault(0) != null)
            {
                newEnemies--;
                
                EnemyManager.SpawnEnemy(queue[0]);
                queue.RemoveAt(0);
                
                yield return new WaitForSeconds(Difficulty.SpawnTime);
            }
            
            yield return new WaitForSeconds(1.5f);
        }
    }
    
    private List<string> genWaveEnemies()
    {
        var countTotal = getEnemyCountByRound();
        var enemies = new List<string>();

        var countMedium = (ushort) Math.Floor(Difficulty.MediumRate);
        var countHard = (ushort) Math.Floor(Difficulty.HardRate);
        
        var mediumEnemies = EnemyManager.Singleton.EnemyList.Where(pair => pair.Value.Difficulty == 2).Select(pair => pair.Key).ToList();
        if (Wave >= Difficulty.MediumWave)
        {
            for (var i = 0; i < countMedium; i++)
            {
                enemies.Add(mediumEnemies[Random.Range(0, mediumEnemies.Count)]);
            }
        }

        var hardEnemies = EnemyManager.Singleton.EnemyList.Where(pair => pair.Value.Difficulty == 3).Select(pair => pair.Key).ToList();
        if (Wave >= Difficulty.HardWave)
        {
            for (var i = 0; i < countHard; i++)
            {
                enemies.Add(hardEnemies[Random.Range(0, hardEnemies.Count)]);
            }
        }

        if (enemies.Count != countTotal)
        {
            var lowEnemies = EnemyManager.Singleton.EnemyList.Where(pair => pair.Value.Difficulty == 1).Select(pair => pair.Key).ToList();
            for (var i = enemies.Count; i < countTotal; i++)
            {
                enemies.Add(lowEnemies[Random.Range(0, lowEnemies.Count)]);
            }
        }
        

        enemies.Randomize();
        return enemies;
    }

    private ushort getEnemyCountByRound() => (ushort) (Wave * Difficulty.BaseEnemies > 666 ? 666 : Wave * Difficulty.BaseEnemies);
}

public enum RoundState
{
    WAITING,
    FREETIME,
    WAVE,
    BOSS
}