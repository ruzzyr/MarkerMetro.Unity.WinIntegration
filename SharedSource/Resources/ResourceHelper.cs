using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if NETFX_CORE
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using System.Diagnostics;
using Windows.ApplicationModel.Resources;
#elif WINDOWS_PHONE
using Microsoft.Phone.Tasks;
using System.Xml.Linq;
using System.Windows;
using Microsoft.Phone.Info;
using Windows.ApplicationModel.Store;
using System.Resources;
using System.Globalization;
using System.Reflection;
#endif


namespace MarkerMetro.Unity.WinIntegration.Resources
{
    /// <summary>
    /// Resource Helper
    /// </summary>
    public class ResourceHelper
    {

        private static ResourceHelper _instance;
        private static readonly object _sync = new object();
#if WINDOWS_PHONE
        private static ResourceManager _resourceMan;
#elif NETFX_CORE
        private static ResourceLoader _resourceLoader;
#endif
        private static string _resourceBaseName;

        public static ResourceHelper GetInstance(string resourceBaseName = "")
        {
            lock (_sync)
            {
                if (_instance == null)
                    _instance = new ResourceHelper();
            }
            _resourceBaseName = resourceBaseName;
            return _instance;
        }

#if WINDOWS_PHONE

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        private static ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(_resourceMan, null))
                {
                    if (Dispatcher.AppAssembly == null)
                    { 
                        throw new NullReferenceException("Dispatcher's AppAssembly has not been set, unable to get resources for Windows Phone");
                    };
                    ResourceManager temp = new ResourceManager(_resourceBaseName, Dispatcher.AppAssembly);
                    _resourceMan = temp;
                }
                return _resourceMan;
            }
        }
#elif NETFX_CORE

        /// <summary>
        ///   Returns the cached ResourceLoader instance used by this class.
        /// </summary>
        private static ResourceLoader ResourceLoader
        {
            get
            {
                if (object.ReferenceEquals(_resourceLoader, null))
                {
                    ResourceLoader temp = new ResourceLoader();
                    _resourceLoader = temp;
                }
                return _resourceLoader;
            }
        }

#endif


        /// <summary>
        /// Returns a localized value based on a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string resourceKey)
        {
            string val = String.Empty;
#if NETFX_CORE
            ResourceLoader rl = new ResourceLoader(); 
            try
            {
                val = rl.GetString(resourceKey);
            }
            catch
            {
                val = String.Empty;
            }
#elif WINDOWS_PHONE
            val = ResourceManager.GetString(resourceKey);
#endif
            return val;
        }

    }
}