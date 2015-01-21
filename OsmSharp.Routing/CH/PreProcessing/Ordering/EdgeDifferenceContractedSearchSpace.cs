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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OsmSharp.Routing.Graph;
using OsmSharp.Routing.Graph.Router;

namespace OsmSharp.Routing.CH.PreProcessing.Ordering
{
    /// <summary>
    /// The edge difference calculator.
    /// </summary>
    public class EdgeDifferenceContractedSearchSpace : INodeWeightCalculator
    {
        /// <summary>
        /// Holds the graph.
        /// </summary>
        private INodeWitnessCalculator _witness_calculator;

        /// <summary>
        /// Holds the data.
        /// </summary>
        private IGraph<CHEdgeData> _data;

        /// <summary>
        /// Holds the contracted count.
        /// </summary>
        private Dictionary<uint, short> _contraction_count;

        /// <summary>
        /// Holds the depth.
        /// </summary>
        private Dictionary<long, long> _depth;

        /// <summary>
        /// Creates a new edge difference calculator.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="witness_calculator"></param>
        public EdgeDifferenceContractedSearchSpace(IGraph<CHEdgeData> data, INodeWitnessCalculator witness_calculator)
        {
            _data = data;
            _witness_calculator = witness_calculator;
            _contraction_count = new Dictionary<uint, short>();
            _depth = new Dictionary<long, long>();
        }

        /// <summary>
        /// Calculates the edge-difference if u would be contracted.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public float Calculate(uint vertex)
        {
            short contracted = 0;
            _contraction_count.TryGetValue(vertex, out contracted);

            // get the neighbours.
            var neighbours = _data.GetEdges(vertex);

            // simulate the construction of new edges.
            int newEdges = 0;
            int removed = 0;
            var edgesForContractions = new List<Edge<CHEdgeData>>();
            var tos = new List<uint>();
            foreach (var neighbour in neighbours)
            {
                edgesForContractions.Add(neighbour);
                tos.Add(neighbour.Neighbour);
                removed++;
            }

            // loop over all neighbours and check for witnesses.
            // loop over each combination of edges just once.
            var forwardWitnesses = new bool[edgesForContractions.Count];
            var backwardWitnesses = new bool[edgesForContractions.Count];
            var weights = new List<float>(edgesForContractions.Count);
            for (int x = 0; x < edgesForContractions.Count; x++)
            { // loop over all elements first.
                var xEdge = edgesForContractions[x];

                // calculate max weight.
                weights.Clear();
                for (int y = 0; y < x; y++)
                {
                    // update maxWeight.
                    var yEdge = edgesForContractions[y];
                    if (xEdge.Neighbour != yEdge.Neighbour)
                    {
                        // reset witnesses.
                        forwardWitnesses[y] = !xEdge.EdgeData.CanMoveBackward || !yEdge.EdgeData.CanMoveForward;
                        backwardWitnesses[y] = !xEdge.EdgeData.CanMoveForward || !yEdge.EdgeData.CanMoveBackward;
                        weights.Add((float)xEdge.EdgeData.Weight + (float)yEdge.EdgeData.Weight);
                    }
                    else
                    { // already set this to true, not use calculating it's witness.
                        forwardWitnesses[y] = true;
                        backwardWitnesses[y] = true;
                        weights.Add(0);
                    }
                }

                // calculate witnesses.
                _witness_calculator.Exists(_data, true, xEdge.Neighbour, tos, weights, int.MaxValue, ref forwardWitnesses);
                _witness_calculator.Exists(_data, false, xEdge.Neighbour, tos, weights, int.MaxValue, ref backwardWitnesses);

                for (int y = 0; y < edgesForContractions.Count; y++)
                { // loop over all elements.
                    var yEdge = edgesForContractions[y];

                    var canMoveForward = !forwardWitnesses[y] && (xEdge.EdgeData.CanMoveBackward && yEdge.EdgeData.CanMoveForward);
                    var canMoveBackward = !backwardWitnesses[y] && (xEdge.EdgeData.CanMoveForward && yEdge.EdgeData.CanMoveBackward);

                    if (yEdge.Neighbour != xEdge.Neighbour &&
                        (canMoveForward || canMoveBackward))
                    { // the neighbours point to different vertices.
                        // a new edge is needed.
                        // no witness exists.
                        newEdges++;
                    }
                }
            }

            // get the depth.
            long vertex_depth = 0;
            _depth.TryGetValue(vertex, out vertex_depth);
            return (2 * newEdges) + (-3 * removed) + (2 * contracted) + (2 * vertex_depth);
        }

        /// <summary>
        /// Notifies this calculator that the vertex was contracted.
        /// </summary>
        /// <param name="vertex"></param>
        public void NotifyContracted(uint vertex)
        {
            // removes the contractions count.
            _contraction_count.Remove(vertex);

            // loop over all neighbours.
            var neighbours = _data.GetEdges(vertex);
            foreach (var neighbour in neighbours)
            {
                short count;
                if (!_contraction_count.TryGetValue(neighbour.Neighbour, out count))
                {
                    _contraction_count[neighbour.Neighbour] = 1;
                }
                else
                {
                    _contraction_count[neighbour.Neighbour] = count++;
                }
            }

            long vertex_depth = 0;
            _depth.TryGetValue(vertex, out vertex_depth);
            _depth.Remove(vertex);
            vertex_depth++;

            // store the depth.
            foreach (var neighbour in neighbours)
            {
                long depth = 0;
                _depth.TryGetValue(neighbour.Neighbour, out depth);
                if (vertex_depth < depth)
                {
                    // _depth[neighbour.Neighbour] = depth;
                }
                else
                {
                    _depth[neighbour.Neighbour] = vertex_depth;
                }
            }
        }
    }
}