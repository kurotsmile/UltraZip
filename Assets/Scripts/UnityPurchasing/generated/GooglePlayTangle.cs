// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("jkyr1G1vblI+jxZNbQLvAq3V+cWw0QcSxwEfLqT1ZqO5gsPJsy8ow8BcJG/qdzWYgcRpMKl3wrYU5ZVbloWu9A3dWmIrYOT3ITh8OzombV2OZdU0uBKLDenRxbMyre9sY5cziPm7f+2vMM8Cud+BaHnN2IQBUgtypmKiycL4VYKG0f39VytJzUSX5Q9bgN6qOPIQWKHNfApuyUjdUI62sh74AYm613n2iFpAKeD1w4BX0Ga3qGUIZJBC6+Bsgteqlj9n9kjGx9w3hQYlNwoBDi2BT4HwCgYGBgIHBIUGCAc3hQYNBYUGBgfMAutp+7GrIRDeP8pzJqnxrw2HNUauaP+BoK0kPbp59L8UrxdLwPsA+7PitfjnsgS/y88x+x1bPAUEBgcG");
        private static int[] order = new int[] { 6,11,7,5,7,9,7,12,8,10,12,11,13,13,14 };
        private static int key = 7;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
