namespace Volo.Abp.Identity
{
    public static class OrganizationUnitConsts
    {
        /// <summary>
        /// Maximum length of the DisplayName property.
        /// </summary>
        public static int MaxDisplayNameLength { get; set; } = 128;

        /// <summary>
        /// Maximum depth of an OU hierarchy.
        /// </summary>
        public const int MaxDepth = 16;

        /// <summary>
        /// Length of a code unit between dots.
        /// </summary>
        public const int CodeUnitLength = 5;

        /// <summary>
        /// Maximum length of the Code property.
        /// </summary>
        public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;
    }
}
