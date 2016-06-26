/*
Created by Wilhelm Liao on 2015-12-25.
Copyright (c) 2015, Wilhelm Liao
All rights reserved.
Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
- Original xxHash's License follows:
"""
  Copyright (C) 2012-2015, Yann Collet. (https://github.com/Cyan4973/xxHash)
  BSD 2-Clause License (http://www.opensource.org/licenses/bsd-license.php)
  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions are
  met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
 
    * Redistributions in binary form must reproduce the above
      copyright notice, this list of conditions and the following disclaimer
      in the documentation and/or other materials provided with the
      distribution.
  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
  OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
  LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
  You can contact the author at :
  - xxHash source repository : https://github.com/Cyan4973/xxHash
"""
*/

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WitcherScriptMerger.Inventory
{
    [ComVisible(true)]
    [Serializable]
    public sealed class xxHash
    {
        #region Members

        const uint PRIME_1 = 2654435761U,
                   PRIME_2 = 2246822519U,
                   PRIME_3 = 3266489917U,
                   PRIME_4 = 668265263U,
                   PRIME_5 = 374761393U;

        ulong _totalLength;
        uint _seed;
        uint _v1;
        uint _v2;
        uint _v3;
        uint _v4;
        byte[] _buffer = null;
        uint _bufferSize;

        #endregion

        #region Static Methods

        public static string ComputeHashHex(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            return string.Format("{0:X}", ComputeHash(filePath).Value);
        }

        public static uint? ComputeHash(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            var hasher = new xxHash();
            hasher.UpdateWith(filePath);
            return hasher.ComputeHash();
        }

        #endregion

        #region Constructors

        public xxHash()
        {
            Reset(0U);
        }

        public xxHash(uint seed)
        {
            Reset(seed);
        }

        #endregion

        public void Reset(uint seed)
        {
            _seed = seed;
            _v1 = seed + PRIME_1 + PRIME_2;
            _v2 = seed + PRIME_2;
            _v3 = seed;
            _v4 = seed - PRIME_1;
            _totalLength = 0;
            _bufferSize = 0;
            _buffer = new byte[16];
        }

        public void UpdateWith(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                UpdateWith(stream);
            }
        }

        public void UpdateWith(Stream inStream)
        {
            if (inStream == null)
                throw new ArgumentNullException(nameof(inStream));

            byte[] readBytes = new byte[0x1000];
            int readBytesCount;
            do
            {
                readBytesCount = inStream.Read(readBytes, 0, 0x1000);
                if (readBytesCount <= 0)
                    continue;

                var byteStream = new ByteStream(readBytes, 0);

                _totalLength += (ulong)readBytesCount;

                if (_bufferSize + readBytesCount < 16)   /* fill in tmp buffer */
                {
                    Array.Copy(readBytes, 0, _buffer, _bufferSize, readBytesCount);
                    _bufferSize += (uint)readBytesCount;
                    return;
                }

                if (_bufferSize > 0)   /* some data left from previous update */
                {
                    Array.Copy(readBytes, 0, _buffer, _bufferSize, 16 - _bufferSize);

                    {
                        var s = new ByteStream(_buffer, (int)_bufferSize);
                        TransformBy(ref s);
                    }

                    byteStream.Skip(16 - (int)_bufferSize);
                    _bufferSize = 0;
                }

                if (byteStream.Position <= readBytesCount - 16)
                {
                    long limit = readBytesCount - 16;
                    do
                    {
                        TransformBy(ref byteStream);
                    }
                    while (byteStream.Position <= limit);
                }

                if (byteStream.Position < readBytesCount)
                {
                    Array.Copy(readBytes, byteStream.Position, _buffer, 0, readBytesCount - byteStream.Position);
                    _bufferSize = (uint)(readBytesCount - byteStream.Position);
                }
            } while (readBytesCount > 0);
        }

        void TransformBy(ref ByteStream stream)
        {
            _v1 += stream.ReadUInt32() * PRIME_2;
            _v1 = XXH_rotl(_v1, 13);
            _v1 *= PRIME_1;

            _v2 += stream.ReadUInt32() * PRIME_2;
            _v2 = XXH_rotl(_v2, 13);
            _v2 *= PRIME_1;

            _v3 += stream.ReadUInt32() * PRIME_2;
            _v3 = XXH_rotl(_v3, 13);
            _v3 *= PRIME_1;

            _v4 += stream.ReadUInt32() * PRIME_2;
            _v4 = XXH_rotl(_v4, 13);
            _v4 *= PRIME_1;
        }

        public uint ComputeHash()
        {
            var p = new ByteStream(_buffer);
            long bEnd = _bufferSize;
            uint hash;

            hash =
                (_totalLength >= 16)
                ? XXH_rotl(_v1, 1) + XXH_rotl(_v2, 7) + XXH_rotl(_v3, 12) + XXH_rotl(_v4, 18)
                : _seed + PRIME_5;

            hash += (uint)_totalLength;

            while (p.Position + 4 <= bEnd)
            {
                hash += p.ReadUInt32() * PRIME_3;
                hash = XXH_rotl(hash, 17) * PRIME_4;
            }

            while (p.Position < bEnd)
            {
                hash += p.ReadByte() * PRIME_5;
                hash = XXH_rotl(hash, 11) * PRIME_1;
            }

            hash ^= hash >> 15;
            hash *= PRIME_2;
            hash ^= hash >> 13;
            hash *= PRIME_3;
            hash ^= hash >> 16;

            return hash;
        }

        /* Rotates unsigned 32-bits integer "x" to the left by the number of bits specified in the "r" parameter. */
        static uint XXH_rotl(uint x, int r)
        {
            return (x << r) | (x >> (32 - r));
        }
    }

    /****************************************************
     *  Struct provides byte array read operations
    ****************************************************/

    struct ByteStream
    {
        private readonly byte[] _data;
        private int _position;

        // Gets the length in bytes of the stream.
        public int Length =>
            (_data != null) ? _data.Length : 0;

        // Gets the position within the current stream.
        public int Position =>
            _position;

        // Gets a value that indicates whether the current stream position is at the end of the stream.
        public bool EndOfStream =>
            (_data == null || _position >= _data.Length);

        internal ByteStream(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (input.Rank != 1)
                throw new ArgumentException("Multi-dimension array is not supported on this operation.", nameof(input));
            if (input.GetLowerBound(0) != 0)
                throw new ArgumentException("The lower bound of target array must be zero.", nameof(input));

            _data = input;
            _position = 0;
        }

        internal ByteStream(byte[] input, int offset)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (input.Rank != 1)
                throw new ArgumentException("Multi-dimension array is not supported on this operation.", nameof(input));
            if (input.GetLowerBound(0) != 0)
                throw new ArgumentException("The lower bound of target array must be zero.", nameof(input));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "Specified argument must be a non-negative integer.");
            if (input.Length < offset)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset was out of bounds for the array.");

            _data = input;
            _position = offset;
        }

        // Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by
        // four bytes.
        public uint ReadUInt32()
        {
            if (EndOfStream)
                throw new InvalidOperationException();

            uint value = BitConverter.ToUInt32(_data, _position);
            Skip(4);
            return value;
        }

        // Reads the next byte from the current stream and advances the current position of the stream by one 
        // byte.
        public byte ReadByte()
        {
            if (EndOfStream)
                throw new InvalidOperationException();

            return _data[_position++];
        }

        // Skip a number of bytes in the current stream.
        public bool Skip(int skipNumBytes)
        {
            if (skipNumBytes < 0)
                throw new ArgumentOutOfRangeException(nameof(skipNumBytes));

            if (!EndOfStream)
            {
                if (_position + skipNumBytes > _data.Length)
                    _position = _data.Length;
                else
                    _position += skipNumBytes;

                return true;
            }
            return false;
        }
    }
}
