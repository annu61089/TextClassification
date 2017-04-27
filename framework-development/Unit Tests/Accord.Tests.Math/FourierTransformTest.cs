﻿// Accord Unit Tests
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2017
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.Tests.Math
{
    using Accord.Math;
    using NUnit.Framework;
    using AForge.Math;
    using System;
    using Accord.Math.Transforms;
    using System.Numerics;

    [TestFixture]
    public class FourierTransformTest
    {

        [Test]
        public void FFTTest()
        {
            // Tests from 
            // http://www.nayuki.io/res/free-small-fft-in-multiple-languages/FftTest.java

            // Test power-of-2 size FFTs
            for (int i = 0; i <= 12; i++)
                testFft(1 << i);

            // Test small size FFTs
            for (int i = 0; i < 30; i++)
                testFft(i);

            // Test diverse size FFTs
            int prev = 0;
            for (int i = 0; i <= 100; i++)
            {
                int n = (int)Math.Round(Math.Pow(1500, i / 100.0));
                if (n > prev)
                {
                    testFft(n);
                    prev = n;
                }
            }

            // Test power-of-2 size convolutions
            for (int i = 0; i <= 12; i++)
                testConvolution(1 << i);

            // Test diverse size convolutions
            prev = 0;
            for (int i = 0; i <= 100; i++)
            {
                int n = (int)Math.Round(Math.Pow(1500, i / 100.0));
                if (n > prev)
                {
                    testConvolution(n);
                    prev = n;
                }
            }

            Assert.IsTrue(maxLogError < -12.9);
        }


        private static void testFft(int size)
        {
            double[] re = randomReals(size);
            double[] im = randomReals(size);

            // Test double array overloads
            double[] inputreal = (double[])re.Clone();
            double[] inputimag = (double[])im.Clone();

            double[] refoutreal = new double[size];
            double[] refoutimag = new double[size];
            naiveDft(inputreal, inputimag, refoutreal, refoutimag, false);

            double[] actualoutreal = (double[])inputreal.Clone();
            double[] actualoutimag = (double[])inputimag.Clone();
            FourierTransform2.FFT(actualoutreal, actualoutimag, FourierTransform.Direction.Forward);

            double error = log10RmsErr(refoutreal, refoutimag, actualoutreal, actualoutimag);
            Assert.IsTrue(error < -12.9);


            // Test Complex overloads
            inputreal = (double[])re.Clone();
            inputimag = (double[])im.Clone();

            Complex[] input = inputreal.ToComplex(inputimag);
            FourierTransform2.FFT(input, FourierTransform.Direction.Forward);

            actualoutreal = input.Re();
            actualoutimag = input.Im();

            double newError = log10RmsErr(refoutreal, refoutimag, actualoutreal, actualoutimag);
            Assert.AreEqual(error, newError);
        }


        private static void testConvolution(int size)
        {
            double[] randomReals1 = randomReals(size);
            double[] randomReals2 = randomReals(size);
            double[] randomReals3 = randomReals(size);
            double[] randomReals4 = randomReals(size);

            // Test double array overloads
            double[] input0real = (double[])randomReals1.Clone();
            double[] input0imag = (double[])randomReals2.Clone();
            double[] input1real = (double[])randomReals3.Clone();
            double[] input1imag = (double[])randomReals4.Clone();

            double[] refoutreal = new double[size];
            double[] refoutimag = new double[size];
            naiveConvolve(input0real, input0imag, input1real, input1imag, refoutreal, refoutimag);

            double[] actualoutreal = new double[size];
            double[] actualoutimag = new double[size];
            FourierTransform2.Convolve(input0real, input0imag, input1real, input1imag, actualoutreal, actualoutimag);

            double error = log10RmsErr(refoutreal, refoutimag, actualoutreal, actualoutimag);
            Assert.IsTrue(error < 13);



            // Test Complex overloads
            input0real = (double[])randomReals1.Clone();
            input0imag = (double[])randomReals2.Clone();
            input1real = (double[])randomReals3.Clone();
            input1imag = (double[])randomReals4.Clone();

            Complex[] input0 = input0real.ToComplex(input0imag);
            Complex[] input1 = input1real.ToComplex(input1imag);
            Complex[] actualout = new Complex[size];
            FourierTransform2.Convolve(input0, input1, actualout);

            actualoutreal = actualout.Re();
            actualoutimag = actualout.Im();

            double newError = log10RmsErr(refoutreal, refoutimag, actualoutreal, actualoutimag);
            Assert.AreEqual(error, newError);
        }


        /* Naive reference computation functions */
        private static void naiveDft(double[] inreal, double[] inimag, double[] outreal, double[] outimag, bool inverse)
        {
            if (inreal.Length != inimag.Length || inreal.Length != outreal.Length
                || outreal.Length != outimag.Length)
                throw new ArgumentException("Mismatched lengths");

            int n = inreal.Length;
            double coef = (inverse ? 2 : -2) * Math.PI;
            for (int k = 0; k < n; k++)
            {  // For each output element
                double sumreal = 0;
                double sumimag = 0;
                for (int t = 0; t < n; t++)
                {  // For each input element
                    double angle = coef * (int)((long)t * k % n) / n;  // This is more accurate than t * k
                    sumreal += inreal[t] * Math.Cos(angle) - inimag[t] * Math.Sin(angle);
                    sumimag += inreal[t] * Math.Sin(angle) + inimag[t] * Math.Cos(angle);
                }
                outreal[k] = sumreal;
                outimag[k] = sumimag;
            }
        }


        private static void naiveConvolve(double[] xreal, double[] ximag, double[] yreal, double[] yimag, double[] outreal, double[] outimag)
        {
            if (xreal.Length != ximag.Length || xreal.Length != yreal.Length
                || yreal.Length != yimag.Length || xreal.Length != outreal.Length
                || outreal.Length != outimag.Length)
                throw new ArgumentException("Mismatched lengths");

            int n = xreal.Length;
            for (int i = 0; i < n; i++)
            {
                double sumreal = 0;
                double sumimag = 0;
                for (int j = 0; j < n; j++)
                {
                    int k = (i - j + n) % n;
                    sumreal += xreal[k] * yreal[j] - ximag[k] * yimag[j];
                    sumimag += xreal[k] * yimag[j] + ximag[k] * yreal[j];
                }
                outreal[i] = sumreal;
                outimag[i] = sumimag;
            }
        }


        private static double maxLogError = Double.PositiveInfinity;

        private static double log10RmsErr(double[] xreal, double[] ximag, double[] yreal, double[] yimag)
        {
            if (xreal.Length != ximag.Length || xreal.Length != yreal.Length || yreal.Length != yimag.Length)
                throw new ArgumentException("Mismatched lengths");

            double err = 0;
            for (int i = 0; i < xreal.Length; i++)
                err += (xreal[i] - yreal[i]) * (xreal[i] - yreal[i]) + (ximag[i] - yimag[i]) * (ximag[i] - yimag[i]);

            err = Math.Sqrt(err / Math.Max(xreal.Length, 1));  // Now this is a root mean square (RMS) error
            err = Math.Log10(err);

            if (err < maxLogError)
                maxLogError = err;

            return err;
        }


        private static double[] randomReals(int size)
        {
            var random = Accord.Math.Random.Generator.Random;

            double[] result = new double[size];
            for (int i = 0; i < result.Length; i++)
                result[i] = random.NextDouble() * 2 - 1;
            return result;
        }

    }
}
