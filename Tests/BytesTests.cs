using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cyberfuck.Data;

namespace Tests
{
    [TestClass]
    public class BytesTests
    {
        [TestMethod]
        public void TestIntToByte_ByteToInt()
        {
            int[] arr = { 1, 2, 3, 4 };
            byte[] arr2 = new byte[arr.Length * sizeof(int)];
            Buffer.BlockCopy(arr, 0, arr2, 0, arr2.Length);
            int[] arr3 = new int[arr.Length];
            Buffer.BlockCopy(arr2, 0, arr3, 0, arr2.Length);
            Console.WriteLine("{0}, {1}, {2}", arr3[0], arr3[1], arr3[2]);
        }
    }
}
