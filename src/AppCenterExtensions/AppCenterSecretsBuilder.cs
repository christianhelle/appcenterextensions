using System;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions
{
    public class AppCenterSecretsBuilder
    {
        private string appleSecret;
        private string androidSecret;

        public AppCenterSecretsBuilder SetAppleSecret(string secret)
        {
            appleSecret = secret ?? throw new ArgumentNullException(nameof(secret));
            return this;
        }

        public AppCenterSecretsBuilder SetAndroidSecret(string secret)
        {
            androidSecret = secret ?? throw new ArgumentNullException(nameof(secret));
            return this;
        }

        public string Build() => $"ios={appleSecret};android={androidSecret}";

        public static string Build(string appleSecret, string androidSecret)
            => new AppCenterSecretsBuilder()
                .SetAppleSecret(appleSecret)
                .SetAndroidSecret(androidSecret)
                .Build();
    }
}