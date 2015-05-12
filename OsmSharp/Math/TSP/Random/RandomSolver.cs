﻿
// OsmSharp - OpenStreetMap (OSM) SDK
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

using System.Collections.Generic;
using OsmSharp.Math.TSP;
using OsmSharp.Math.TSP.Problems;
using OsmSharp.Math.VRP.Routes;
using OsmSharp.Math.VRP.Routes.ASymmetric;

namespace OsmSharp.Math.TravellingSalesman.Random
{
    /// <summary>
    /// Just generates random routes.
    /// </summary>
    public class RandomSolver : SolverBase
    {
        /// <summary>
        /// Retuns the name of this solver.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Random";
            }
        }

        /// <summary>
        /// Generates a random route.
        /// </summary>
        /// <returns></returns>
        protected override IRoute DoSolve(IProblem problem)
        {
            return RandomSolver.DoSolveStatic(problem);
        }

        /// <summary>
        /// Generates a random route.
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public static IRoute DoSolveStatic(IProblem problem)
        {
            var customers = new List<int>();
            for (int customer = 0; customer < problem.Size; customer++)
            {
                customers.Add(customer);
            }
            customers.Shuffle<int>();
            return DynamicAsymmetricRoute.CreateFrom(customers);
        }

        /// <summary>
        /// Stops execution.
        /// </summary>
        public override void Stop()
        {

        }
    }
}