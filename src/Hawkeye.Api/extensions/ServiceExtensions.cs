using System;
using System.ComponentModel.Design;

using Hawkeye.ComponentModel;

namespace Hawkeye
{
    /// <summary>
    /// Extension method allowing to use generic forms of <c>GetService</c> on <see cref="IServiceProvider"/> instances,
    /// and generic forms of <c>AddService</c> and <c>RemoveService</c> on <see cref="IServiceContainer"/> instances.
    /// </summary>
    public static class ServiceExtensions
    {
        #region IServiceProvider methods

        /// <summary>
        /// Obtient une instance de service.
        /// </summary>
        /// <typeparam name="T">Type de service à récupérer.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>L'instance de service ou <b>null</b></returns>
        public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Obtient une instance de service.
        /// </summary>
        /// <typeparam name="T">Type de service à récupérer.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mandatory">Si à <c>true</c>, lève une
        /// exception <see cref="Hawkeye.ComponentModel.ServiceNotFoundException{T}"/>
        /// dans le cas où le service demandé n'a pas pu être trouvé.</param>
        /// <returns>L'instance de service ou <b>null</b></returns>
        public static T GetService<T>(this IServiceProvider serviceProvider, bool mandatory) where T : class
        {
            T t = serviceProvider.GetService<T>();
            if (t == null)
            {
                if (mandatory) throw new ServiceNotFoundException<T>();
                else return null;
            }

            return t;
        }

        #endregion

        #region IServiceContainer methods

        /// <summary>Ajoute le service spécifié au conteneur de services.</summary>
        /// <typeparam name="T">Type de service à ajouter.</typeparam>
        /// <param name="serviceContainer">The service container.</param>
        /// <param name="callback">Objet de rappel utilisé pour créer le service. Cela permet à 
        /// un service d'être déclaré comme disponible, mais retarde la création de l'objet 
        /// jusqu'à ce que le service soit demandé.</param>
        public static void AddService<T>(this IServiceContainer serviceContainer, ServiceCreatorCallback callback) where T : class
        {
            serviceContainer.AddService(typeof(T), callback);
        }

        /// <summary>Ajoute le service spécifié au conteneur de services et promeut 
        /// éventuellement le service vers les conteneurs de services parents éventuels.</summary>
        /// <typeparam name="T">Type de service à ajouter.</typeparam>
        /// <param name="serviceContainer">The service container.</param>
        /// <param name="callback">Objet de rappel utilisé pour créer le service. Cela permet à 
        /// un service d'être déclaré comme disponible, mais retarde la création de l'objet 
        /// jusqu'à ce que le service soit demandé.</param>
        /// <param name="promote"><b>true</b> pour promouvoir cette demande vers les 
        /// conteneurs de services parents éventuels ; sinon, <b>false</b>.</param>
        public static void AddService<T>(this IServiceContainer serviceContainer, ServiceCreatorCallback callback, bool promote) where T : class
        {
            serviceContainer.AddService(typeof(T), callback, promote);
        }

        /// <summary>Ajoute le service spécifié au conteneur de services.</summary>
        /// <typeparam name="T">Type de service à ajouter.</typeparam>
        /// <param name="serviceContainer">The service container.</param>
        /// <param name="serviceInstance">Instance du type de service à ajouter. 
        /// Cet objet doit implémenter le type indiqué par <typeparamref name="T"/> ou en hériter.</param>
        public static void AddService<T>(this IServiceContainer serviceContainer, T serviceInstance) where T : class
        {
            serviceContainer.AddService(typeof(T), serviceInstance);
        }

        /// <summary>Ajoute le service spécifié au conteneur de services et promeut 
        /// éventuellement le service vers les conteneurs de services parents éventuels.</summary>
        /// <typeparam name="T">Type de service à ajouter.</typeparam>
        /// <param name="serviceContainer">The service container.</param>
        /// <param name="serviceInstance">Instance du type de service à ajouter. 
        /// Cet objet doit implémenter le type indiqué par <typeparamref name="T"/> ou en hériter.</param>
        /// <param name="promote"><b>true</b> pour promouvoir cette demande vers les 
        /// conteneurs de services parents éventuels ; sinon, <b>false</b>.</param>
        public static void AddService<T>(this IServiceContainer serviceContainer, T serviceInstance, bool promote) where T : class
        {
            serviceContainer.AddService(typeof(T), serviceInstance, promote);
        }

        /// <summary>
        /// Supprime le type de service spécifié du conteneur de service
        /// et promeut éventuellement la demande vers les conteneurs de service parents.
        /// </summary>
        /// <typeparam name="T">Type de service à supprimer.</typeparam>
        /// <param name="serviceContainer">The service container.</param>
        /// <param name="promote"><b>true</b> pour promouvoir cette demande vers les
        /// conteneurs de services parents éventuels ; sinon, <b>false</b>.</param>
        public static void RemoveService<T>(this IServiceContainer serviceContainer, bool promote) where T : class
        {
            serviceContainer.RemoveService(typeof(T), promote);
        }

        /// <summary>Supprime le type de service spécifié du conteneur de services.</summary>
        /// <typeparam name="T">Type de service à supprimer.</typeparam>
        /// <param name="serviceContainer">The service container.</param>
        public static void RemoveService<T>(this IServiceContainer serviceContainer) where T : class
        {
            serviceContainer.RemoveService(typeof(T));
        }

        #endregion
    }
}
