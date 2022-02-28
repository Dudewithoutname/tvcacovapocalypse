using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ushort ItemId;
    public byte Chance;
    private Item item; // this is reference to a spawned item

    public void TryRespawn()
    {
        if (item != null || Random.Range(0, 100) > Chance) return;
        
        item = Instantiate(ItemManager.Singleton.Items[ItemId], transform);
    }
}