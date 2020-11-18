using System;

namespace DinderWS.Enums {
    /// <summary>
    /// The Dietary Restriction preference enumeration.
    /// </summary>
    [Flags]
    public enum EDietaryRestriction {
        /// <summary>
        /// Vegan dietary restriction preference.
        /// </summary>
        Vegan = 1 << 0,
        /// <summary>
        /// Vegetarian dietary restriction preference.
        /// </summary>
        Vegetarian = 1 << 1,
        /// <summary>
        /// Pescetarian dietary restriction preference.
        /// </summary>
        Pescetarian = 1 << 2,
        /// <summary>
        /// Gluten Free dietary restriction preference.
        /// </summary>
        GlutenFree = 1 << 3,
        /// <summary>
        /// Lactose Free dietary restriction preference.
        /// </summary>
        LactoseFree = 1 << 4,
        /// <summary>
        /// No dietary restriction preference.
        /// </summary>
        None = 0
    }
}
