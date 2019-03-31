using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
namespace InternalAssets.Scripts.Extensions
{
	public static class CollectionExtension
	{
		private static Random random = new Random();
		
		public static IEnumerable<T> GetRange<T>(this LinkedList<T> parent, List<T> result, T node, int count, bool next = true, int current = 0)
		{
			if (current >= count)
			{
				return result;
			}
			
			var start = parent.Find(node);
			if (start == null)
			{
				Debug.Log("Cant find start node");
				return null;
			}
			
			var find = next ? start.Next : start.Previous;
			if (find == null)
			{
				return result;
			}
			
			result.Add(find.Value);
			// ReSharper disable once TailRecursiveCall
			return GetRange(parent, result, find.Value, count, next, current + 1);
		}
     
		public static T GetRandomElement<T>(this IEnumerable<T> list)
		{
			// If there are no elements in the collection, return the default value of T
			if (list.Count() == 0)
				return default(T);
 
			return list.ElementAt(random.Next(list.Count()));
		}
		
		public static void Shuffle<T>(this IList<T> list)  
		{  
			int n = list.Count;  
			while (n > 1) {  
				n--;  
				int k = random.Next(n + 1);  
				T value = list[k];  
				list[k] = list[n];  
				list[n] = value;  
			}  
		}
	}
}