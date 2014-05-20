#region Header
/**
 * JsonRoundTripTest.cs
 *   Tests using the JsonMapper to do round-trip testing, convert to json and back again, check equal.
 *
 * The authors disclaim copyright to this source code. For more details, see
 * the COPYING file included with this distribution.
 **/
#endregion


using LitJson;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

// Q: should we handle non-numeric values correctly, like Infinity and NaN? (A: yes)

namespace LitJson.Test
{
    [TestFixture]
    public class RoundTripTest
    {
        [Test]
        public void RoundTripInt8()
        {
            RoundTrip<Byte>(0, 1, Byte.MaxValue, Byte.MinValue);
        }

        [Test]
        public void RoundTripInt16()
        {
            RoundTrip<Int16>(0, 1, Int16.MaxValue, Int16.MinValue);
        }

        [Test]
        public void RoundTripInt32()
        {
            RoundTrip<Int32>(0, 1, Int16.MaxValue, Int16.MinValue, Int32.MaxValue, Int32.MinValue);
        }


        [Test]
        public void RoundTripInt64()
        {
            RoundTrip<Int64>(0, 1, Int16.MaxValue, Int16.MinValue, Int32.MaxValue, Int32.MinValue, Int64.MaxValue, Int64.MinValue);
        }

        [Test]
        public void RoundTripUInt16()
        {
            RoundTrip<UInt16>(0, 1, UInt16.MaxValue, UInt16.MinValue);
        }

        [Test]
        public void RoundTripUInt32()
        {
            RoundTrip<UInt32>(0, 1, UInt16.MaxValue, UInt16.MinValue, UInt32.MaxValue, UInt32.MinValue);
        }


        [Test]
        public void RoundTripUInt64()
        {
            RoundTrip<UInt64>(0, 1, UInt16.MaxValue, UInt16.MinValue, UInt32.MaxValue, UInt32.MinValue, UInt64.MaxValue, UInt64.MinValue);
        }

        [Test]
        public void RoundTripFloatSingle()
        {
            RoundTrip<Single>(CompareSingle, true, 0.0f, 1.0f, 0.1f, 0.123456789f, 123456789.123456789f, Single.Epsilon, Single.MinValue, Single.MaxValue);
        }

        [Test]
        public void RoundTripFloatDouble()
        {
            RoundTrip<Double>(CompareDouble, true, 0.0, 1.0, 0.1, 0.123456789, 123456789.123456789, Single.Epsilon, Single.MinValue, Single.MaxValue, Double.Epsilon, Double.MinValue, Double.MaxValue);
        }

        [Test]
        public void RoundTripFloatDecimal()
        {
            RoundTrip<Decimal>(CompareDecimal, true, 0.0M, 1.0M, 0.1M, 0.123456789M, 123456789.123456789M, Decimal.MinValue, Decimal.MaxValue);
        }

        [Test]
        public void RoundTripFloatSingleRandom()
        {
            RoundTripFloatRandom<Single>(CompareSingle, Single.MinValue, Single.MaxValue);
        }

        [Test]
        public void RoundTripFloatDoubleRandom()
        {
            RoundTripFloatRandom<Double>(CompareDouble, Double.MinValue * 0.9, Double.MaxValue * 0.9);
        }

        [Test]
        public void RoundTripFloatDecimalRandom()
        {
            RoundTripFloatRandom<Decimal>(CompareDecimal, (double)Decimal.MinValue, (double)Decimal.MaxValue);
        }

        // NOTE: the comparison functions wouldn't be needed if json round-tripped correctly for float and decimal numbers.
        // It does for double, because the reader always uses double. To make it work for the others we should defer string
        // parsing until the target numeric type is known -- is this possible within the existing framework?
        private int CompareSingle(float x, float y)
        {
#if true
            // TODO: this is what we should be doing, however this doesn't work at present, because LitJson parses float strings as doubles,
            // which can lose precision in the lower order bit.
            // Hmm, interesting, seems to be working fine??
            return Comparer<float>.Default.Compare(x, y);
#else
            if (x == y)
                return 0;

            // Jump through unsafe hoops to compare via binary representation of floating point numbers.
            unsafe
            {
                int xi = *(int*)&x;
                int yi = *(int*)&y;
                int diff = xi - yi;

                // +/-1 are adjacent floats, call them equal. (http://www.altdevblogaday.com/2012/02/22/comparing-floating-point-numbers-2012-edition/)
                return (diff < -1 ? -1 : (diff > 1 ? +1 : 0));
            }
#endif
        }

        // Note that default comparison works fine for double -- doubles round-trip correctly through LitJson, because it uses
        // double as the primary input type for floating point numbers.
        private int CompareDouble(double x, double y)
        {
            return Comparer<double>.Default.Compare(x, y);
        }

        private int CompareDecimal(decimal x, decimal y)
        {
#if false
            // TODO: this is what we should be doing, because decimal is designed for precise repeatability and exact equality.
            // However this doesn't work at present, because LitJson parses decimal strings as doubles, which loses precision.
            return Comparer<decimal>.Default.Compare(x, y);
#else
            if (x == y)
                return 0;

            // In the meantime, convert to double and settle for some loss of precision ...
            // Jump through unsafe hoops to compare via binary representation of floating point numbers.
            double xd = (double)x;
            double yd = (double)y;
            unsafe
            {
                long xi = *(long*)&xd;
                long yi = *(long*)&yd;
                long diff = xi - yi;

                // Note that we can't be as precise as above with doubles.
                return (diff < -16 ? -1 : (diff > 16 ? +1 : 0));
            }
#endif
        }

        // Random floating point numbers don't really add much confidence, better to explicitly test
        // specific edge cases and (logarithmic?) ranges.
        private void RoundTripFloatRandom<T>(System.Comparison<T> compare, double min, double max)
        {
            const int n = 10000;

            Random rand = new Random(0x1337);
            for (int i = 0; i < n; ++i)
            {
                // Add carefully, to avoid overflowing max range.
                double d = rand.NextDouble();
                double num = d * max + (1.0 - d) * min;
                T tnum = (T)Convert.ChangeType(num, typeof(T));
                RoundTrip<T>(compare, false, tnum);
            }
        }

        private void RoundTrip<T>(params T[] input)
        {
            RoundTrip<T>(Comparer<T>.Default.Compare, true, input);
        }

        private void RoundTrip<T>(System.Comparison<T> compare, bool writeJsonToConsole, params T[] input)
        {
            string json = JsonMapper.ToJson(input);
            if (writeJsonToConsole)
            {
                Console.WriteLine(json);
            }
            T[] output = JsonMapper.ToObject<T[]>(json);

            Assert.AreEqual(input.Length, output.Length);
            for (int i = 0; i < input.Length; ++i)
            {
                Assert.That(compare(input[i], output[i]) == 0, "{0} does not match {1}", input[i], output[i]);
            }
        }
    }
}
