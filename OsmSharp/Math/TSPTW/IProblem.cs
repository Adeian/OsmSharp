﻿// OsmSharp - OpenStreetMap (OSM) SDK
// Copyright (C) 2015 Abelshausen Ben
// 
// This file is part of OsmSharp.
// 
// OsmSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// OsmSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with OsmSharp. If not, see <http://www.gnu.org/licenses/>.

using OsmSharp.Math.VRP.Core;

namespace OsmSharp.Math.TSPTW
{
    /// <summary>
    /// Interface representing a generic TSP-problem with time windows.
    /// </summary>
    public interface IProblem : IProblemWeights, IProblemTimeWindows
    {
        /// <summary>
        /// Returns the first customer.
        /// </summary>
        int? First
        {
            get;
        }

        /// <summary>
        /// Returns the last customer.
        /// </summary>
        int? Last
        {
            get;
        }

        /// <summary>
        /// Returns true if the problem is symmetric.
        /// </summary>
        bool Symmetric
        {
            get;
        }

        /// <summary>
        /// Returns true if the problem is euclidean.
        /// </summary>
        bool Euclidean
        {
            get;
        }
    }
}