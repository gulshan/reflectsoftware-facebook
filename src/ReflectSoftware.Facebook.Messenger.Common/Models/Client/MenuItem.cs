﻿// ReflectSoftware.Facebook
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Newtonsoft.Json;

namespace ReflectSoftware.Facebook.Messenger.Common.Models.Client
{
    public abstract class MenuItem
    {
        public MenuItem(string type)
        {
            Type = type;
        }

        /// <summary>
        /// Value is web_url or postback
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// title has a 30 character limit
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}