﻿// ReflectSoftware.Facebook
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using ReflectSoftware.Facebook.Messenger.Common.Interfaces;
using Newtonsoft.Json;

namespace ReflectSoftware.Facebook.Messenger.Common.Models
{
    public abstract class TemplatePayload : IPayload
    {
        public TemplatePayload(string templateType)
        {
            TemplateType = templateType;
        }

        /// <summary>
        /// Template type generic, button or receipt
        /// </summary>
        [JsonProperty("template_type")]
        public string TemplateType { get; private set; } 
    }
}