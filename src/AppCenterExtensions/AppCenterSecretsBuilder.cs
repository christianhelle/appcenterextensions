using System;
using System.Text;

namespace AppCenterExtensions
{
    /// <summary>
    /// A fluent API for building the AppCenter API secrets
    /// </summary>
    public class AppCenterSecretsBuilder
    {
        private string appleSecret;
        private string androidSecret;

        /// <summary>
        /// Set the API secret for the iOS app
        /// </summary>
        /// <param name="secret">iOS secret</param>
        /// <returns>Returns self (Fluent API)</returns>
        public AppCenterSecretsBuilder SetAppleSecret(string secret)
        {
            appleSecret = secret ?? throw new ArgumentNullException(nameof(secret));
            return this;
        }

        /// <summary>
        /// Sets the API secret for the Android app
        /// </summary>
        /// <param name="secret">Android secret</param>
        /// <returns>Returns self (Fluent API)</returns>
        public AppCenterSecretsBuilder SetAndroidSecret(string secret)
        {
            androidSecret = secret ?? throw new ArgumentNullException(nameof(secret));
            return this;
        }

        /// <summary>
        /// Builds a single AppCenter API secret string
        /// </summary>
        /// <returns>This returns a string like ios={secret};android={secret}</returns>
        public string Build()
        {
            var builder = new StringBuilder();
            
            if (!string.IsNullOrWhiteSpace(appleSecret))
                builder.Append($"ios={appleSecret};");
            
            if (!string.IsNullOrWhiteSpace(androidSecret))
                builder.Append($"android={androidSecret}");
            
            return builder.ToString();
        }

        /// <summary>
        /// Builds a single AppCenter API secret string
        /// </summary>
        /// <param name="appleSecret">iOS secret</param>
        /// <param name="androidSecret">Android secret</param>
        /// <returns>This returns a string like ios={secret};android={secret}</returns>
        public static string Build(string appleSecret, string androidSecret)
            => new AppCenterSecretsBuilder()
                .SetAppleSecret(appleSecret)
                .SetAndroidSecret(androidSecret)
                .Build();
    }
}