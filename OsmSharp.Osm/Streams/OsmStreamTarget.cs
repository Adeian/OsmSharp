﻿// OsmSharp - OpenStreetMap tools & library.
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

using OsmSharp.Osm;

namespace OsmSharp.Osm.Data.Streams
{
    /// <summary>
    /// Any target of osm data (Nodes, Ways and Relations).
    /// </summary>
    public abstract class OsmStreamTarget
    {
        /// <summary>
        /// Holds the source for this target.
        /// </summary>
        private OsmStreamSource _reader;

        /// <summary>
        /// Creates a new target.
        /// </summary>
        protected OsmStreamTarget()
        {

        }

        /// <summary>
        /// Initializes the target.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Adds a node to the target.
        /// </summary>
        /// <param name="simpleNode"></param>
        public abstract void AddNode(Node simpleNode);

        /// <summary>
        /// Adds a way to the target.
        /// </summary>
        /// <param name="simpleWay"></param>
        public abstract void AddWay(Way simpleWay);

        /// <summary>
        /// Adds a relation to the target.
        /// </summary>
        /// <param name="simpleRelation"></param>
        public abstract void AddRelation(Relation simpleRelation);

        /// <summary>
        /// Registers a reader on this writer.
        /// </summary>
        /// <param name="reader"></param>
        public void RegisterSource(OsmStreamSource reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Returns the registered reader.
        /// </summary>
        protected OsmStreamSource Reader
        {
            get
            {
                return _reader;
            }
        }

        /// <summary>
        /// Pulls the changes from the source to this target.
        /// </summary>
        public void Pull()
        {
            _reader.Initialize();
            this.Initialize();
            while (_reader.MoveNext())
            {
                object sourceObject = _reader.Current();
                if (sourceObject is Node)
                {
                    this.AddNode(sourceObject as Node);
                }
                else if (sourceObject is Way)
                {
                    this.AddWay(sourceObject as Way);
                }
                else if (sourceObject is Relation)
                {
                    this.AddRelation(sourceObject as Relation);
                }
            }
            this.Close();
        }

        /// <summary>
        /// Pulls the next object and returns true if there was one.
        /// </summary>
        /// <returns></returns>
        public bool PullNext()
        {
            if (_reader.MoveNext())
            {
                object sourceObject = _reader.Current();
                if (sourceObject is Node)
                {
                    this.AddNode(sourceObject as Node);
                }
                else if (sourceObject is Way)
                {
                    this.AddWay(sourceObject as Way);
                }
                else if (sourceObject is Relation)
                {
                    this.AddRelation(sourceObject as Relation);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Closes the current target.
        /// </summary>
        public virtual void Close()
        {

        }
    }
}