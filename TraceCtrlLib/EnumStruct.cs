﻿using System;

namespace TraceCtrlLib.PanelExtend
{
    public class TraceChain
    {
        public TracePanel PreviousCtrl { get; set; }
        public TracePanel NextCtrl { get; set; }
        public String Entity { get; set; }
    }

    public enum DoorStyle
    {
        一扇=1,
        二扇=2
    }
}
