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

using OsmSharp.Android.UI.IO.Web;
using OsmSharp.Android.UI.Renderer.Images;

namespace OsmSharp.Android.UI
{
    /// <summary>
    /// Class responsable for creating native hooks for plaform-specific functionality.
    /// </summary>
    public static class Native
    {
        /// <summary>
        /// Initializes some iOS-specifics for OsmSharp to use.
        /// </summary>
        public static void Initialize()
        {
            // intialize the native image cache factory.
            OsmSharp.UI.Renderer.Images.NativeImageCacheFactory.SetDelegate(
                () =>
            {
                return new NativeImageCache();
            });

            // register the native http webrequest.
            OsmSharp.IO.Web.HttpWebRequest.CreateNativeWebRequest = (url) =>
            {
                return new NativeHttpWebRequest(url);
            };
        }
    }
}