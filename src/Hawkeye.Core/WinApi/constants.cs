using System;

namespace Hawkeye.WinApi
{
    // Constants & flags
    [Flags]
    internal enum SnapshotFlags : uint
    {
        HeapList = 0x00000001,
        Process = 0x00000002,
        Thread = 0x00000004,
        Module = 0x00000008,
        Module32 = 0x00000010,
        Inherit = 0x80000000,
        All = 0x0000001F
    }

    // RedrawWindow flags
    [Flags]
    internal enum RDW : uint
    {
        RDW_INVALIDATE = 0x0001,
        RDW_INTERNALPAINT = 0x0002,
        RDW_ERASE = 0x0004,

        RDW_VALIDATE = 0x0008,
        RDW_NOINTERNALPAINT = 0x0010,
        RDW_NOERASE = 0x0020,

        RDW_NOCHILDREN = 0x0040,
        RDW_ALLCHILDREN = 0x0080,

        RDW_UPDATENOW = 0x0100,
        RDW_ERASENOW = 0x0200,

        RDW_FRAME = 0x0400,
        RDW_NOFRAME = 0x0800
    }

    internal static class WindowMessages
    {
        public const uint WM_CLOSE = 0x0010;
        public const uint WM_CANCELMODE = 0x001F;
        public const uint WM_ENABLE = 0x000A;
    }
}
