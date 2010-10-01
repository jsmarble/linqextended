using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Represents the method that compares two objects of the same type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the objects to compare.This type parameter is contravariant.
    /// That is, you can use either the type you specified or any type that is less
    /// derived. For more information about covariance and contravariance, see Covariance
    /// and Contravariance in Generics.
    /// </typeparam>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>A boolean that indicates whether x and y are equal.</returns>
    public delegate bool EqualityComparison<in T>(T x, T y);
}
