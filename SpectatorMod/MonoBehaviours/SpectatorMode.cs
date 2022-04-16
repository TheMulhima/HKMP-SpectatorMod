using JetBrains.Annotations;
using UnityEngine.Serialization;
using Logger = Modding.Logger;
using HKMP_SpectatorMod.Utils;

namespace HKMP_SpectatorMod.MonoBehaviours;

public class SpectatorMode : MonoBehaviour
{ 
    public GameObject Player;
    public bool? isInSpectatorMode = null;

    public void SetVisible()
    {
        if (Player == null) return;
        var material = Player.GetComponent<Renderer>().material;
        material.color = material.color.SetAlpha(1f);
    }
    
    public void SetInVisible()
    {
        if (Player == null) return;
        var material = Player.GetComponent<Renderer>().material;
        material.color = material.color.SetAlpha(HKMP_SpectatorMod.settings.visibility);
    }

    public void LateUpdate()
    {
        if (isInSpectatorMode == null)
        {
            return;
        }
        if (isInSpectatorMode.Value)
        {
            SetInVisible();
        }
        else
        {
            SetVisible();
        }
    }

    public void OnDestroy()
    {
        SetVisible();
    }

    public void OnDisable()
    {
        SetVisible();
    }
}