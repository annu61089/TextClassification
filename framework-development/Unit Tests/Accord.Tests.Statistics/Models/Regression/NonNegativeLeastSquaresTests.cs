﻿// Accord Unit Tests
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © Clement Schiano di Colella, 2015-2016
// clement.schiano@gmail.com
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

namespace Accord.Tests.Statistics.Models.Regression
{
    using System;
    using System.Diagnostics;
    using Accord.Math;
    using NUnit.Framework;
    using Accord.Statistics.Models.Regression.Fitting;
    using Accord.Statistics.Models.Regression.Linear;

    [TestFixture]
    public class NonNegativeLeastSquaresTests
    {
        [Test]
        public void ExampleTest()
        {
            // Suppose we would like to map the continuous values in the
            // second column to the integer values in the first column.
            var inputs = new[]
            {
                new[] { 1.0, 1.0 },
                new[] { 2.0, 4.0 },
                new[] { 3.0, 9.0 },
                new[] { 4.0, 16.0 },
            };

            var ouputs = new[] { 0.6, 2.2, 4.8, 8.4 };

            var regression = new MultipleLinearRegression(2);
            var nnls = new NonNegativeLeastSquares(regression)
            {
                MaxIterations = 100
            };

            nnls.Run(inputs, ouputs);
            Assert.AreEqual(0.1, nnls.Coefficients[0], 1e-3);
            Assert.AreEqual(0.5, nnls.Coefficients[1], 1e-3);
        }

        [Test]
        public void ExampleTest2()
        {
            // Suppose we would like to map the continuous values in the
            // second column to the integer values in the first column.
            var inputs = new[]
            {
                new[] { 1.0, 1.0, 1.0 },
                new[] { 2.0, 4.0, 8.0 },
                new[] { 3.0, 9.0, 27.0 },
                new[] { 4.0, 16.0, 64.0 },
            };

            var ouputs = new[] { 0.73, 3.24, 8.31, 16.72 };

            var regression = new MultipleLinearRegression(3);
            var nnls = new NonNegativeLeastSquares(regression)
            {
                MaxIterations = 100
            };

            nnls.Run(inputs, ouputs);
            Assert.AreEqual(0.1, nnls.Coefficients[0], 1e-3);
            Assert.AreEqual(0.5, nnls.Coefficients[1], 1e-3);
            Assert.AreEqual(0.13, nnls.Coefficients[2], 1e-3);
        }

        [Test]
        public void ExampleTest3()
        {
            // Suppose we would like to map the continuous values in the
            // second column to the integer values in the first column.
            var inputs = new[]
            {
                new[] { 1.0, 1.0, 1.0, 1.0 },
                new[] { 2.0, 4.0, 8.0, 16.0 },
                new[] { 3.0, 9.0, 27.0, 81.0 },
                new[] { 4.0, 16.0, 64.0, 256.0 },
            };

            var ouputs = new[] { 0.73, 3.24, 8.31, 16.72 };

            var regression = new MultipleLinearRegression(4);
            var nnls = new NonNegativeLeastSquares(regression)
            {
                MaxIterations = 100
            };

            nnls.Run(inputs, ouputs);
            Assert.AreEqual(0.1, nnls.Coefficients[0], 1e-3);
            Assert.AreEqual(0.5, nnls.Coefficients[1], 1e-3);
            Assert.AreEqual(0.13, nnls.Coefficients[2], 1e-3);
            Assert.AreEqual(0, nnls.Coefficients[3], 1e-3);
        }

        [Test]
        public void ExampleTest4()
        {
            // Suppose we would like to map the continuous values in the
            // second column to the integer values in the first column.
            var inputs = new[]
            {
                new[] { 1.0, 1.0, 1.0 },
                new[] { 2.0, 4.0, 8.0 },
                new[] { 3.0, 9.0, 27.0 },
                new[] { 4.0, 16.0, 64.0 },
            };

            var ouputs = new[] { 0.23, 1.24, 3.81, 8.72 };

            var regression = new MultipleLinearRegression(3);
            var nnls = new NonNegativeLeastSquares(regression)
            {
                MaxIterations = 100
            };

            nnls.Run(inputs, ouputs);
            Assert.AreEqual(0.1, nnls.Coefficients[0], 1e-3);
            Assert.AreEqual(0, nnls.Coefficients[1], 1e-3);
            Assert.AreEqual(0.13, nnls.Coefficients[2], 1e-3);
        }
    }
}
