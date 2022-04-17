using JetBrains.Annotations;
using UnityEngine.Serialization;
using Logger = Modding.Logger;
using HKMP_SpectatorMod.Utils;

namespace HKMP_SpectatorMod.MonoBehaviours;

public class SpectatorMode : MonoBehaviour
{ 
    public GameObject Player;
    public bool? isInSpectatorMode = null;

    public void SetInVisible()
    {
        if (Player == null) return;
        Material material = Player.GetComponent<Renderer>().material;
        material.color = material.color.SetAlpha(HKMP_SpectatorMod.settings.visibility);
    }

    public void SetVars(GameObject player, bool spectator)
    {
        Player = player;
        isInSpectatorMode = spectator;
        if (spectator)
        {
            Player.layer = (int)GlobalEnums.PhysLayers.GRASS;
        }
        else
        {
            if (Player.GetComponent<DamageHero>().enabled)
            {
                Player.layer = (int)GlobalEnums.PhysLayers.ENEMIES;
            }
            else
            {
                Player.layer = (int)GlobalEnums.PhysLayers.PLAYER;
            }
        }
    }
    
    public void LateUpdate()
    {
        if (isInSpectatorMode != null && isInSpectatorMode.Value)
        {
            SetInVisible();
        }
    }

    public void OnDestroy()
    {
        if (Player == null) return;
        if (Player.GetComponent<DamageHero>().enabled)
        {
            Player.layer = (int)GlobalEnums.PhysLayers.ENEMIES;
        }
        else
        {
            Player.layer = (int)GlobalEnums.PhysLayers.PLAYER;
        }
    }

    public void OnDisable()
    {
        if (Player == null) return;
        if (Player.GetComponent<DamageHero>().enabled)
        {
            Player.layer = (int)GlobalEnums.PhysLayers.ENEMIES;
        }
        else
        {
            Player.layer = (int)GlobalEnums.PhysLayers.PLAYER;
        }
    }
}
