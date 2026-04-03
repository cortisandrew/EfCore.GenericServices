// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Mapster;

namespace GenericServices.Configuration
{
    /// <summary>
    /// This provides a per-DTO/ViewModel configuration source
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class PerDtoConfig<TDto, TEntity> : PerDtoConfig
        where TDto : class where TEntity : class
    {
        //-------------------------------------------------
        //Properties to set the Mapster Read and Save mappings

        /// <summary>
        /// This allows you to configure Mapster's type adapter setter for the reader, i.e. from Entity class to DTO/ViewModel
        /// </summary>
        /// <returns>
        /// true if overridden and configured, false otherwise. It is important to return true if you override this method!
        /// </returns>
        public virtual bool ConfigureReadMapping(out TypeAdapterSetter<TEntity, TDto> typeAdapterSetter)
        {
            typeAdapterSetter = null;
            return false;
        }
        /// <summary>
        /// This allows you to configure the Mapster's type adapter setter for the create/update, i.e. from DTO/ViewModel to Entity class 
        /// </summary>
        /// <returns>
        /// true if overridden and configured, false otherwise. It is important to return true if you override this method!
        /// </returns>
        public virtual bool ConfigureSaveMapping(out TypeAdapterSetter<TDto, TEntity> typeAdapterSetter)
        {
            typeAdapterSetter = null;
            return false;
        }
    }
}