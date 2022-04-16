using Hkmp.Networking.Packet;
using Vector2 = Hkmp.Math.Vector2;

namespace HKMP_SpectatorMod.HKMPNetwork;

public class ToClientPacketData : IPacketData
{
    public bool IsReliable => true;
    public bool DropReliableDataIfNewerExists => true;

    public bool isInSpectatorMode { get; set; }
    public ushort playerId { get; set; }

    public void WriteData(IPacket packet)
    {
        packet.Write(playerId);
        packet.Write(isInSpectatorMode);
    }

    public void ReadData(IPacket packet)
    {
        playerId = packet.ReadUShort();
        isInSpectatorMode = packet.ReadBool();
    }
}

public class ToServerPacketData : IPacketData
{
    public bool IsReliable => true;
    public bool DropReliableDataIfNewerExists => true;

    public bool isInSpectatorMode { get; set; }

    public void WriteData(IPacket packet)
    {
        packet.Write(isInSpectatorMode);
    }

    public void ReadData(IPacket packet)
    {
        isInSpectatorMode = packet.ReadBool();
    }
}
public class ToServerRequest : IPacketData
{
    public bool IsReliable => true;
    public bool DropReliableDataIfNewerExists => true;

    public ushort playerid { get; set; }

    public void WriteData(IPacket packet)
    {
        packet.Write(playerid);
    }

    public void ReadData(IPacket packet)
    {
        playerid = packet.ReadUShort();
    }
}

public enum ToServerPackets
{
    SendSpectorMode,
    RequestSpectatorMode
}
public enum ToClientPackets
{ 
    BroadcastSpectorMode,
    RequestSpectatorMode
}

