using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public abstract ushort Id { get; }
    public abstract string Name { get; }
}