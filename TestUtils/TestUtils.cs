using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestUtils
{
    public static class TestUtils
    {
        public static string RandomString(int length = 32)
        {
            var count = (length + 31) / 32;
            var stringBuilder = new StringBuilder(length);

            Range(count, _ => stringBuilder.Append(Guid.NewGuid().ToString("N")));

            return stringBuilder.ToString().Substring(0, length);
        }

        public static int RandomInt() => random.Next();
        public static int RandomInt(int maxValue) => random.Next(maxValue);
        public static int RandomInt(int minValue, int maxValue) => random.Next(minValue, maxValue);
        
        public static long RandomLong() => BitConverter.ToInt64(new byte[8].Build(t => random.NextBytes(t)), 0);
        public static long RandomLong(long maxValue) => RandomLong(long.MinValue, maxValue);
        public static long RandomLong(long min, long max) => Math.Abs(RandomLong() % (max - min)) + min;

        public static DateTime RandomDate() => new DateTime(1900, 1, 1).AddDays(Math.Abs(RandomInt()) % (365 * 200));

        public static double RandomDouble() => random.NextDouble() * RandomInt();

        public static double RandomDouble(double minValue, double maxValue) => random.NextDouble() * (maxValue - minValue) + minValue;

        public static T RandomEnum<T>() where T : Enum => (T) Enum.GetValues(typeof(T)).GetValue(RandomInt(0, Enum.GetValues(typeof(T)).Length));

        public static T RandomObject<T>() where T : class, new() => Activator.CreateInstance<T>()
            .Build(result => result.GetType().GetProperties().ToList().ForEach(propertyInfo =>
            {
                if (!propertyInfo.CanWrite) return;
                propertyInfo.SetValue(result, GenerateValue<T>(propertyInfo.PropertyType));
            }));

        public static List<T> RandomList<T>(int count = 10) where T : class, new() => Range(count, _ => RandomObject<T>());
        public static T RandomItem<T>(List<T> list) => list.ToArray()[RandomInt(0, list.Count)];

        public static List<T> Range<T>(int count, Func<int, T> func) => Range(count).Select(func.Invoke).ToList();
        public static List<T> Range<T>(int start, int end, Func<int, T> func) => Range(start, end).Select(func.Invoke).ToList();
        public static void Range(int count, Action<int> action) => Range(count).ForEach(action.Invoke);
        public static void Range(int start, int end, Action<int> action) => Range(start, end).ForEach(action.Invoke);
        public static List<int> Range(int count) => Range(1, count);
        public static List<int> Range(int start, int end) => new List<int>().Build(result =>
        {
            while (start <= end) result.Add(start++);
        });
        
        public static T Build<T>(this T t, Action<T> action)
        {
            action.Invoke(t);
            return t;
        }
        public static List<T> BuildAll<T>(this List<T> list, Action<T> action) where T : class
        {
            list.ForEach(action.Invoke);
            return list;
        }

        public static List<T> AsList<T>(this T t) => new List<T> {t};

        private static Random random => new Random(randomSeed);
        private static int randomSeed => Guid.NewGuid().GetHashCode();
        private static object GenerateValue<T>(Type propertyType) where T : class, new()
        {
            bool IsNullableType(Type type, string typeName) => type.IsGenericType &&
                                                               type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                                                               type.GetGenericArguments()[0].Name == typeName;

            return propertyType switch
            {
                { } type when type.Name == nameof(Int32) || IsNullableType(type, nameof(Int32)) => RandomInt(),
                { } type when type.Name == nameof(Int64) || IsNullableType(type, nameof(Int64)) => RandomLong(),
                { } type when type.Name == nameof(String) || IsNullableType(type, nameof(String)) => RandomString(),
                { } type when type.Name == nameof(Boolean) || IsNullableType(type, nameof(Boolean)) => RandomInt() % 2 == 0,
                { } type when type.Name == nameof(Decimal) || IsNullableType(type, nameof(Decimal)) => Convert.ToDecimal(RandomDouble()),
                { } type when type.Name == nameof(DateTime) || IsNullableType(type, nameof(DateTime)) => RandomDate(),
                { } type when type.IsEnum => Enum.GetValues(propertyType).GetValue(RandomInt(0, Enum.GetValues(propertyType).Length)),
                _ => null
            };
        }
    }
}