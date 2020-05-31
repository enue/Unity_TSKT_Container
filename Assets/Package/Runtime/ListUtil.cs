using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT
{
    public static class ListUtil
    {
        public static int RemoveAll<T>(List<T> list, T item)
        {
            var removedCount = 0;
            var index = list.Count - 1;
            while(index >= 0)
            {
                index = list.LastIndexOf(item, index);
                if (index < 0)
                {
                    break;
                }
                list.RemoveAt(index);
                ++removedCount;
                --index;
            }
            return removedCount;
        }
    }
}