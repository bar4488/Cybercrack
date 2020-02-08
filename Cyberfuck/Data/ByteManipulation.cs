using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyberfuck.Data
{
    public static class ByteManipulation
    {
        public static byte[] WrapWithInt(byte[] msgContent, int number)
        {
            byte[] numberBytes = BitConverter.GetBytes(number);
            return ConcatByteArrays(numberBytes, msgContent);
        }

        public static byte[] ConcatByteArrays(params byte[][] bytesArray)
        {
            int length = 0;
            for (int i = 0; i < bytesArray.Length; i++)
            {
                length += bytesArray[i].Length;
            }
            byte[] finalBytes = new byte[length];
            int offset = 0;
            for (int i = 0; i < bytesArray.Length; i++)
            {
                Buffer.BlockCopy(bytesArray[i], 0, finalBytes, offset, bytesArray[i].Length);
                offset += bytesArray[i].Length;
            }
            return finalBytes;
        }
        public static int[] ConvertBytesToInts(byte[] arr)
        {
            int count = arr.Length / sizeof(int);
            return ConvertBytesToInts(arr, 0, count);
        }
        public static int[] ConvertBytesToInts(byte[] arr, int offset, int count)
        {
            int[] result = new int[count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToInt32(arr, offset + i * sizeof(int));
            }
            return result;
        }
        public static float[] ConvertBytesToFloats(byte[] arr)
        {
            int count = arr.Length / sizeof(float);
            return ConvertBytesToFloats(arr, 0, count);
        }

        public static float[] ConvertBytesToFloats(byte[] arr, int offset, int count)
        {
            float[] result = new float[count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToSingle(arr, offset + i * sizeof(float));
            }
            return result;
        }

        public static byte[] ConvertIntsToBytes(params int[] arr)
        {
            byte[] data = new byte[arr.Length * sizeof(int)];
            Buffer.BlockCopy(arr, 0, data, 0, data.Length);
            return data;
        }
        public static byte[] ConvertFloatsToBytes(params float[] arr)
        {
            byte[] data = new byte[arr.Length * sizeof(float)];
            Buffer.BlockCopy(arr, 0, data, 0, data.Length);
            return data;
        }
    }
}
