﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.OData.Routing
{
    /// <summary>
    /// Indicates that a controller is not an OData controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class NonODataControllerAttribute : Attribute
    {
    }
}