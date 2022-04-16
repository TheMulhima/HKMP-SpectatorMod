using Hkmp.Networking.Packet.Data;

namespace HKMP_SpectatorMod.HKMPNetwork;

public class SpectatorModServer : ServerAddon 
{
        public IServerApi serverApi;

        public override void Initialize(IServerApi _serverApi)
        {
            this.serverApi = _serverApi;
            
            var netReceiver = _serverApi.NetServer.GetNetworkReceiver<ToServerPackets>(this,InstantiatePacket);
            var netSender = _serverApi.NetServer.GetNetworkSender<ToClientPackets>(this);

            netReceiver.RegisterPacketHandler<ToServerPacketData>
            (
                ToServerPackets.SendSpectorMode,
                (id, packetData) => 
                {
                    netSender.BroadcastSingleData(ToClientPackets.BroadcastSpectorMode, new ToClientPacketData()
                    {
                        playerId = id,
                        isInSpectatorMode = packetData.isInSpectatorMode,
                    });

                }
            );
            
            netReceiver.RegisterPacketHandler<ToServerRequest>
            (
                ToServerPackets.RequestSpectatorMode,
                (id, packetData) => 
                {
                    netSender.SendSingleData(ToClientPackets.RequestSpectatorMode, new ReliableEmptyData(), packetData.playerid);
                }
            );
        }

        private static IPacketData InstantiatePacket(ToServerPackets Serverpackets) {
            switch (Serverpackets) 
            {
                case ToServerPackets.SendSpectorMode:
                    return new ToServerPacketData();
                case ToServerPackets.RequestSpectatorMode:
                    return new ToServerRequest();
            }
            return null;
        }

        protected override string Name => "Spectator Mod";
        protected override string Version => "0.0.1";
        public override bool NeedsNetwork => true;
    }