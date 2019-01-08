using System;

namespace Nez
{
    /// <summary>
    ///     A class that should be rendered in the inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InspectableClass : Attribute
    {
    }
}