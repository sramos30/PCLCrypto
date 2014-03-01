﻿namespace PCLCrypto.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PCLTesting;
    using Validation;

    /// <summary>
    /// Ensures that our CryptoStream tests pass on the official
    /// .NET Framework's version of CryptoStream as well.
    /// </summary>
    [TestClass]
    public class DesktopCryptoStreamTests : CryptoStreamTests
    {
        protected override Stream CreateCryptoStream(Stream target, PCLCrypto.ICryptoTransform transform, PCLCrypto.CryptoStreamMode mode)
        {
            return new CryptoStream(target, new CryptoTransformAdapter(transform), ModeAdapter(mode));
        }

        protected override void FlushFinalBlock(Stream stream)
        {
            ((CryptoStream)stream).FlushFinalBlock();
        }

        private static CryptoStreamMode ModeAdapter(PCLCrypto.CryptoStreamMode mode)
        {
            switch (mode)
            {
                case PCLCrypto.CryptoStreamMode.Read:
                    return CryptoStreamMode.Read;
                case PCLCrypto.CryptoStreamMode.Write:
                    return CryptoStreamMode.Write;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private class CryptoTransformAdapter : ICryptoTransform
        {
            private readonly PCLCrypto.ICryptoTransform transform;

            internal CryptoTransformAdapter(PCLCrypto.ICryptoTransform transform)
            {
                Requires.NotNull(transform, "transform");

                this.transform = transform;
            }

            public bool CanReuseTransform
            {
                get { return this.transform.CanReuseTransform; }
            }

            public bool CanTransformMultipleBlocks
            {
                get { return this.transform.CanTransformMultipleBlocks; }
            }

            public int InputBlockSize
            {
                get { return this.transform.InputBlockSize; }
            }

            public int OutputBlockSize
            {
                get { return this.transform.OutputBlockSize; }
            }

            public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
            {
                return this.transform.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
            }

            public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
            {
                return this.transform.TransformFinalBlock(inputBuffer, inputOffset, inputCount);
            }

            public void Dispose()
            {
                this.transform.Dispose();
            }
        }
    }
}
