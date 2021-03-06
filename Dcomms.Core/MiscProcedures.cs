﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Dcomms
{
    public static class MiscProcedures
    {
        public static string BandwidthToString(this float bandwidth)
        {
            var sb = new StringBuilder();
            if (bandwidth > 1024 * 1024)
            {
                sb.AppendFormat("{0:0.00}Mbps", bandwidth / (1024 * 1024));
            }
            else if (bandwidth > 1024)
            {
                sb.AppendFormat("{0:0.00}kbps", bandwidth / (1024));
            }
            else
                sb.AppendFormat("{0:0.00}bps", bandwidth);

            return sb.ToString();
        }
        public static string PpsToString(this float packetsPerSecond)
        {
            var sb = new StringBuilder();
            if (packetsPerSecond > 1024 * 1024)
            {
                sb.AppendFormat("{0:0.00}Mpps", packetsPerSecond / (1024 * 1024));
            }
            else if (packetsPerSecond > 1024)
            {
                sb.AppendFormat("{0:0.00}kpps", packetsPerSecond / (1024));
            }
            else
                sb.AppendFormat("{0:0.00}pps", packetsPerSecond);

            return sb.ToString();
        }
        public static string PacketLossToString(this float loss)
        {
            return String.Format("{0:0.00}%", loss * 100);
        }
               
        public static bool TimeStamp1IsLess(uint t1, uint t2)
        {
            //    t1              t2         result
            //   ffff ffff       ffff fffe   false
            //   0000 0000       ffff ffff   false
            //   ffff ffff       0000 0000   true
            //   0000 0001       0000 0000   false

            if (t1 > 0xCFFFFFFF && t2 < 0x3FFFFFFF)
                return true;
            if (t1 < 0x3FFFFFFF && t2 > 0xCFFFFFFF)
                return false;

            return t1 < t2;
        }

        public static string TimeSpanToString(this TimeSpan? ts)
        {
            if (ts == null) return "N/A";
            if (ts.Value.Ticks < TimeSpan.TicksPerSecond) return String.Format("{0:0.0}ms", ts.Value.TotalMilliseconds);
            else if (ts.Value.Ticks < TimeSpan.TicksPerMinute) return String.Format("{0:0.0}s", ts.Value.TotalSeconds);
            else if (ts.Value.Ticks < TimeSpan.TicksPerHour) return String.Format("{0:0.0}m", ts.Value.TotalMinutes);
            else return String.Format("{0:0.0}h", ts.Value.TotalHours);
        }

       
        public static Color RttToColor(this TimeSpan? rtt)
        {
            if (!rtt.HasValue) return Color.Transparent;
            return ValueToColor((float)rtt.Value.TotalMilliseconds, new[] {
                new Tuple<float, Color>(0, Color.FromArgb(100, 255, 100)),
                new Tuple<float, Color>(200, Color.FromArgb(255, 255, 150)),
                new Tuple<float, Color>(1000, Color.FromArgb(255, 150, 150))
                });
        }

        public static Color BandwidthToColor(this float bandwidth)
        {
            return ValueToColor(bandwidth, new[] {
                new Tuple<float, Color>(0, Color.FromArgb(255, 150, 150)),
                new Tuple<float, Color>(1024 * 1024, Color.FromArgb(255, 255, 100)),
                new Tuple<float, Color>(20 * 1024 * 1024, Color.FromArgb(100, 255, 100)),
                new Tuple<float, Color>(100 * 1024 * 1024, Color.FromArgb(100, 255, 200))
                });
        }
        public static Color PacketLossToColor(this float? packetLoss01)
        {
            if (!packetLoss01.HasValue) return Color.Transparent;
            return ValueToColor(packetLoss01.Value, new[] {
                new Tuple<float, Color>(0, Color.FromArgb(100, 255, 100)),
                new Tuple<float, Color>(0.02f, Color.FromArgb(255, 255, 0)),
                new Tuple<float, Color>(0.1f, Color.FromArgb(255, 150, 150)),
                });
        }

        
        /// <param name="referencePoints">must be sorted by value, ascending</param>
        public static Color ValueToColor(this float value, Tuple<float,Color>[] referencePoints)
        {
            var p1 = referencePoints[0];
            if (value < p1.Item1) return p1.Item2;
            for (int i = 1; i < referencePoints.Length; i++)
            {
                var p2 = referencePoints[i];
                if (value < p2.Item1)
                {
                    var v01 = (value - p1.Item1) / (p2.Item1 - p1.Item1);
                    if (v01 < 0) v01 = 0; else if (v01 > 1) v01 = 1;
                    return Color.FromArgb(ColorComponentSubroutine(v01, p1.Item2.A, p2.Item2.A), ColorComponentSubroutine(v01, p1.Item2.R, p2.Item2.R), ColorComponentSubroutine(v01, p1.Item2.G, p2.Item2.G), ColorComponentSubroutine(v01, p1.Item2.B, p2.Item2.B));
                }
                p1 = p2;
            }
            return p1.Item2;
        }
        static int ColorComponentSubroutine(float v01, int c1, int c2)
        {
            return c1 + (int)(v01 * (c2 - c1));
        }
    }
    public class AverageSingle
    {
        uint _n;
        float _sum;
        public float? Average => _n != 0 ? (float?)(_sum / _n) : null;
        public void Input(float v)
        {
            if (float.IsNaN(v)) return;
            if (float.IsInfinity(v)) return;
            _sum += v;
            _n++;
        }
    }
}
