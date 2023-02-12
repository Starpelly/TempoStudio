using System;

namespace TempoStudio
{
    public class Decibels
    {
        private const double LOG_TO_DB = 8.685889638065037;

        private const double DB_TO_LOG = 0.11512925464970228;

        public static double LinearToDecibels(double lin)
        {
            return Math.Log(lin) * LOG_TO_DB;
        }

        public static double DecibelsToLinear(double dB)
        {
            return Math.Exp(dB * DB_TO_LOG);
        }
    }
}