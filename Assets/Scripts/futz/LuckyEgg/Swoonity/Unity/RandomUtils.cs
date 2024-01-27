using System;
using Swoonity.CSharp;
using UnityEngine;
using uRandom = UnityEngine.Random;
using V3 = UnityEngine.Vector3;

namespace Swoonity.Unity
{
public static class RandomUtils
{
	/// 0..max-1 (max exclusive)
	public static int Random(this int max) => uRandom.Range(0, max);

	/// min..max-1 (min inclusive, max exclusive)
	public static int Random(this int min, int max) => uRandom.Range(min, max);

	/// min..max-1 (min inclusive, max exclusive)
	public static int Random(this (int min, int max) range) => uRandom.Range(range.min, range.max);

	/// min..max (min inclusive, max inclusive)
	public static int RandomIncl(this (int min, int max) range) => uRandom.Range(range.min, range.max + 1);

	public static float Random(this float max) => max * uRandom.value;
	public static float Random(this float min, float max) => uRandom.Range(min, max);
	public static bool RandomBool(this float chance) => uRandom.value <= chance;

	/// min..max-1 (min inclusive, max exclusive)
	public static float Random(this (float min, float max) range) => uRandom.Range(range.min, range.max);

	public static Vector2 RandomVector2(this float max) => new(max.Random(), max.Random());

	public static V3 RandomVector3(this float max) => new(max.Random(), max.Random(), max.Random());

	public static Quaternion RandomQuaternion(this float max)
		=> Quaternion.Euler(max.Random(), max.Random(), max.Random());

	public static Quaternion RandomRotationY(this float max) => Quaternion.Euler(0, max.Random(), 0);


	/// max.x.Random(), max.y.Random(), max.z.Random()
	public static V3 Random(this V3 max) => new(max.x.Random(), max.y.Random(), max.z.Random());

	public static V3 RandomAroundXZ(this V3 pos, float distance)
	{
		var angleRads = Random(Maths.TAU);
		return pos + new V3(Mathf.Cos(angleRads), 0, Mathf.Sin(angleRads)) * distance;
	}

	/// -v..v
	public static float RandomPlusMinus(this float v) => uRandom.Range(-v, v);

	/// new Vector3(-v.x...v.x, -v.y...v.y, -v.z...v.z)
	public static V3 RandomPlusMinus(this V3 v)
		=> new(
			uRandom.Range(-v.x, v.x),
			uRandom.Range(-v.y, v.y),
			uRandom.Range(-v.z, v.z)
		);

	public static V3 RandomVariance(this V3 variance)
		=> new(
			variance.x.RandomVariance(),
			variance.y.RandomVariance(),
			variance.z.RandomVariance()
		);


	/// Assumes float is less than or equal to 1. Returns true if random value is lower than or equal to float.
	public static bool RandomChance(this float thisFloat) => uRandom.value <= thisFloat;

	/// Returns random between (-float/2 and float/2) + plus
	/// 10.RandomVariance(100) = 95...105
	// public static float RandomVariance(this float thisFloat, float plus = 0)
	// 	=> plus + uRandom.Range(-thisFloat / 2f, thisFloat / 2f);

	/// e.g. 10.RandomVariance(100) => 95..105 
	public static float RandomVariance(this float variance, float around) => around + variance.RandomVariance();

	/// e.g. 10.RandomVariance() => -5..5
	public static float RandomVariance(this float variance)
	{
		var half = variance / 2f;
		return uRandom.Range(-half, half);
	}


	/// flip a coin and choose 1 
	public static T CoinFlip<T>(this (T, T) tuple) => uRandom.value <= .5f ? tuple.Item1 : tuple.Item2;


	public static Color RandomColor(this Color color, bool randomAlpha = false)
	{
		return new Color(
			uRandom.value,
			uRandom.value,
			uRandom.value,
			randomAlpha ? uRandom.value : 1
		);
	}


	public static V3 RandomPoint(this Bounds bounds) => bounds.min + bounds.size.Random();
}

[Serializable]
public class RandomChance
{
	[Range(0, 100)] public float StartingChance;
	[Range(0, 100)] public float IncreasePerFail;

	public float CurrentChance;

	public RandomChance(float startingChance, float increasePerRoll = 0)
	{
		StartingChance = startingChance;
		CurrentChance = StartingChance;
		IncreasePerFail = increasePerRoll;
	}

	public bool Roll()
	{
		if (uRandom.value * 100 <= CurrentChance) {
			Reset();
			return true;
		}

		CurrentChance += IncreasePerFail;
		return false;
	}

	public bool Roll(float increasePerFailOverride)
	{
		if (uRandom.value * 100 <= CurrentChance) {
			Reset();
			return true;
		}

		CurrentChance += increasePerFailOverride;
		return false;
	}

	public void Reset()
	{
		CurrentChance = StartingChance;
	}

	public void Reset(float startingChance, float increasePerRoll = 0)
	{
		StartingChance = startingChance;
		CurrentChance = StartingChance;
		IncreasePerFail = increasePerRoll;
	}


	// public static void Test(float startingChance, float increasePerFail, int numberOfTests) {
	// 	var tester = new RandomChance(startingChance, increasePerFail);
	// 	int successes = 0;
	// 	int currentFailStreak = 0;
	// 	int highestFailStreak = 0;
	// 	int averageFailStreak = 0;
	// 	int numberOfStreaks = 0;
	// 	numberOfTests.Times(
	// 		_ => {
	// 			if (tester.Roll()) {
	// 				if (currentFailStreak > highestFailStreak) {
	// 					highestFailStreak = currentFailStreak;
	// 				}
	//
	// 				if (currentFailStreak > 1) {
	// 					averageFailStreak += currentFailStreak;
	// 					++numberOfStreaks;
	// 				}
	//
	// 				currentFailStreak = 0;
	// 				++successes;
	// 			}
	// 			else {
	// 				++currentFailStreak;
	// 			}
	// 		}
	// 	);
	//
	// 	Debug.Log(
	// 		"Percent Test Results - Start: {0}% + {1}%: {2}%.  Highest fail streak: {3}. Avg: {4}"
	// 		   .Fmt(
	// 				startingChance,
	// 				increasePerFail,
	// 				(successes / (float)numberOfTests) * 100,
	// 				highestFailStreak,
	// 				averageFailStreak / (float)numberOfStreaks
	// 			)
	// 	);
	// }

	// public void Test(int numberOfTests) {
	// 	Test(StartingChance, IncreasePerFail, numberOfTests);
	// }
}
}