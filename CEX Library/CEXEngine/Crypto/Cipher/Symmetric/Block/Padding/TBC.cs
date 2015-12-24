﻿#region Directives
using System;
#endregion

namespace VTDev.Libraries.CEXEngine.Crypto.Cipher.Symmetric.Block.Padding
{
    /// <summary>
    /// <h3>The Trailing Bit Compliment Padding Scheme.</h3>
    /// </summary>
    public sealed class TBC : IPadding
    {
        #region Constants
        private const string ALG_NAME = "TBC";
        private const byte ZBCODE = (byte)0x00;
        private const byte MKCODE = (byte)0xff;
        #endregion

        #region Properties
        /// <summary>
        /// Get: Padding name
        /// </summary>
        public string Name
        {
            get { return ALG_NAME; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add padding to input array
        /// </summary>
        /// 
        /// <param name="Input">Array to modify</param>
        /// <param name="Offset">Offset into array</param>
        /// 
        /// <returns>Length of padding</returns>
        public int AddPadding(byte[] Input, int Offset)
        {
            int olen = (Offset > 0) ? Offset - 1 : 0;
	        int plen = Input.Length - Offset;
	        byte code;

	        if ((Input[olen] & 0x01) == 0)
		        code = MKCODE;
	        else
		        code = ZBCODE;

            while (Offset < Input.Length)
		        Input[Offset++] = code;

	        return plen;
        }

        /// <summary>
        /// Get the length of padding in an array
        /// </summary>
        /// 
        /// <param name="Input">Padded array of bytes</param>
        /// 
        /// <returns>Length of padding</returns>
        public int GetPaddingLength(byte[] Input)
        {
            int plen = Input.Length;
	        byte code = Input[plen - 1];

	        while (plen != 0 && Input[plen - 1] == code)
		        plen--;

            return Input.Length - plen;
        }

        /// <summary>
        /// Get the length of padding in an array
        /// </summary>
        /// 
        /// <param name="Input">Padded array of bytes</param>
        /// <param name="Offset">Offset into array</param>
        /// 
        /// <returns>Length of padding</returns>
        public int GetPaddingLength(byte[] Input, int Offset)
        {
            int plen = Input.Length - Offset;
            byte code = Input[Input.Length - 1];

	        while (plen != 0 && Input[Offset + (plen - 1)] == code)
		        plen--;

            return (Input.Length - Offset) - plen;
        }
        #endregion
    }
}
