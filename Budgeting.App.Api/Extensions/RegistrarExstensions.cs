// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Registrars;

namespace Budgeting.App.Api.Extensions
{
    /// <summary>
    /// Represents an extension class for registering services and pipeline components in a web application.
    /// </summary>
    public static class RegistrarExstensions
    {
        /// <summary>
        /// Registers services by scanning the specified type and invoking the RegisterServices method of each found registrar.
        /// </summary>
        /// <param name="builder">The WebApplicationBuilder instance.</param>
        /// <param name="scanningType">The type used for scanning registrars.</param>
        public static void RegisterServices(this WebApplicationBuilder builder, Type scanningType)
        {
            // Get all registrars that implement the IWebApplicationBuilderRegistrar interface from the specified scanning type
            var registrars = GetRegistrars<IWebApplicationBuilderRegistrar>(scanningType);

            // Invoke the RegisterServices method of each registrar
            foreach (var registrar in registrars)
            {
                registrar.RegisterServices(builder);
            }
        }

        /// <summary>
        /// Registers pipeline components by scanning the specified type and invoking the RegisterPipelineComponents method of each found registrar.
        /// </summary>
        /// <param name="app">The WebApplication instance.</param>
        /// <param name="scanningType">The type used for scanning registrars.</param>
        public static void RegisterPiplineComponents(this WebApplication app, Type scanningType)
        {
            // Get all registrars that implement the IWebApplicationRegistrar interface from the specified scanning type
            var registrars = GetRegistrars<IWebApplicationRegistrar>(scanningType);

            // Invoke the RegisterPipelineComponents method of each registrar
            foreach (var registrar in registrars)
            {
                registrar.RegisterPipelineComponents(app);
            }
        }

        /// <summary>
        /// Gets registrars of the specified type by scanning the assembly of the scanning type and creating instances of those registrars.
        /// </summary>
        /// <typeparam name="T">The type of registrar to retrieve.</typeparam>
        /// <param name="scanningType">The type used for scanning registrars.</param>
        /// <returns>An IEnumerable collection of registrars of the specified type.</returns>
        private static IEnumerable<T> GetRegistrars<T>(Type scanningType) where T : IRegistrar
        {
            return scanningType.Assembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(T)) && !t.IsAbstract && !t.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<T>();
        }
    }
}
