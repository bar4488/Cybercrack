using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Network
{
    public abstract class NetBase : INetBase
    {
        public void SendMessage(MessageType messageType, int player, object data)
        {
            byte[] msg = ProcessMessage(messageType, player, data);
            SendBuffer(msg, player);
        }
        public byte[] ProcessMessage(MessageType messageType, int player, object data)
        {
            int messageLength = 0;
            byte[] msgType = BitConverter.GetBytes((int)messageType);
            messageLength += msgType.Length;
            byte[] msgPlayer = BitConverter.GetBytes(player);
            messageLength += msgPlayer.Length;
            byte[] msgContent;
            switch (messageType)
            {
                case MessageType.PlayerUpdate:
                    messageLength += GetEntityDataBytes(out msgContent, (EntityData)data);
                    break;
                default:
                    return null;
            }
            byte[] msg = new byte[messageLength];
            Buffer.BlockCopy(msgType, 0, msg, 0, msgType.Length);
            Buffer.BlockCopy(msgPlayer, 0, msg, msgType.Length, msgPlayer.Length);
            Buffer.BlockCopy(msgContent, 0, msg, msgType.Length + msgPlayer.Length, msgContent.Length);
            return msg;
        }

        public abstract void SendBuffer(byte[] msg, int player);

        public int GetEntityDataBytes(out byte[] msg, EntityData entityData)
        {
            int posX = entityData.Position.X;
            int posY = entityData.Position.Y;
            int velX = entityData.Velocity.X;
            int velY = entityData.Velocity.Y;
            int type = (int)entityData.Type;
            int[] dataArr = new int[] { type, posX, posY, velX, velY};
            msg = new byte[dataArr.Length * sizeof(int)];
            Buffer.BlockCopy(dataArr, 0, msg, 0, msg.Length);
            return msg.Length;
        }
        
        public abstract void Close();

        public abstract void Update();
    }
}
