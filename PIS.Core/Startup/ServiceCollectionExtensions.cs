﻿using System;
using PIS.Framework.Context;
using PIS.Framework.Uow;
using PIS.Framework.Repositories;
using PIS.Framework.Paging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PIS.Framework
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddDataAccess<TEntityContext>(this IServiceCollection services) where TEntityContext : EntityContextBase<TEntityContext>
        {
            RegisterDataAccess<TEntityContext>(services);
            return services;
        }

        private static void RegisterDataAccess<TEntityContext>(IServiceCollection services) where TEntityContext : EntityContextBase<TEntityContext>
        {
            services.TryAddScoped<IUowProvider, UowProvider>();
            services.TryAddTransient<IEntityContext, TEntityContext>();
            services.TryAddTransient(typeof(IRepository<>), typeof(GenericEntityRepository<>));
            services.TryAddTransient(typeof(IDataPager<>), typeof(DataPager<>));
            services.AddScoped(typeof(ICacheProvider<>), typeof(CacheProvider<>));
        }

        private static void ValidateMandatoryField(string field, string fieldName)
        {
            if ( field == null ) throw new ArgumentNullException(fieldName, $"{fieldName} cannot be null.");
            if ( field.Trim() == String.Empty ) throw new ArgumentException($"{fieldName} cannot be empty.", fieldName);
        }

        private static void ValidateMandatoryField(object field, string fieldName)
        {
            if ( field == null ) throw new ArgumentNullException(fieldName, $"{fieldName} cannot be null.");
        }
    }
}
