using fightnight.Server.Interfaces.Auth;

namespace MetInProximityBack.Factories
{
    public class OAuthProviderFactory
    {
        private readonly IEnumerable<IOAuthProvider> _providers;

        public OAuthProviderFactory(IEnumerable<IOAuthProvider> providers)
        {
            _providers = providers;
        }

        public IOAuthProvider GetProvider(string providerName)
        {
            IOAuthProvider provider = _providers.FirstOrDefault(p => p.ProviderName.Equals(providerName));
            if (provider == null)
            {
                throw new ArgumentException("Unsupported Provider");
            }
            return provider;
        }
    }
}
