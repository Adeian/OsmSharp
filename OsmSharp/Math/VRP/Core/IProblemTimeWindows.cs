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

namespace OsmSharp.Math.VRP.Core
{
    /// <summary>
    /// Represents timewindows for a number of customers.
    /// </summary>
    public interface IProblemTimeWindows
    {
        /// <summary>
        /// Returns the timewindows.
        /// </summary>
        TimeWindow[] Windows
        {
            get;
        }

        /// <summary>
        /// Returns the size.
        /// </summary>
        int Size
        {
            get;
        }
    }

    /// <summary>
    /// Represents a timewindow.
    /// </summary>
    public struct TimeWindow
    {
        /// <summary>
        /// The minimum time in seconds.
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// The maximum time in seconds.
        /// </summary>
        public int Max { get; set; }
    }
}