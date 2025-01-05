using System.Collections;

internal static class RequestBuilderHelper
{
    // todo: handle with OOP.
    internal static string DictionaryToString(Dictionary<string, object> dict)
    {
        var str = "";
        foreach (var key in dict.Keys.OrderBy(x => x)) // order keys asc
        {
            var value = dict[key];
            if (value is IEnumerable en)
            {
                value = "";
                foreach (var item in en)
                {
                    value += item.ToString();
                }
            }

            str += key + value;
        }

        return str;
    }

    public static Dictionary<string, object> ObjectToDictionary(this object inputObject)
    {
        return inputObject.GetType().GetProperties().ToDictionary(property => property.Name, property => property.GetValue(inputObject) ?? string.Empty);
    }
}