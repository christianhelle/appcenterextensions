using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    internal class MockPlatformServices : IPlatformServices
    {
        private readonly Action<Action> _invokeOnMainThread;
        private readonly Action<Uri> _openUriAction;
        private readonly Func<Uri, CancellationToken, Task<Stream>> _getStreamAsync;

        public MockPlatformServices(Action<Action> invokeOnMainThread = null, Action<Uri> openUriAction = null,
            Func<Uri, CancellationToken, Task<Stream>> getStreamAsync = null)
        {
            _invokeOnMainThread = invokeOnMainThread;
            _openUriAction = openUriAction;
            _getStreamAsync = getStreamAsync;
        }

        public string GetMD5Hash(string input)
        {
            throw new NotImplementedException();
        }

        static int hex(int v)
        {
            if (v < 10)
                return '0' + v;
            return 'a' + v - 10;
        }

        public double GetNamedSize(NamedSize size, Type targetElement, bool useOldSizes)
        {
            switch (size)
            {
                case NamedSize.Default:
                    return 10;
                case NamedSize.Micro:
                    return 4;
                case NamedSize.Small:
                    return 8;
                case NamedSize.Medium:
                    return 12;
                case NamedSize.Large:
                    return 16;
                default:
                    throw new ArgumentOutOfRangeException(nameof(size));
            }
        }

        public void OpenUriAction(Uri uri)
        {
            if (_openUriAction != null)
                _openUriAction(uri);
            else
                throw new NotImplementedException();
        }

        public void QuitApplication()
        {
            throw new NotImplementedException();
        }

        public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint) 
            => default(SizeRequest);

        public bool IsInvokeRequired => false;

        public string RuntimePlatform { get; set; }

        public void BeginInvokeOnMainThread(Action action)
        {
            if (_invokeOnMainThread == null)
                action();
            else
                _invokeOnMainThread(action);
        }

        public Ticker CreateTicker()
        {
            return new MockTicker();
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            Timer timer = null;

            void OnTimeout(object o) => BeginInvokeOnMainThread(() =>
            {
                if (callback()) return;

                timer?.Dispose();
            });

            timer = new Timer(OnTimeout, null, interval, interval);
        }

        public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (_getStreamAsync == null)
                throw new NotImplementedException();
            return _getStreamAsync(uri, cancellationToken);
        }

        public Assembly[] GetAssemblies()
        {
            return new Assembly[0];
        }

        public IIsolatedStorageFile GetUserStoreForApplication()
        {
            throw new NotImplementedException();
        }
    }
}