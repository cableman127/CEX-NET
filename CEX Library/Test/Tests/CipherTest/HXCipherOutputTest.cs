﻿#region Directives
using VTDev.Libraries.CEXEngine.Crypto.Cipher.Symmetric.Block;
using VTDev.Libraries.CEXEngine.Crypto.Cipher.Symmetric.Block.Mode;
using VTDev.Libraries.CEXEngine.Crypto.Common;
using VTDev.Libraries.CEXEngine.Crypto.Enumeration;
using VTDev.Libraries.CEXEngine.Tools;
using System;
#endregion

namespace VTDev.Projects.CEX.Test.Tests.CipherTest
{
    /// <summary>
    /// Outputs expected values for the HX Ciphers
    /// </summary>
    public class HXCipherOutputTest
    {
        #region Public Methods
        /// <summary>
        /// Outputs expected values for the HX Ciphers
        /// </summary>
        /// 
        /// <returns>State</returns>
        public string GetVector(BlockCiphers EngineType, Digests DigestType, RoundCounts Rounds)
        {
            IBlockCipher engine = GetCipher(EngineType, DigestType, Rounds);
            int keyLen = GetKeySize(engine);
            byte[] key = new byte[keyLen];
            byte[] iv = new byte[engine.BlockSize];
            ICipherMode cipher = new CTR(engine);

            for (int i = 0; i < keyLen; i++)
                key[i] = (byte)i;
            for (int i = 0; i < iv.Length; i++)
                iv[i] = (byte)i;

            cipher.Initialize(true, new KeyParams(key, iv));

            return MonteCarloTest(cipher);
        }
        #endregion

        #region Private
        private int GetKeySize(IBlockCipher Engine)
        {
            if (Engine.Name == "RHX")
                return ((RHX)Engine).LegalKeySizes[0];
            else if (Engine.Name == "SHX")
                return ((SHX)Engine).LegalKeySizes[0];
            else if (Engine.Name == "THX")
                return ((THX)Engine).LegalKeySizes[0];
            else
                throw new Exception();
        }

        private IBlockCipher GetCipher(BlockCiphers EngineType, Digests DigestType, RoundCounts Rounds)
        {
            switch (EngineType)
            {
                case BlockCiphers.RHX:
                    return new RHX((int)Rounds, 16, DigestType);
                case BlockCiphers.SHX:
                    return new SHX((int)Rounds, DigestType);
                case BlockCiphers.THX:
                    return new THX((int)Rounds, DigestType);
            }
            throw new Exception();
        }

        private string MonteCarloTest(ICipherMode Engine)
        {
            byte[] outBytes = new byte[Engine.BlockSize];

            for (int i = 0; i < 100; i++)
                Engine.Transform(outBytes, outBytes);

            return HexConverter.ToString(outBytes);
        }
        #endregion
    }
}