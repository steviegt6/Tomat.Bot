#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;

namespace Tomat.TomatBot.Core.Services.Commands
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ParametersAttribute : Attribute
    {
        public string Parameters { get; }

        public ParametersAttribute(string parameters) => Parameters = parameters;
    }
}