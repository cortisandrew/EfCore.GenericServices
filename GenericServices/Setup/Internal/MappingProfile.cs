// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace GenericServices.Setup.Internal
{
    /// <summary>
    /// A Mapping Profile indicating whether attributes with ReadOnly attribute should be ignored
    /// </summary>
    /// <remarks>
    /// Had to place as public because it is referenced in a public interface. I have added a sealed here to avoid inheritance outside of the internal namespace.
    /// It does not seem to be used externally
    /// </remarks>
    public sealed class MappingProfile
    {
        public bool IgnoreReadOnlyAttributes { get; private set; }

        public bool IgnorePropertiesWithInaccessibleSetters { get; private set; }

        internal MappingProfile(bool ignoreReadOnlyAttributes, bool ignorePropertiesWithInaccessibleSetters = false)
        {
            IgnoreReadOnlyAttributes = ignoreReadOnlyAttributes;
            IgnorePropertiesWithInaccessibleSetters = ignorePropertiesWithInaccessibleSetters;
        }

        /* Remarks: The code below is now depracated as the responsibility is now moved elsewhere
        public static void SetIgnoreReadOnly<TSource, TDestination>(TypeAdapterSetter<TSource, TDestination> config, bool addIgnoreParts)
        {
            if (addIgnoreParts)
            {
                foreach (var member in typeof(TDestination).GetMembers())
                {
                    if (Filter(member)) // Filter now takes PropertyInfo
                    {
                        config.Ignore(member.Name);
                    }
                }
            }
        }

        /// <summary>
        /// This returns true for source properties that we DON'T want to be copied
        /// This stops DTP properties that are null, or have a [ReadOnly(true)] attribute, fom being copied.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private static bool Filter(MemberInfo member)
        {         
            return member == null || (member.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly ?? false);
        }
        */
    }
}