using System.Globalization;
using Newtonsoft.Json;

namespace Exchange.Api
{
    public partial class RequestBuilder
    {
        internal sealed class FormatNumbersAsTextConverter : JsonConverter
        {
            public override bool CanRead => false;
            public override bool CanWrite => true;
            public override bool CanConvert(Type type) => IsNumeric(type);

            public override void WriteJson(
                JsonWriter writer, object value, JsonSerializer serializer)
            {
                var number = value;

                if (value is double dbl)
                {
                    writer.WriteValue(dbl.ToString(CultureInfo.InvariantCulture));
                }
                else if (value is decimal dcl)
                {
                    writer.WriteValue(dcl.ToString(CultureInfo.InvariantCulture));
                }
                else if (value is float flt)
                {
                    writer.WriteValue(flt.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    writer.WriteValue(number.ToString());
                }
            }

            public override object ReadJson(
                JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
            {
                typeof(int),  typeof(double),  typeof(decimal),
                typeof(long), typeof(short),   typeof(sbyte),
                typeof(byte), typeof(ulong),   typeof(ushort),
                typeof(uint), typeof(float)
            };

            private bool IsNumeric(Type myType)
            {
                return NumericTypes.Contains(Nullable.GetUnderlyingType(myType) ?? myType);
            }
        }
    }
}