using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberfuck.Network;

namespace Cyberfuck.Data
{
    enum EventType
    {
        Kill,
    }
    struct PlayerEventData: IMessageContent
    {
        public EventType type;
        public int player;
        public MessageContentType ContentType => MessageContentType.PlayerEvent;

        public PlayerEventData(int player, EventType type)
        {
            this.player = player;
            this.type = type;
        }
        public byte[] Encode()
        {
            return ByteManipulation.ConvertIntsToBytes(player, (int)type);
        }

        public static PlayerEventData Decode(byte[] bytes)
        {
            int[] p = ByteManipulation.ConvertBytesToInts(bytes);
            return new PlayerEventData(p[0], (EventType)p[1]);
        }
    }
}
