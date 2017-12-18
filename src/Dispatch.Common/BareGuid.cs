//Created by Clone Commando: https://github.com/Clone-Commando

//FEATURES:
//Low memory usage
//Least looping as possible to be heckin' CPU effecient
//Parsing
//Custom serialization for maximum data compression
//Method documentation
//XTREME COMMENTING

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Dispatch.Common
{
    [Serializable]
    public class BareGuid
    {
        private byte[] Bytes;

        #region Constructors
        private BareGuid() =>
            Bytes = new byte[16];

        /// <summary>
        /// Creates a new BareGuild from an array of 16 bytes.
        /// </summary>
        /// <param name="bytes">The byte array to create the BareGuild from</param>
        public BareGuid(byte[] bytes) =>
            Bytes = FromBytes(bytes).Bytes;
        #endregion

        #region Creation/getting object
        /// <summary>
        /// Represents a BareGuild with no value; all 16 bytes empty.
        /// </summary>
        public static readonly BareGuid Empty = FromBytes(new byte[16]);
        /// <summary>
        /// Create a random new BareGuid.
        /// </summary>
        /// <returns>a random BareGuid.</returns>
        public static BareGuid NewBareGuid()
        {
            Random rng = new Random();
            byte[] bytes = new byte[16];
            rng.NextBytes(bytes);

            return FromBytes(bytes);
        }
        /// <summary>
        /// Create a random new BareGuid that does not exist in an enumerable.
        /// </summary>
        /// <param name="existing">The existing BareGuilds not to create</param>
        /// <returns>a unique random BareGuild.</returns>
        public static BareGuid NewBareGuid(IEnumerable<BareGuid> existing)
        {
            if (existing == null)
                throw new ArgumentNullException(nameof(existing));

            BareGuid guid = null;
            IEnumerable<BareGuid> betterGuids = existing as BareGuid[] ?? existing.ToArray();
            while (guid == null || betterGuids.Contains(guid))
                guid = NewBareGuid();

            return guid;
        }

        private static BareGuid FromBytes(byte[] bytes)
        {
            if (bytes.Length != 16)
                throw new ArgumentOutOfRangeException(nameof(bytes), "There must be exactly 16 bytes to convert bytes to a BareGuild.");

            return new BareGuid
            {
                Bytes = bytes
            };
        }
        #endregion

        #region Parsing
        /// <summary>
        /// Create a BareGuild from a string.
        /// </summary>
        /// <param name="str">String representing the BareGuild.</param>
        /// <returns>a BareGuild represented by the string.</returns>
        public static BareGuid Parse(string str)
        {
            char[] chars = str.ToCharArray();
            string hex = new string(new char[]
            {
                chars[0], chars[1], chars[2], chars[3],
                chars[5], chars[6], chars[7], chars[8],
                chars[10], chars[11], chars[12], chars[13],
                chars[15], chars[16], chars[17], chars[18],
                chars[20], chars[21], chars[22], chars[23],
                chars[25], chars[26], chars[27], chars[28],
                chars[30], chars[31], chars[32], chars[33],
                chars[35], chars[36], chars[37], chars[38]
            });

            byte[] bytes = new byte[16];

            for (int i = 0; i < hex.Length; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return FromBytes(bytes);
        }
        /// <summary>
        /// Try to create a BareGuild from a string.
        /// </summary>
        /// <param name="str">String representing the BareGuild.</param>
        /// <param name="guid">The parsed BareGuild if successful.</param>
        /// <returns>a representation of if the parsing was successful.</returns>
        public static bool TryParse(string str, out BareGuid guid)
        {
            try
            {
                guid = Parse(str);
                return true;
            }
            catch
            {
                guid = null;
                return false;
            }
        }
        #endregion

        #region Overrides and operators
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            try
            {
                return this == (BareGuid)obj;
            }
            catch
            {
                return false;
            }
        }
        public override int GetHashCode() =>
            Bytes[0] + Bytes[1] + Bytes[2] + Bytes[3] + Bytes[4] + Bytes[5] + Bytes[6] + Bytes[7];
        public override string ToString()
        {
            char[] chars = BitConverter.ToString(Bytes).ToCharArray();
            string str = new string(new char[]
            {
                chars[0], chars[1],
                chars[3], chars[4],
                chars[6], chars[7],
                chars[9], chars[10],
                chars[12], chars[13],
                chars[15], chars[16],
                chars[18], chars[19],
                chars[21], chars[22],
                chars[24], chars[25],
                chars[27], chars[28],
                chars[30], chars[31],
                chars[33], chars[34],
                chars[36], chars[37],
                chars[39], chars[40],
                chars[42], chars[43],
                chars[45], chars[46]
            });

            str = str.Insert(4, "-")
                .Insert(9, "-")
                .Insert(14, "-")
                .Insert(19, "-")
                .Insert(24, "-")
                .Insert(29, "-")
                .Insert(34, "-");

            return str;
        }

        public static bool operator ==(BareGuid guid1, BareGuid guid2)
        {
            if (ReferenceEquals(guid1, null))
                throw new ArgumentNullException(nameof(guid1));

            if (ReferenceEquals(guid2, null))
                throw new ArgumentNullException(nameof(guid2));

            bool equals = true;
            for (int i = 0; i < 16; i++)
                equals = equals && guid1.Bytes[i] == guid2.Bytes[i];

            return equals;
        }
        public static bool operator !=(BareGuid guid1, BareGuid guid2) =>
            !(guid1 == guid2);
    }
    #endregion
}