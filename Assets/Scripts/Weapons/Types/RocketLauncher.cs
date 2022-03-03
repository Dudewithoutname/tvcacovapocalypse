using System.Collections;
using UnityEngine;

public class RocketLauncher : Gun
{
    public override void Use()
    {
        base.Use();
        SubModels[0].gameObject.SetActive(false);
    }

    public override IEnumerator ReloadTick()
    {
        yield return base.ReloadTick();
        SubModels[0].gameObject.SetActive(true);
    }
}
