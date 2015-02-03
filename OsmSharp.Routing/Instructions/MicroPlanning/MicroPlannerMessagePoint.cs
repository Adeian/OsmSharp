﻿// OsmSharp - OpenStreetMap (OSM) SDK
// Copyright (C) 2013 Abelshausen Ben
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

using OsmSharp.Routing.EdgeAggregation.Output;

namespace OsmSharp.Routing.Instructions.MicroPlanning
{
    internal class MicroPlannerMessagePoint : MicroPlannerMessage
    {
        /// <summary>
        /// Gets or sets the aggregated point.
        /// </summary>
        public AggregatedPoint Point { get; set; }

        /// <summary>
        /// Returns a System.String that represents the current System.Object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int arcsNotTaken = 0;
            if(this.Point.ArcsNotTaken != null)
            { // arcs not taken.
                arcsNotTaken = this.Point.ArcsNotTaken.Count;
            }
            int points = 0;
            if (this.Point.Points != null)
            {
                points = this.Point.Points.Count;
            }
            return string.Format("Point:Angle={0},Location={1},ArcsNotTaken={2},Points={3}", 
                this.Point.Angle, this.Point.Location, arcsNotTaken, points);
        }
    }
}