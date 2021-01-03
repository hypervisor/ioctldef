using System;

namespace ioctldef
{
    internal static class Program
    {
        private enum IoctlMethod
        {
            METHOD_BUFFERED = 0,
            METHOD_IN_DIRECT = 1,
            METHOD_OUT_DIRECT = 2,
            METHOD_NEITHER = 3
        }

        private enum IoctlAccess
        {
            FILE_ANY_ACCESS = 0,
            FILE_READ_ACCESS = 1,
            FILE_WRITE_ACCESS = 2,
            INVALID_ACCESS = 3
        }

        public static void Main(string[] args)
        {
            var codeString = string.Empty;
            if (args.Length != 1)
            {
                Console.Write("IOCTL: ");
                codeString = Console.ReadLine();
            } else
            {
                codeString = args[0];
            }

            if (!uint.TryParse(codeString, System.Globalization.NumberStyles.HexNumber, null, out uint codeValue))
            {
                Console.WriteLine("Failed to parse IOCTL!");
                Console.ReadLine();

                return;
            }

            uint transferType = codeValue & 3; // First two bits, also known as "Method"
            uint functionCode = (codeValue >> 2) & 0x3FF; // 10 bits, 2 from the right
            uint accessType = (codeValue >> 14) & 3; // 2 bits, 14 from the right
            uint deviceType = (codeValue & 0xFFFF0000) >> 16; // 16 bits (upper half)

            Console.WriteLine($"#define IOCTL_{codeString} CTL_CODE(0x{deviceType.ToString("X")}, 0x{functionCode.ToString("X")}, {((IoctlMethod)transferType).ToString()}, {((IoctlAccess)accessType).ToString()})");
            Console.ReadLine();
        }
    }
}
