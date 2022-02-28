using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Player.Singleton == null) return;
        
        var lerp = Vector3.Lerp(transform.position, Player.Singleton.transform.position, Time.fixedDeltaTime * (Player.Singleton.Speed + 0.2f));
        transform.position = new Vector3(lerp.x, lerp.y, transform.position.z);
    }
}
