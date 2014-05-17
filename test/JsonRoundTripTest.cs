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
            RoundTrip<Single>(CompareSingle, 0.0f, 1.0f, 0.1f, 0.123456789f, 123456789.123456789f, Single.Epsilon, Single.MinValue, Single.MaxValue);
        }

        [Test]
        public void RoundTripFloatDouble()
        {
			RoundTrip<Double>(CompareDouble, 0.0, 1.0, 0.1, 0.123456789, 123456789.123456789, Single.Epsilon, Single.MinValue, Single.MaxValue, Double.Epsilon, Double.MinValue, Double.MaxValue);
		}

        [Test]
        public void RoundTripFloatDecimal()
        {
            RoundTrip<Decimal>(CompareDecimal, 0.0M, 1.0M, 0.1M, 0.123456789M, 123456789.123456789M, Decimal.MinValue, Decimal.MaxValue);
        }

        private int CompareSingle(float x, float y)
        {
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
        }

        private int CompareDouble(double x, double y)
        {
            if (x == y)
                return 0;

            // Jump through unsafe hoops to compare via binary representation of floating point numbers.
            unsafe
            {
                long xi = *(int*)&x;
                long yi = *(int*)&y;
                long diff = xi - yi;

                // +/-1 are adjacent doubles, call them equal. (http://www.altdevblogaday.com/2012/02/22/comparing-floating-point-numbers-2012-edition/)
                return (diff < -1 ? -1 : (diff > 1 ? +1 : 0));
            }
        }

        private int CompareDecimal (decimal x, decimal y)
        {
            // TODO: this is what we should be doing, because decimal is designed for precise repeatability and exact equality.
            // However this doesn't work at present, because LitJson parses decimal strings as doubles, which can lose precision.
            // A good fix might be to put an 'm' suffix on decimal values in the output json text, and respect them on input.
            // (For that matter a similar 'f' suffix for float could simplify the parsing and data conversion.)

            //return Decimal.Compare (x, y);

            if (x == y)
                return 0;

            // In the meantime, convert to double and settle for some loss of precision ...

            // Jump through unsafe hoops to compare via binary representation of floating point numbers.
            double xd = (double)x;
            double yd = (double)y;
            unsafe
            {
                long xi = *(int*)&xd;
                long yi = *(int*)&yd;
                long diff = xi - yi;

                // Note that we can't be as precise as above with doubles.
                return (diff < -16 ? -1 : (diff > 16 ? +1 : 0));
            }
        }

        private void RoundTrip<T>(params T[] before)
        {
            RoundTrip<T>(Comparer<T>.Default.Compare, before);
        }

        private void RoundTrip<T>(System.Comparison<T> compare, params T[] before)
        {
            string json = JsonMapper.ToJson(before);
            Console.WriteLine(json);
            T[] after = JsonMapper.ToObject<T[]>(json);

            Assert.AreEqual(before.Length, after.Length);
            for (int i = 0; i < before.Length; ++i)
            {
                Assert.That(compare(before[i], after[i]) == 0, "{0} does not match {1}", before[i], after[i]);
            }
        }
    }
}
