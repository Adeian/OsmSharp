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

using System.Reflection;
using NUnit.Framework;
using OsmSharp.Collections.Tags;
using OsmSharp.Osm.Streams.Filters;
using OsmSharp.Osm.Xml.Streams;
using OsmSharp.Routing;
using OsmSharp.Routing.CH;
using OsmSharp.Routing.CH.PreProcessing;
using OsmSharp.Routing.CH.PreProcessing.Ordering;
using OsmSharp.Routing.CH.PreProcessing.Witnesses;
using OsmSharp.Routing.Graph;
using OsmSharp.Routing.Graph.Router;
using OsmSharp.Routing.Interpreter;
using OsmSharp.Routing.Osm.Interpreter;
using OsmSharp.Routing.Osm.Streams.Graphs;

namespace OsmSharp.Test.Unittests.Routing.CH
{
    /// <summary>
    /// Tests the sparse node ordering CH.
    /// </summary>
    [TestFixture]
    public class CHEdgeDifferenceRoutingTest : SimpleRoutingTests<CHEdgeData>
    {
        /// <summary>
        /// Returns a new router.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="interpreter"></param>
        /// <param name="basicRouter"></param>
        /// <returns></returns>
        public override Router BuildRouter(IBasicRouterDataSource<CHEdgeData> data,
            IRoutingInterpreter interpreter, IBasicRouter<CHEdgeData> basicRouter)
        {
            return Router.CreateCHFrom(data, basicRouter, interpreter);
        }

        /// <summary>
        /// Returns a basic router.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override IBasicRouter<CHEdgeData> BuildBasicRouter(IBasicRouterDataSource<CHEdgeData> data)
        {
            return new CHRouter();
        }

        /// <summary>
        /// Builds the data.
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="embeddedString"></param>
        /// <returns></returns>
        public override IBasicRouterDataSource<CHEdgeData> BuildData(IOsmRoutingInterpreter interpreter, 
            string embeddedString)
        {
            string key = string.Format("CHEdgeDifference.Routing.IBasicRouterDataSource<CHEdgeData>.OSM.{0}",
                embeddedString);
            var data = StaticDictionary.Get<IBasicRouterDataSource<CHEdgeData>>(
                key);
            if (data == null)
            {
                var tagsIndex = new TagsTableCollectionIndex();

                // do the data processing.
                var memoryData = new DynamicGraphRouterDataSource<CHEdgeData>(tagsIndex);
                var targetData = new CHEdgeGraphOsmStreamTarget(
                    memoryData, interpreter, tagsIndex, Vehicle.Car);
                var dataProcessorSource = new XmlOsmStreamSource(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedString));
                var sorter = new OsmStreamFilterSort();
                sorter.RegisterSource(dataProcessorSource);
                targetData.RegisterSource(sorter);
                targetData.Pull();

                // do the pre-processing part.
                var witnessCalculator = new DykstraWitnessCalculator();
                var preProcessor = new CHPreProcessor(memoryData,
                    new EdgeDifference(memoryData, witnessCalculator), witnessCalculator);
                preProcessor.Start();

                data = memoryData;
                StaticDictionary.Add<IBasicRouterDataSource<CHEdgeData>>(key, data);
            }
            return data;
        }

        /// <summary>
        /// Tests a simple shortest route calculation.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceShortedDefault()
        {
            this.DoTestShortestDefault();
        }

        /// <summary>
        /// Tests if the raw router preserves tags.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceResolvedTags()
        {
            this.DoTestResolvedTags();
        }

        /// <summary>
        /// Tests if the raw router preserves tags on arcs/ways.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceArcTags()
        {
            this.DoTestArcTags();
        }

        /// <summary>
        /// Test is the CH router can calculate another route.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceShortest1()
        {
            this.DoTestShortest1();
        }

        /// <summary>
        /// Test is the CH router can calculate another route.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceShortest2()
        {
            this.DoTestShortest2();
        }

        /// <summary>
        /// Test is the CH router can calculate another route.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceShortest3()
        {
            this.DoTestShortest3();
        }

        /// <summary>
        /// Test is the CH router can calculate another route.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceShortest4()
        {
            this.DoTestShortest4();
        }

        /// <summary>
        /// Test is the CH router can calculate another route.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceShortest5()
        {
            this.DoTestShortest5();
        }

        /// <summary>
        /// Test is the raw router can calculate another route.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceResolvedShortest1()
        {
            this.DoTestShortestResolved1();
        }

        /// <summary>
        /// Test is the raw router can calculate another route.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceResolvedShortest2()
        {
            this.DoTestShortestResolved2();
        }

        /// <summary>
        /// Test if the ch router many-to-many weights correspond to the point-to-point weights.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceManyToMany1()
        {
            this.DoTestManyToMany1();
        }

        /// <summary>
        /// Tests a simple shortest route calculation.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceResolveAllNodes()
        {
            this.DoTestResolveAllNodes();
        }

        /// <summary>
        /// Tests a simple shortest route calculation.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceResolveBetweenNodes()
        {
            this.DoTestResolveBetweenNodes();
        }

        /// <summary>
        /// Tests routing when resolving points.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceResolveBetweenClose()
        {
            this.DoTestResolveBetweenClose();
        }

        /// <summary>
        /// Tests routing when resolving points.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceResolveBetweenTwo()
        {
            this.DoTestResolveBetweenTwo();
        }

        /// <summary>
        /// Tests routing when resolving points.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceResolveCase1()
        {
            this.DoTestResolveCase1();
        }

        /// <summary>
        /// Tests routing when resolving points.
        /// </summary>
        [Test]
        public void TestCHEdgeDifferenceResolveCase2()
        {
            this.DoTestResolveCase2();
        }
    }
}