using UnityEngine;

[System.Serializable]
public class SerializableDecimal : ISerializationCallbackReceiver
{
    [SerializeField] private string setter = "0";
    [HideInInspector] public decimal value;
 
    public void OnBeforeSerialize() {}
 
    public void OnAfterDeserialize()
    {
        if (!decimal.TryParse(setter, out value))
            setter = "error couldn't parse decimal";
    }
}