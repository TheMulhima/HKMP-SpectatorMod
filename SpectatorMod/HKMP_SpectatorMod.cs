using System.Linq;

namespace HKMP_SpectatorMod;

public class HKMP_SpectatorMod:Mod, IGlobalSettings<GlobalSettings>, ICustomMenuMod
{
    public SpectatorModClient clientAddon = new SpectatorModClient();
    public SpectatorModServer serverAddon = new SpectatorModServer();
    private Menu menuRef;

    public static HKMP_SpectatorMod Instance;
    public static GlobalSettings settings { get; set; } = new ();
    public void OnLoadGlobal(GlobalSettings s) => settings = s;
    public GlobalSettings OnSaveGlobal() => settings;

    public override string GetVersion() => AssemblyUtils.GetAssemblyVersionHash();

    public override void Initialize()
    {
        Instance ??= this;
        ClientAddon.RegisterAddon(clientAddon);
        ServerAddon.RegisterAddon(serverAddon);
        On.QuitToMenu.Start += (orig, self) =>
        {
            settings.isInSpectatorMode = false;
            return orig(self);
        };
    }
    
    public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
    {
        menuRef ??= new Menu("Spectator Mode", new Element[]
        {
            new HorizontalOption("Visibility",
                "How visible do you want to be when in spectator mode", 
                Enumerable.Range((int)(0 / 0.05f), (int) (1f / 0.05f) + 1)
                    .Select(x => (x * 0.05f).ToString()).ToArray(),
                i =>
                {
                    settings.visibility = i * 0.05f;
                },
                () => (int)(settings.visibility / 0.05f))
        });
        
        return menuRef.GetMenuScreen(modListMenu);
    }

    public bool ToggleButtonInsideMenu { get; }
}