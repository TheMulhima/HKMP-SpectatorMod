namespace HKMP_SpectatorMod.HKMPNetwork;

public class SpectatorModeCommand : Hkmp.Api.Command.Client.IClientCommand
{
    public string Trigger { get; } = "spectator";
    public string[] Aliases { get; }
    public void Execute(string[] arguments)
    {
        if (arguments[0].ToLower() == "on")
        {
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
        }
        else if (arguments[0].ToLower() == "off")
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
        }
    }
}