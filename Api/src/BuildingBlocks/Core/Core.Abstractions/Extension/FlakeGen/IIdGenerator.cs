﻿using System.Collections.Generic;

namespace Core.Abstractions.Extension.FlakeGen
{
    /// <summary>
    /// This interface represents ID generator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdGenerator<T> : IEnumerable<T>
    {
        /// <summary>
        /// Generates new identifier every time the method is called
        /// </summary>
        /// <returns>new identifier</returns>
        T GenerateId();
    }
}