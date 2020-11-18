using System;

namespace DinderWS.Enums {
    /// <summary>
    /// The Interest preference enumeration.
    /// </summary>
    [Flags]
    public enum EInterest {
        /// <summary>
        /// Sports interest preference.
        /// </summary>
        Sports = 1 << 0,
        /// <summary>
        /// Music interest preference.
        /// </summary>
        Music = 1 << 1,
        /// <summary>
        /// Movies interest preference.
        /// </summary>
        Movies = 1 << 2,
        /// <summary>
        /// Outdoors interest preference.
        /// </summary>
        Outdoors = 1 << 3,
        /// <summary>
        /// Any interest preference.
        /// </summary>
        Any = Sports | Music | Movies | Outdoors
    }
}
