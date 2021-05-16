using System;

namespace Tomat.TomatBot.Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ParametersAttribute : Attribute
    {
        public string Parameters { get; }

        public ParametersAttribute(string parameters) => Parameters = parameters;
    }
}