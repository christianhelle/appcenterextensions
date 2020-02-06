using System;
using System.Diagnostics.CodeAnalysis;

namespace AppCenterExtensions
{
    /// <summary>
    /// Generic singleton implementation using lazy initialization
    /// </summary>
    /// <typeparam name="T">Class with a parameter-less constructor</typeparam>
    [ExcludeFromCodeCoverage]
    internal static class Singleton<T> where T : class, new()
    {
        private static readonly Lazy<T> LazyInstance = new Lazy<T>();
        
        /// <summary>
        /// Gets the single instance
        /// </summary>
        /// <returns>Returns a static instance of the specified generic type</returns>
        internal static T GetInstance() => LazyInstance.Value;
    }
}