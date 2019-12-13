using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Cyberfuck.Data;

namespace Cyberfuck.Network
{
    public enum MessageError: byte
    {
        Success,
        Empty,
        Error
    }
    public class NetworkMessage
    {
        int contentLength;
        MessageContentType type;
        byte[] content;

        public int ContentLength { get => contentLength; private set => contentLength = value; }
        public MessageContentType Type { get => type; private set => type = value; }
        public byte[] Content { get => content; private set => content = value; }

        private NetworkMessage() { }
        public NetworkMessage(IMessageContent content)
        {
            this.Type = content.ContentType;
            this.Content = content.Encode();
            this.ContentLength = this.Content.Length;
        }
        public NetworkMessage(byte[] content, MessageContentType type, int contentLength = 0)
        {
            this.ContentLength = content.Length;
            this.Type = type;
            this.Content = content;
            this.ContentLength = contentLength;
        }
        public byte[] Encode() { return Encode(this); }
        public static byte[] Encode(NetworkMessage message)
        {
            byte[] start = new byte[] { 69 };
            byte[] msgType = BitConverter.GetBytes((int)message.Type);
            byte[] bytesContentLength = BitConverter.GetBytes(message.ContentLength);
            return ByteManipulation.ConcatByteArrays(start, bytesContentLength, msgType, message.Content);
        }
        public static byte[] EmptyMessage(MessageContentType type)
        {
            byte[] b = new byte[0];
            return WrapWithHeader(b, type);
        }
        public static byte[] WrapWithHeader(byte[] msgContent, MessageContentType type)
        {
            int messageLength = msgContent.Length;
            byte[] msgType = BitConverter.GetBytes((int)type);
            messageLength += msgType.Length + sizeof(int);
            byte[] msgLength = BitConverter.GetBytes(messageLength);
            return ByteManipulation.ConcatByteArrays(msgLength, msgType, msgContent);
        }

        public static MessageError TryDecodeFromStream(out NetworkMessage message, NetworkStream stream)
        {
            if (!stream.DataAvailable)
            {
                message = null;
                return MessageError.Empty;
            }
            if(!(stream.ReadByte() == 69))
            {
                message = null;
                return MessageError.Error;
            }
            message = new NetworkMessage();
            byte[] bytesContentLength = new byte[sizeof(int)];
            stream.Read(bytesContentLength, 0, bytesContentLength.Length);
            message.ContentLength = BitConverter.ToInt32(bytesContentLength, 0);

            byte[] msgType = new byte[sizeof(MessageContentType)];
            stream.Read(msgType, 0, msgType.Length);
            message.Type = (MessageContentType)BitConverter.ToInt32(msgType, 0);

            message.Content = new byte[message.ContentLength];
            stream.Read(message.Content, 0, message.ContentLength);
            return MessageError.Success;
        }
    }
}
