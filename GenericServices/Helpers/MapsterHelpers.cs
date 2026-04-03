namespace GenericServices.Helpers
{
    using Mapster;
    using System.ComponentModel;
    using System.Reflection;

    namespace GenericServices.Helpers
    {
        /// <summary>
        /// Extension methods to help with Mapster configuration, such as ignoring properties with [ReadOnly(true)] attributes, or properties with inaccessible setters.
        /// </summary>
        public static class MapsterHelpers
        {
            /// <summary>
            /// Sets members that have ReadOnlyAttribute with a ReadOnly value set to true as Ignored by Mapster mapping when addIgnoreParts is true
            /// </summary>
            /// <typeparam name="TSource"></typeparam>
            /// <typeparam name="TDestination"></typeparam>
            /// <param name="config"></param>
            /// <param name="addIgnoreParts"></param>
            public static TypeAdapterSetter<TSource, TDestination> SetIgnoreReadOnly<TSource, TDestination>(this TypeAdapterSetter<TSource, TDestination> config, bool addIgnoreParts)
            {
                if (addIgnoreParts)
                {
                    foreach (var member in typeof(TSource).GetMembers())
                    {
                        if (Filter(member)) // Filter now takes PropertyInfo
                        {
                            config.Ignore(member.Name);
                        }
                    }
                }

                return config;
            }

            /// <summary>
            /// When mapping to a destination of type <typeparamref name="TDestination"/>, this method will instruct Mapster to ignore any properties that
            /// have an inaccessible setter (i.e. a private setter, or no setter at all).
            /// </summary>
            /// <typeparam name="TSource"></typeparam>
            /// <typeparam name="TDestination"></typeparam>
            /// <param name="config"></param>
            /// <returns></returns>
            public static TypeAdapterSetter<TSource, TDestination> IgnoreAllPropertiesWithAnInaccessibleSetter<TSource, TDestination>(this TypeAdapterSetter<TSource, TDestination> config)
            {
                foreach (var prop in typeof(TDestination).GetProperties())
                {
                    var setMethod = prop.SetMethod;

                    if (setMethod == null || !setMethod.IsPublic)
                    {
                        config.Ignore(prop.Name);
                    }
                }

                return config;
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
        }
    }
}
