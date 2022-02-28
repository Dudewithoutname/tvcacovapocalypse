using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ushort Id;
    public string Name;
    public ushort Damage;
    public bool Auto;
    public bool IsAvailabe = true;
    public bool IsAnimating;
    public SpriteRenderer Model;
    public WeaponType Type;
 
    public bool Buyable;
    public SerializableDecimal Price;
    
    public float IconWidth;
    public float IconHeight;
    public float IconRot;
    public Texture IconTexture;
    
    protected Animator animator;
    protected AudioSource audioSource;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
    }

    public virtual void Use(){}
}

public enum WeaponType : byte
{
    PRIMARY,
    SECONDARY,
    MELEE
}
