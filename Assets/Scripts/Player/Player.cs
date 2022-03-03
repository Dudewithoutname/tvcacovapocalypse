using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour
{
    public static Player Singleton
    {
        get => _singleton;
        private set => _singleton ??= value;
    }
    private static Player _singleton;
    
    public short Health
    {
        get => _health;
        set
        {
            var prevHealth = _health;
            
            _health = value;
            
            if (_health > MaxHealth) _health = MaxHealth;
            
            UIManager.Singleton.Health = _health.ToString();
            
            if (prevHealth > _health) AudioBulb.PlayAudio(HitSounds[Random.Range(0, HitSounds.Count)], 0.95f);
            
            if (_health <= 0)
            {
                UIManager.Singleton.Health = "0";
                if(!isDeath) die();
            }
        }
    }

    public decimal Money
    {
        get => _money;
        set
        {
            _money = value;
            // TODO FIX
            UIManager.Singleton.Money = _money.ToString(CultureInfo.CurrentCulture).Replace(',', '.');
        }
    }

    public short MaxHealth = 100;
    public float Speed;
    
    public SpriteRenderer Model;
    public PlayerWeapons Weapons;
    public Rigidbody2D Rb;
    public Light2D FlashLight;
    public AudioSource AudioSource;
    public List<AudioClip> HitSounds;
    
    private bool isDeath;
    private short _health;
    public decimal _money;

    private void Awake() => Singleton = this;

    private void Start()
    {
        Health = 100;
    }
    private void OnDestroy() => _singleton = null;

    private void FixedUpdate()
    {
        if (Health <= 0) return;
        
        var move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        move *= Time.fixedDeltaTime * Speed;

        move = Vector2.Lerp(Rb.position, Rb.position + move, Speed);

        Rb.MovePosition(move);
    }

    private void die()
    {
        isDeath = true;
        PlayAudio(EffectLibrary.AudioClips["kurvo"], true);
        UIManager.Singleton.DeathScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PlayAudio(AudioClip clip, bool priority = false)
    {
        if(AudioSource.isPlaying && !priority) return;
        
        AudioSource.PlayOneShot(clip);
    }
}
