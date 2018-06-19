// Credit: https://github.com/Kritsu/ExtractorSharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace PackageManager
{
    public static class Arrays
    {

        public static T Find<T>(this T[] array, Predicate<T> match) => Array.Find(array, match);

        public static T[] Concat<T>(this T[] arr1, T[] arr2)
        {
            var newArray = new T[arr1.Length + arr2.Length];
            Buffer.BlockCopy(arr1, 0, newArray, 0, arr1.Length);
            Buffer.BlockCopy(arr2, 0, newArray, arr1.Length, arr2.Length);
            return newArray;
        }

        public static T[][] Split<T>(this T[] data, T[] pattern)
        {
            var last = 0;
            var list = new List<T[]>();
            for (var i = 0; i < data.Length; i++)
            {
                var j = i;
                while (j < data.Length && j - i < pattern.Length && (Equals(data[j], pattern[j - i])))
                {
                    j++;
                }
                if (j - i == pattern.Length)
                {
                    var temp = new T[j - last];
                    Buffer.BlockCopy(data, last, temp, 0, temp.Length);
                    list.Add(temp);
                    last = j;
                    continue;
                }
                i = j;
            }
            var arr = new T[data.Length - last];
            Buffer.BlockCopy(data, last, arr, 0, arr.Length);
            list.Add(arr);
            return list.ToArray();
        }



        /// <summary>
        /// 安全插入 当插入的位置不在于集合的区间时，改为添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="t"></param>
        public static void InsertAt<T>(this List<T> list, int index, IEnumerable<T> t)
        {
            if (index > list.Count)
            {
                list.AddRange(t);
            }
            else if (index < 0)
            {
                list.InsertRange(0, t);
            }
            else
            {
                list.InsertRange(index, t);
            }
        }
    }
}
