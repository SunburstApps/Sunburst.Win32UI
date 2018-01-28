namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Specifies the possible ways two <see cref="Region"/> instances can be combined.
    /// </summary>
    public enum RegionCombinationMode
    {
        /// <summary>
        /// The new region contains the area where input regions overlap.
        /// </summary>
        Intersect,

        /// <summary>
        /// The new region contains the area of both the input regions.
        /// </summary>
        Union,

        /// <summary>
        /// The new region contains the are of both the input regions, except where they overlap.
        /// </summary>
        UnionExceptOverlap
    }
}
