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

using OsmSharp.Routing.Graph;
using System.Collections.Generic;

namespace OsmSharp.Routing.Contracted.PreProcessing
{
    /// <summary>
    /// Represents the data on a contracted edge.
    /// </summary>
    public struct ContractedEdge : IEdge
    {
        /// <summary>
        /// Bitmask holding status info [forwardMove(1), backwardMove(2), forwardTags(4), contracted(8)]
        /// </summary>
        private byte _meta;

        /// <summary>
        /// Contains a value that either represents the contracted vertex of the tags id.
        /// </summary>
        private uint _value;

        /// <summary>
        /// Creates a new contracted edge that represents normal neighbour relations.
        /// </summary>
        /// <param name="tagsId"></param>
        /// <param name="tagsForward"></param>
        /// <param name="canMoveforward"></param>
        /// <param name="canMoveBackward"></param>
        /// <param name="weight"></param>
        public ContractedEdge(uint tagsId, bool tagsForward, bool canMoveforward, bool canMoveBackward, float weight)
            : this()
        {
            _meta = 0;

            this.CanMoveBackward = canMoveBackward;
            this.CanMoveForward = canMoveforward;
            this.Tags = tagsId;
            this.Forward = tagsForward;
            this.Weight = weight;
        }

        /// <summary>
        /// Creates a new contracted edge.
        /// </summary>
        /// <param name="contractedId"></param>
        /// <param name="canMoveforward"></param>
        /// <param name="canMoveBackward"></param>
        /// <param name="weight"></param>
        public ContractedEdge(uint contractedId, bool canMoveforward, bool canMoveBackward, float weight)
            : this()
        {
            _meta = 0;

            this.CanMoveBackward = canMoveBackward;
            this.CanMoveForward = canMoveforward;
            this.ContractedId = contractedId;
            this.Weight = weight;
        }

        /// <summary>
        /// Creates a new contracted edge using raw data.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="weight"></param>
        /// <param name="meta"></param>
        public ContractedEdge(uint value, float weight, byte meta)
            : this()
        {
            _meta = meta;
            _value = value;

            this.Weight = weight;
        }

        /// <summary>
        /// Gets the raw meta data.
        /// </summary>
        public byte Meta
        {
            get
            {
                return _meta;
            }
        }

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        public uint Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// Holds the weight.
        /// </summary>
        public float Weight { get; private set; }

        /// <summary>
        /// Returns true if you can move forward along this edge.
        /// </summary>
        public bool CanMoveForward
        {
            get
            {
                return (_meta & (1 << 0)) != 0;
            }
            private set
            {
                if (value)
                {
                    _meta = (byte)(_meta | 1);
                }
                else
                {
                    _meta = (byte)(_meta & (255 - 1));
                }
            }
        }

        /// <summary>
        /// Returns true if you can move backward along this edge.
        /// </summary>
        public bool CanMoveBackward
        {
            get
            {
                return (_meta & (1 << 1)) != 0;
            }
            private set
            {
                if (value)
                {
                    _meta = (byte)(_meta | 2);
                }
                else
                {
                    _meta = (byte)(_meta & (255 - 2));
                }
            }
        }

        /// <summary>
        /// Holds the forward contracted id.
        /// </summary>
        public uint ContractedId
        {
            get
            {
                if (!this.RepresentsNeighbourRelations)
                {
                    return _value;
                }
                return uint.MaxValue;
            }
            private set
            {
                // set contracted.
                _meta = (byte)(_meta | 8);
                _value = value;
            }
        }

        /// <summary>
        /// Returns true when this edge is not contracted and represents an normal neighbour relation.
        /// </summary>
        public bool RepresentsNeighbourRelations
        {
            get
            {
                return !this.IsContracted;
            }
        }

        /// <summary>
        /// Returns true if this edge is a contracted edge.
        /// </summary>
        public bool IsContracted
        {
            get
            {
                return (_meta & (1 << 3)) != 0;
            }
        }

        /// <summary>
        /// Flag indicating if the tags are forward relative to this edge or not.
        /// </summary>
        public bool Forward
        {
            get
            {
                return (_meta & (1 << 2)) != 0;
            }
            private set
            {
                if (value)
                {
                    _meta = (byte)(_meta | 4);
                }
                else
                {
                    _meta = (byte)(_meta & (255 - 4));
                }
            }
        }

        /// <summary>
        /// The properties of this edge.
        /// </summary>
        public uint Tags
        {
            get
            {
                if (this.IsContracted)
                {
                    return uint.MaxValue;
                }
                return _value;
            }
            set
            {
                // set uncontracted.
                _meta = (byte)(_meta & (255 - 8));
                _value = value;
            }
        }

        /// <summary>
        /// Returns the exact inverse of this edge.
        /// </summary>
        /// <returns></returns>
        public IEdge Reverse()
        {
            if (this.IsContracted)
            {
                return new ContractedEdge(this.ContractedId, this.CanMoveBackward, this.CanMoveForward, this.Weight);
            }
            return new ContractedEdge(this.Tags, !this.Forward,
                this.CanMoveBackward, this.CanMoveForward, this.Weight);
        }

        /// <summary>
        /// Returns true if the given edge equals this edge.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IEdge other)
        {
            var otherEdge = (ContractedEdge)other;
            return otherEdge._value == this._value &&
                otherEdge._meta == this._meta &&
                otherEdge.Weight == this.Weight;
        }
    }

    /// <summary>
    /// Contains extensions related to the CHEdgeData.
    /// </summary>
    public static class CHExtensions
    {
        /// <summary>
        /// Removes all contracted edges.
        /// </summary>
        /// <param name="edges"></param>
        public static List<Edge<ContractedEdge>> KeepUncontracted(this List<Edge<ContractedEdge>> edges)
        {
            var result = new List<Edge<ContractedEdge>>(edges.Count);
            foreach (var edge in edges)
            {
                if (!edge.EdgeData.IsContracted)
                {
                    result.Add(edge);
                }
            }
            return result;
        }
    }
}