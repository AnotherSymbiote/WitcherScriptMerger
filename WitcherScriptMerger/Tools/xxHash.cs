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

using System.IO;

namespace WitcherScriptMerger.Tools
{
    public static class Hasher
    {
        public static string ComputeHash(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Can't find file to hash:  " + filePath);

            return new xxHash(filePath).ToString();
        }
    }

    class xxHash
    {
        const uint Seed = 0U,
                   Prime1 = 2654435761U,
                   Prime2 = 2246822519U,
                   Prime3 = 3266489917U,
                   Prime4 = 668265263U,
                   Prime5 = 374761393U;

        const int TransformSize = sizeof(uint) * 4;
        uint _v1,
             _v2,
             _v3,
             _v4;

        string _hex;

        public override string ToString() => _hex;

        public xxHash(string filePath)
        {
            unchecked  // Allow integer overflow
            {
                _v1 = Seed + Prime1 + Prime2;
                _v2 = Seed + Prime2;
                _v3 = Seed;
                _v4 = Seed - Prime1;
            }

            using (var reader = new BinaryReader(File.OpenRead(filePath)))
            {
                // Limit prevents trying to transform by
                // last chunk of input when it's too short
                long streamLength = reader.BaseStream.Length;  // Cache this because it's IO-expensive
                long limit = streamLength - TransformSize;
                while (reader.BaseStream.Position <= limit)
                {
                    TransformBy(reader);
                }

                _hex = string.Format("{0:X}", Finalize(reader, streamLength));
            }
        }

        void TransformBy(BinaryReader reader)
        {
            _v1 += reader.ReadUInt32() * Prime2;
            _v1 = XXH_rotl(_v1, 13);
            _v1 *= Prime1;

            _v2 += reader.ReadUInt32() * Prime2;
            _v2 = XXH_rotl(_v2, 13);
            _v2 *= Prime1;

            _v3 += reader.ReadUInt32() * Prime2;
            _v3 = XXH_rotl(_v3, 13);
            _v3 *= Prime1;

            _v4 += reader.ReadUInt32() * Prime2;
            _v4 = XXH_rotl(_v4, 13);
            _v4 *= Prime1;
        }

        uint Finalize(BinaryReader reader, long streamLength)
        {
            var stream = reader.BaseStream;

            uint hash =
                (streamLength >= 16)
                ? XXH_rotl(_v1, 1) + XXH_rotl(_v2, 7) + XXH_rotl(_v3, 12) + XXH_rotl(_v4, 18)
                : Seed + Prime5;

            hash += (uint)streamLength;

            // Transform hash by any leftover bytes at end of input
            if (stream.Position < streamLength)
            {
                while (stream.Position + sizeof(uint) <= streamLength)
                {
                    hash += reader.ReadUInt32() * Prime3;
                    hash = XXH_rotl(hash, 17) * Prime4;
                }

                while (stream.Position < streamLength)
                {
                    hash += reader.ReadByte() * Prime5;
                    hash = XXH_rotl(hash, 11) * Prime1;
                }
            }

            hash ^= hash >> 15;
            hash *= Prime2;
            hash ^= hash >> 13;
            hash *= Prime3;
            hash ^= hash >> 16;

            return hash;
        }

        // Rotates unsigned 32-bit integer "x" to the left by the number of bits "r"
        static uint XXH_rotl(uint x, int r)
        {
            return (x << r) | (x >> (32 - r));
        }
    }
}
