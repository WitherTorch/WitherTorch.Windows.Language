using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WitherTorch.Windows.Language
{
    public readonly struct ResourcesData
    {
        private readonly Assembly _assembly;
        private readonly CultureInfo _culture;
        private readonly string _resourceName;

        public CultureInfo Culture
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _culture;
        }

        public ResourcesData(Assembly assembly, CultureInfo culture, string resourceName)
        {
            _assembly = assembly;
            _culture = culture;
            _resourceName = resourceName;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Stream OpenResourceStream()
            => _assembly.GetManifestResourceStream(_resourceName)!;
    }

    public static class Resources
    {
        public static IEnumerable<ResourcesData> GetLanguageResources()
        {
            const string ResourceNameHeader = "WitherTorch.Windows.Language.Data.";
            const string ResourceNameFooter = ".json";

            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] names = assembly.GetManifestResourceNames();
            if (names.Length == 0)
                yield break;

            foreach (string name in names)
            {
                if (!name.StartsWith(ResourceNameHeader) || !name.EndsWith(ResourceNameFooter))
                    continue;
                CultureInfo culture;
                try
                {
                    culture = CultureInfo.GetCultureInfo(name.Substring(ResourceNameHeader.Length, 
                        name.Length - ResourceNameHeader.Length - ResourceNameFooter.Length));
                }
                catch (Exception)
                {
                    continue;
                }
                yield return new ResourcesData(assembly, culture, name);
            }
        }
    }
}
