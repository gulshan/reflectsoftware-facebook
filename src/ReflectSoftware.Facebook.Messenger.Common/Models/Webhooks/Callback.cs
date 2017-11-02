﻿// ReflectSoftware.Facebook
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Newtonsoft.Json;
using System.Collections.Generic;

namespace ReflectSoftware.Facebook.Messenger.Common.Models.Webhooks
{
    public class Callback
    {
        /// <summary>
        /// Value will be page
        /// </summary>
        [JsonProperty("object")]
        public string Object { get; set; }

        /// <summary>
        /// Array containing event data
        /// </summary>
        [JsonProperty("entry")]
        public List<Entry> Entry { get; set; }
    }
}
