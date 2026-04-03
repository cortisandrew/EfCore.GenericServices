// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using GenericServices.Configuration;
using GenericServices.Helpers.GenericServices.Helpers;
using GenericServices.Setup.Internal;
using Mapster;
using System;

namespace GenericServices.PublicButHidden
{
    /// <summary>
    /// This is the interface used for dependency injection of the <see cref="WrappedAndMapper"/>
    /// </summary>
    public interface IWrappedConfigAndMapper
    {
        /// <summary>
        /// This is the global configuration information
        /// </summary>
        IGenericServicesConfig Config { get; }

        /// <summary>
        /// This is the Mapster configuration used for reading/projection from entity class to DTO
        /// </summary>
        MappingProfile MapsterReadConfig { get; }

        /// <summary>
        /// This is the Mapster configuration used for copying from a DTO to the entity class
        /// </summary>
        MappingProfile MapsterSaveConfig { get; }

    }

    /// <summary>
    /// This contains the Mapster setting needed by GenericServices
    /// </summary>
    public class WrappedAndMapper : IWrappedConfigAndMapper
    {

        internal WrappedAndMapper(IGenericServicesConfig config, MappingProfile mapsterReadConfig = null, MappingProfile mapsterSaveConfig = null)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            MapsterReadConfig = mapsterReadConfig;
            MapsterSaveConfig = mapsterSaveConfig;
        }

        /// <inheritdoc />
        public IGenericServicesConfig Config { get; }

        /// <inheritdoc />
        public MappingProfile MapsterReadConfig { get; }
       

        /// <inheritdoc />
        public MappingProfile MapsterSaveConfig { get; }
    }
}