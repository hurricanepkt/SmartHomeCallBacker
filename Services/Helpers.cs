namespace Services;

public static class Helpers {

    public static string DecodeBase64(this string base64) {
        var base64Bytes = System.Convert.FromBase64String(base64);
        return System.Text.Encoding.UTF8.GetString(base64Bytes);
    }
}