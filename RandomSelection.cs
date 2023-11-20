using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public static class RandomSelection
{
	public static int FlagCount<T>(T flags) where T : struct, IConvertible
	{
		int f = flags.ToInt32(null);
		int count = 0;
		for(int i = 0; i < 32; i++)
		{
			if((f & (1 << i)) != 0)
			{
				count++;
			}
		}
		return count;
	}
	public static T PickEnum<T>(T flags, RandomNumberGenerator rng) where T : struct, IConvertible
	{
		int f = flags.ToInt32(null);
		if(f == 0)
		{
			return flags;
		}
		
		int count = FlagCount<T>(flags);
		if(count == 1)
		{
			return flags;
		}
		
		int pickIndex = rng.RandiRange(0, count - 1);
		for(int i = 0; i < 32; i++)
		{
			if((f & (1 << i)) != 0)
			{
				pickIndex--;
				if(pickIndex < 0)
				{
					return (T)(object)(1 << i);
				}
			}
		}
		
		// Should not reach this point
		throw new Exception();
	}
	
	public static T Pick<T>(IList<T> list, RandomNumberGenerator rng)
	{
		return list[rng.RandiRange(0, list.Count - 1)];
	}
	
	public static T[] PickN<T>(IList<T> list, int n, RandomNumberGenerator rng)
	{
		T[] result = new T[n];
		for(int i = 0; i < n; i++)
		{
			result[i] = list[rng.RandiRange(0, list.Count - 1)];
		}
		return result;
	}
	public static T[] PickNNoRepeats<T>(IList<T> list, int n, RandomNumberGenerator rng)
	{
		if(list.Count < n)
		{
			throw new ArgumentException();
		}
		
		int[] indices = new int[list.Count];
		for(int i = 0; i < indices.Length; i++)
		{
			indices[i] = i;
		}
		ShuffleInPlace(indices, rng);
		
		T[] result = new T[n];
		for(int i = 0; i < n; i++)
		{
			result[i] = list[indices[i]];
		}
		return result;
	}
	
	public static void ShuffleInPlace(IList list, RandomNumberGenerator rng)
	{
		for(int i = 0; i < list.Count - 1; i++)
		{
			int j = rng.RandiRange(i, list.Count - 1);
			object temp = list[i];
			list[i] = list[j];
			list[j] = temp;
		}
	}
	public static T[] GetShuffled<T>(IList<T> list, RandomNumberGenerator rng)
	{
		T[] result = new T[list.Count];
		list.CopyTo(result, 0);
		ShuffleInPlace(result, rng);
		return result;
	}
}
