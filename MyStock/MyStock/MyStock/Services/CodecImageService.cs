using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyStock.Services
{
    public class CodecImageService
    {
        public static byte[] ReadFully(Stream _input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                _input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
