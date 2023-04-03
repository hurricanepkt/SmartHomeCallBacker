using System.Collections;

namespace Infrastructure;

public static class TheConfiguration {
    private static IDictionary _data = new Dictionary<string,string>();

    public static void Setup(IDictionary data) {
        _data = data;
        // Display the details with key and value
        foreach (DictionaryEntry i in _data)
        {
            Console.WriteLine("{0}:{1}", i.Key, i.Value);
        }

    }

    public static string CustomString => (_data.Contains("CustomString") ? _data["CustomString"].ToString() : "key not found") ?? "-";
    public static int MaxFailures => (_data.Contains("MaxFailures") ? int.Parse(_data["MaxFailures"].ToString()) : 10);

}