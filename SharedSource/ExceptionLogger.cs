using MarkerMetro.Unity.WinIntegration.Resources;
using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
#if NETFX_CORE
using Mindscape.Raygun4Net;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using System.Collections;
#elif WINDOWS_PHONE
using Mindscape.Raygun4Net;
using Microsoft.Phone.Tasks;
using System.Xml.Linq;
using System.Windows;
using Microsoft.Phone.Info;
using Windows.ApplicationModel.Store;
using Windows.Networking.Connectivity;
#endif

namespace MarkerMetro.Unity.WinIntegration
{
    /// <summary>
    /// Exception Logger 
    /// </summary>
    public class ExceptionLogger
    {
        static ExceptionLogger _instance;
        static readonly object _sync = new object();

#if NETFX_CORE || WINDOWS_PHONE
        Lazy<RaygunClient> _logger;
#endif

        public static ExceptionLogger Instance
        {
            get
            {
                lock (_sync)
                {
                    if (_instance == null)
                        _instance = new ExceptionLogger();
                }
                return _instance;
            }
        }

        public static void Initialize(string apiKey)
        {
            lock (_sync)
            {
                if (_instance == null)
                    _instance = new ExceptionLogger();

#if NETFX_CORE || WINDOWS_PHONE
                _instance._logger = new Lazy<RaygunClient>(() => BuildRaygunClient(apiKey));
#else
                Debug.WriteLine("ExceptionLogger not supported");
#endif
            }
        }

#if NETFX_CORE || WINDOWS_PHONE
        static RaygunClient BuildRaygunClient(string apiKey)
        {
            try
            {
                string version = null, user = null;

                version = Helper.Instance.GetAppVersion();
                try
                {
                    user = Helper.Instance.GetUserDeviceId();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to get UserDeviceId: {0}", ex);
                }

                return new RaygunClient(apiKey)
                {
                    ApplicationVersion = version,
                    User = user,
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to BuildRaygunClient", ex);

                throw;
            }
        }
#endif

        public void Send(Exception ex)
        {
#if NETFX_CORE || WINDOWS_PHONE
            if(_logger!=null)
                _logger.Value.Send(ex);
#else
            // Debug.WriteLine("ExceptionLogger not supported: {0}", ex);
#endif
        }

        public void Send(string message, string stackTrace)
        {
#if NETFX_CORE || WINDOWS_PHONE
            if(_logger!=null)
                _logger.Value.Send(new WrappedException(message, stackTrace));
#else
            // Debug.WriteLine("ExceptionLogger not supported: {0}", ex);
#endif
        }
    }
}