using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Enumerables
{
    /// <summary>
    /// The types of encryptions.
    /// </summary>
    public enum ECryptoType
    {
        HASH_SHA256,
        HASH_SHA1,
        HASH_MD5,
        HASH_CHARPOS,
        ENC_SHIFT,
        ENC_UNP,
        ENC_POINTER,
        ENC_BACKLASH
    }
}
