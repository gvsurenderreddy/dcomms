﻿using Dcomms.P2PTP.LocalLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcomms.SUBT
{
    internal static class SubtLogicConfiguration
    {
        internal const uint JitterBufferLengthTicks = (uint)(TimeSpan.TicksPerMillisecond * 200);
        internal const double RecentPacketLossDecayTimeTicks = TimeSpan.TicksPerMillisecond * 700; // is feed back to sender with delay = RTT + JB length
        internal const double RecenRxBandwidthDecayTimeTicks = TimeSpan.TicksPerMillisecond * 700; // is feed back to sender with delay = RTT + JB length
        internal const double RecentTxBandwidthDecayTimeTicks = TimeSpan.TicksPerMillisecond * 700; // self-test at sender side
        internal const uint RxMeasurementsTransmissionIntervalTicks = (uint)(TimeSpan.TicksPerMillisecond * 100);
        internal const long MeasurementsIntervalTicks = TimeSpan.TicksPerSecond * 5;
        internal const long MeasurementInitializationTimeTicks = TimeSpan.TicksPerSecond * 10;


        internal const float BandwidthForStreams_UserInitial = 1024 * 100;

        internal static readonly TimeSpan MaxPeerIdleTime_TxPayload = LocalLogicConfiguration.SendHelloRequestPeriod + TimeSpan.FromSeconds(3);
        internal const int MaxMeasurementsCount = 100000;

        internal const float PerStreamSoftTxBandwidthLimit = 1024 * 1024 * 10;
        internal const float PerStreamHardTxBandwidthLimit = 1024 * 1024 * 20;
    }
}
