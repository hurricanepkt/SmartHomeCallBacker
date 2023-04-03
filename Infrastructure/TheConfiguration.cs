using System.Collections;

namespace Infrastructure;

public class TheConfiguration {

    private static ILogger _logger { get => StaticLoggerFactory.GetStaticLogger<TheConfiguration>(); }
    private static IDictionary _data = new Dictionary<string,string>();

    public static void Setup(IDictionary data, WebApplication app ) {
        _data = data;
        // Display the details with key and value
        foreach (DictionaryEntry i in _data)
        {
            _logger.LogInformation("{0}:{1}", i.Key, i.Value);
        }
    }

    public static string CustomString => GetStringValueByKeySafe("CustomString", "key not found");
    public static int MaxFailures => GetIntValueByKeySafe("MaxFailures", 10);

    private static string GetStringValueByKeySafe(string keyName, string defaultValue) {
        ArgumentNullException.ThrowIfNull(_data, nameof(_data));        
        if (_data.Contains(keyName)) {
            try {
                return ((string?)_data[keyName]) ?? defaultValue;
            } catch {
                return defaultValue;
            }
        }
        return defaultValue;
    }
    private static int GetIntValueByKeySafe(string keyName, int defaultValue) {
        try {
            return int.Parse(GetStringValueByKeySafe(keyName, String.Empty));
        } catch {
            return defaultValue;
        }
        
    }
}