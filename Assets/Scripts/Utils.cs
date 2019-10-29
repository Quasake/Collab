using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	public static int GetRandomInteger (int min, int max) {
		return Constants.RANDOM.Next(min, max + 1);
	}
}
