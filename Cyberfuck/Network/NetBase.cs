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
            byte[] msg = ProcessMessage(messageType, data);
            SendBuffer(msg, player);
        }
        public byte[] ProcessMessage(MessageType type, object data)
        {
            byte[] msgContent;
            switch (type)
            {
                case MessageType.PlayerUpdate:
                    msgContent = GetPlayerDataBytes((PlayerData)data);
                    break;
                case MessageType.EntityData:
                    msgContent = GetEntityDataBytes((EntityData)data);
                    break;
                default:
                    return null;
            }
            return WrapWithHeader(msgContent, type);
        }
        public static byte[] WrapWithHeader(byte[] msgContent, MessageType type)
        {
            int messageLength = msgContent.Length;
            byte[] msgType = BitConverter.GetBytes((int)type);
            messageLength += msgType.Length + sizeof(int);
            byte[] msgLength = BitConverter.GetBytes(messageLength);
            return ByteManipulation.ConcatByteArrays(msgLength, msgType, msgContent);
        }

        public abstract void SendBuffer(byte[] msg, int player);

        public byte[] GetPlayerDataBytes(PlayerData playerData)
        {
            return ByteManipulation.WrapWithInt(GetEntityDataBytes(playerData.Entity), playerData.ID);
        }
        public byte[] GetEntityDataBytes(EntityData entityData)
        {
            int type = (int)entityData.Type;
            int posX = entityData.Position.X;
            int posY = entityData.Position.Y;
            int velX = entityData.Velocity.X;
            int velY = entityData.Velocity.Y;
            int[] dataArr = new int[] { type, posX, posY, velX, velY};
            byte[] msg = new byte[dataArr.Length * sizeof(int)];
            Buffer.BlockCopy(dataArr, 0, msg, 0, msg.Length);
            return msg;
        }
        
        public abstract void Close();

        public abstract void Update();
    }
}
