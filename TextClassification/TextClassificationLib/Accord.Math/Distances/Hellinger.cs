﻿// Accord Math Library
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

namespace Accord.Math.Distances
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///   Herlinger distance.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   In probability and statistics, the Hellinger distance (also called 
    ///   Bhattacharyya distance as this was originally introduced by Anil Kumar
    ///   Bhattacharya) is used to quantify the similarity between two probability
    ///   distributions. It is a type of f-divergence. The Hellinger distance is 
    ///   defined in terms of the Hellinger integral, which was introduced by Ernst
    ///   Hellinger in 1909.</para>
    ///   
    /// <para>
    ///   References:
    ///   <list type="bullet">
    ///     <item><description><a href="https://en.wikipedia.org/wiki/Hellinger_distance">
    ///       https://en.wikipedia.org/wiki/Hellinger_distance </a></description></item>
    ///   </list></para>  
    /// </remarks>
    /// 
    [Serializable]
    public struct Hellinger : IMetric<double[]>
    {
        /// <summary>
        ///   Computes the distance <c>d(x,y)</c> between points
        ///   <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// 
        /// <param name="x">The first point <c>x</c>.</param>
        /// <param name="y">The second point <c>y</c>.</param>
        /// 
        /// <returns>
        ///   A double-precision value representing the distance <c>d(x,y)</c>
        ///   between <paramref name="x"/> and <paramref name="y"/> according 
        ///   to the distance function implemented by this class.
        /// </returns>
        /// 
#if NET45 || NET46
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public double Distance(double[] x, double[] y)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
                sum += Math.Pow(Math.Sqrt(x[i]) - Math.Sqrt(y[i]), 2);

            return sum / Math.Sqrt(2);
        }

    }
}
