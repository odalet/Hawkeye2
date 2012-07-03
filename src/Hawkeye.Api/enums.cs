﻿using System;

namespace Hawkeye
{
    /// <summary>
    /// Enumerates the clr versions supported by Hawkeye
    /// </summary>
    public enum Clr
    {
        /// <summary>Clr could not be determined</summary>
        Unknown,
        /// <summary>
        /// CLR for .NET 2 (matches .NET 2, .NET 3 and .NET 3.5
        /// </summary>
        Net2,
        /// <summary>
        /// CLR for .NET 4 (matches .NET 4 and .NET 4.5 ???)
        /// </summary>
        Net4
    }

    /// <summary>
    /// Bitness of a platform or an application.
    /// </summary>
    public enum Bitness
    {
        /// <summary>Bitness could not be determined</summary>
        Unknown,
        /// <summary>x86 means 32bits</summary>
        x86,
        /// <summary>x64 means 64bits.</summary>
        x64
    }
}
