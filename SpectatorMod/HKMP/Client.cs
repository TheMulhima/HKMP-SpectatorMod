using Hkmp.Networking.Packet.Data;

namespace HKMP_SpectatorMod.HKMPNetwork;

public class SpectatorModClient : ClientAddon
{
    public ClientAddon clientAddon;
    public IClientApi clientApi;
    
    protected override string Name => "Spectator Mod";
    protected override string Version => "0.0.1";
    public override bool NeedsNetwork => true;

    internal static Dictionary<IClientPlayer, SpectatorMode> SpectatorModeComponentCache =
        new Dictionary<IClientPlayer, SpectatorMode>();

    public override void Initialize(IClientApi _clientApi)
    {
        clientApi = _clientApi;
        clientAddon = this;

        var netReceiver = _clientApi.NetClient.GetNetworkReceiver<ToClientPackets>(clientAddon, InstantiatePacket);
        var netSender = _clientApi.NetClient.GetNetworkSender<ToServerPackets>(clientAddon);

        netReceiver.RegisterPacketHandler<ToClientPacketData>(
            ToClientPackets.BroadcastSpectorMode,
            packetData =>
            {
                var player = clientApi.ClientManager.GetPlayer(packetData.playerId);
                if (player is { IsInLocalScene: true })
                {
                    SpectatorMode spectatorMode;
                    if (SpectatorModeComponentCache.TryGetValue(player, out spectatorMode))
                    {
                        if (spectatorMode == null)
                        {
                            spectatorMode = getAddSpectatorMode(player);
                        }
                    }
                    else
                    {
                        spectatorMode = AddPlayerToCache(player);
                    }
                    
                    spectatorMode.SetVars(player.PlayerObject, packetData.isInSpectatorMode);
                }
            });

        netReceiver.RegisterPacketHandler<ReliableEmptyData>(
            ToClientPackets.RequestSpectatorMode,
            packetData =>
            {
                SendUpdate();
            }
        );

        clientApi.ClientManager.PlayerDisconnectEvent += (player) =>
        {
            if (SpectatorModeComponentCache.TryGetValue(player, out var spectatorMode))
            {
                UnityEngine.Object.Destroy(spectatorMode);
                SpectatorModeComponentCache.Remove(player);
            }
        };

        clientApi.ClientManager.PlayerConnectEvent += (player) =>
        {
            SendRequest(player.Id);
        };
        clientApi.ClientManager.PlayerEnterSceneEvent += (player) =>
        {
            SendRequest(player.Id);
        };

        ModHooks.HeroUpdateHook += () =>
        {
            if (clientApi is { NetClient.IsConnected: true })
            {
                foreach (var player in _clientApi.ClientManager.Players)
                {
                    if (!SpectatorModeComponentCache.TryGetValue(player, out _))
                    {
                        SendRequest(player.Id);
                    }
                }
            }
        };
        
        clientApi.CommandManager.RegisterCommand(new SpectatorModeCommand());
    }

    private SpectatorMode AddPlayerToCache(IClientPlayer player)
    {
        SpectatorMode spectatorMode = getAddSpectatorMode(player);
        
        SpectatorModeComponentCache[player] = spectatorMode;
        return spectatorMode;
    }

    private SpectatorMode getAddSpectatorMode(IClientPlayer player)
    {
        SpectatorMode spectatorMode;
        if (player.PlayerObject.GetComponent<SpectatorMode>() == null)
        {
            spectatorMode = player.PlayerObject.AddComponent<SpectatorMode>();
        }
        else
        {
            spectatorMode = player.PlayerObject.GetComponent<SpectatorMode>();
        }

        return spectatorMode;
    }
    
    public void SendUpdate()
    {
        var netSender = clientApi.NetClient.GetNetworkSender<ToServerPackets>(clientAddon);
        netSender.SendSingleData(ToServerPackets.SendSpectorMode, new ToServerPacketData
        {
            isInSpectatorMode = HKMP_SpectatorMod.settings.isInSpectatorMode,
        });
    }
    
    public void SendRequest(ushort playerId)
    {
        var netSender = clientApi.NetClient.GetNetworkSender<ToServerPackets>(clientAddon);
        netSender.SendSingleData(ToServerPackets.RequestSpectatorMode, new ToServerRequest
        {
            playerid =  playerId,
        });
    }
    
    private static IPacketData InstantiatePacket(ToClientPackets Clientpackets) {
        switch (Clientpackets) 
        {
            case ToClientPackets.BroadcastSpectorMode:
                return new ToClientPacketData();
            case ToClientPackets.RequestSpectatorMode:
                return new ReliableEmptyData();
        }
        return null;
    }
}


