using System.Linq;

namespace HKMP_SpectatorMod.HKMPNetwork;

public class SpectatorModeCommand : Hkmp.Api.Command.Client.IClientCommand
{
    public string Trigger { get; } = "spectator";
    public string[] Aliases { get; } = new[] 
    { 
        "Spectator",
        "spectators", "Spectators",
        "spectatormode", "SpectatorMode", 
        "/spectator", @"\spectator", "/Spectator", @"\Spectator",
        "/spectatormode", @"\spectatormode", "/SpectatorMode", @"\SpectatorMode",
    };

    public void Execute(string[] arguments)
    {
        if (arguments[1].ToLower() == "on"
            || arguments[1].ToLower() == "true"
            || arguments[1].ToLower() == "yes")
        {
            HKMP_SpectatorMod.Instance.Log("turning on spectator Mode");
            HKMP_SpectatorMod.settings.isInSpectatorMode = true;
            HKMP_SpectatorMod.Instance.clientAddon.SendUpdate();
            if (!ReflectionHelper.GetField<DebugMod.DebugMod, bool>("noclip"))
            {
                DebugMod.BindableFunctions.ToggleNoclip();
            }
            if (!ReflectionHelper.GetField<DebugMod.DebugMod, bool>("playerInvincible"))
            {
                DebugMod.BindableFunctions.ToggleInvincibility();
            }

            var spectatorMode = HeroController.instance.gameObject.GetAddComponent<SpectatorMode>();
            spectatorMode.isInSpectatorMode = true;
            spectatorMode.Player = HeroController.instance.gameObject;
        }
        else if (arguments[1].ToLower() == "off"
             || arguments[1].ToLower() == "false"
             || arguments[1].ToLower() == "no")

        {
            HKMP_SpectatorMod.settings.isInSpectatorMode = false;
            HKMP_SpectatorMod.Instance.clientAddon.SendUpdate();
            if (ReflectionHelper.GetField<DebugMod.DebugMod, bool>("noclip"))
            {
                DebugMod.BindableFunctions.ToggleNoclip();
            }

            if (ReflectionHelper.GetField<DebugMod.DebugMod, bool>("playerInvincible"))
            {
                DebugMod.BindableFunctions.ToggleInvincibility();
            }
            var spectatorMode = HeroController.instance.gameObject.GetAddComponent<SpectatorMode>();
            spectatorMode.isInSpectatorMode = false;
        }
    }
}