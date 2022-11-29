//  Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DKBozkurt.Utils
{
    public static partial class DKBozkurtUtils
    {
        private static Camera _camera;
        
        /// <summary>
        /// Getting main camera.
        /// </summary>
        public static Camera Camera
        {
            get
            {
                if(_camera == null) _camera = UnityEngine.Camera.main;
                return _camera;
            }    
        }
        
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary =
            new Dictionary<float, WaitForSeconds>();

        /// <summary>
        /// Non-allocating WaitForSeconds. Avoids allocating more garbage.
        /// </summary>
        /// <param name="time">Time that we wanna wait.</param>
        /// <returns>After waiting for time seconds.</returns>
        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }
        
        /// <summary>
        /// Find world point of canvas elements respect to main camera.
        /// </summary>
        /// <param name="element"> Any canvas elements rect Transform to get position in 3D space</param>
        /// <returns></returns>
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
            return result;
        }
        
        /// <summary>
        /// Parse a float, return default if failed
        /// </summary>
        /// <param name="txt">String will get the parsed value on it.</param>
        /// <param name="_default">Float value to be converted into string. </param>
        /// <returns></returns>
        public static float Parse_Float(string txt, float _default) {
            float f;
            if (!float.TryParse(txt, out f)) {
                f = _default;
            }
            return f;
        }
        
        /// <summary>
        /// Parse a int, return default if failed
        /// </summary>
        /// <param name="txt">String will get the parsed value on it.</param>
        /// <param name="_default">Int value to be converted into string. </param>
        /// <returns></returns>
        public static int Parse_Int(string txt, int _default) {
            int i;
            if (!int.TryParse(txt, out i)) {
                i = _default;
            }
            return i;
        }
        
        /// <summary>
        /// Get vector output from input angle.
        /// </summary>
        /// <param name="angle"> angle = 0 -> 360</param>
        /// <returns>Direction vector.</returns>
        public static Vector3 GetVectorFromAngle(int angle) {
            
            float angleRad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
        
        /// <summary>
        /// Get vector output from input angle.
        /// </summary>
        /// <param name="angle"> angle = 0 -> 360</param>
        /// <returns>Direction vector.</returns>
        public static Vector3 GetVectorFromAngle(float angle) {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
        
        /// <summary>
        /// Get angle output from vector3 direction.
        /// </summary>
        /// <param name="dir">Direction.</param>
        /// <returns>Float angle.</returns>
        public static float GetAngleFromVectorFloat(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }
        
        /// <summary>
        /// Get angle output from vector3 direction.
        /// </summary>
        /// <param name="dir">Direction.</param>
        /// <returns>Float angle.</returns>
        public static int GetAngleFromVectorInt(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }
        
        /// <summary>
        /// Shakes main camera.
        /// </summary>
        /// <param name="intensity">Shake force.</param>
        /// <param name="timer">Shake duration.</param>
        public static void ShakeCamera(float intensity, float timer) {
            Vector3 lastCameraMovement = Vector3.zero;
            FunctionCaller.Create(delegate () {
                timer -= Time.unscaledDeltaTime;
                Vector3 randomMovement = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * intensity;
                Camera.main.transform.position = Camera.transform.position - lastCameraMovement + randomMovement;
                lastCameraMovement = randomMovement;
                return timer <= 0f;
            }, "CAMERA_SHAKE");
        }

        /// <summary>
        /// Getting random element of an array
        /// </summary>
        /// <param name="array">Array to get random element.</param>
        /// <typeparam name="T">Array type.</typeparam>
        /// <returns>Random Elements belongs to input array. </returns>
        public static T GetRandomFromArray<T>(T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
        
        /// <summary>
        /// Returns string hex value from decimal. 
        /// </summary>
        /// <param name="value">value 0->255.</param>
        /// <returns>00-FF</returns>
        public static string Dec_to_Hex(int value) {
            return value.ToString("X2");
        }

        // Returns 0-255
        
        /// <summary>
        /// Returns decimal value from hex value.  
        /// </summary>
        /// <param name="hex">00-FF</param>
        /// <returns>0->255</returns>
        public static int Hex_to_Dec(string hex) {
            return Convert.ToInt32(hex, 16);
        }
        
        /// <summary>
        /// Returns a hex string based on a number between 0->1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Dec01_to_Hex(float value) {
            return Dec_to_Hex((int)Mathf.Round(value*255f));
        }
        
        /// <summary>
        /// Returns a float between 0->1 
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static float Hex_to_Dec01(string hex) {
            return Hex_to_Dec(hex)/255f;
        }
        
        /// <summary>
        /// Get Random Color
        /// </summary>
        /// <returns> Random color.</returns>
        public static Color GetRandomColor() {
            return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        }
        
        /// <summary>
        /// Get Hex Color code ex: FF00FF 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns>6 letter color code.</returns>
        public static string GetStringFromColor(float r, float g, float b) {
            string red = Dec01_to_Hex(r);
            string green = Dec01_to_Hex(g);
            string blue = Dec01_to_Hex(b);
            return red+green+blue;
        }
        
        /// <summary>
        /// Get Hex Color code ex: FF00FF with alpha value
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns>6 letter color code.</returns>
        public static string GetStringFromColor(float r, float g, float b, float a) {
            string alpha = Dec01_to_Hex(a);
            return GetStringFromColor(r,g,b)+alpha;
        }
        
        /// <summary>
        /// Get Color from Hex string FF00FFAA  
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns>Color</returns>
        public static Color GetColorFromString(string color) {
            float red = Hex_to_Dec01(color.Substring(0,2));
            float green = Hex_to_Dec01(color.Substring(2,2));
            float blue = Hex_to_Dec01(color.Substring(4,2));
            float alpha = 1f;
            if (color.Length >= 8) {
                // Color string contains alpha
                alpha = Hex_to_Dec01(color.Substring(6,2));
            }
            return new Color(red, green, blue, alpha);
        }
        
        /// <summary>
        /// Checks if input parameter colors are similar. 
        /// </summary>
        /// <param name="colorA">First Color to compare.</param>
        /// <param name="colorB">Second Color to compare.</param>
        /// <param name="maxDiff">Difference value</param>
        /// <returns>Are they similar colors?</returns>
        public static bool IsColorSimilar(Color colorA, Color colorB, float maxDiff) {
            float rDiff = Mathf.Abs(colorA.r - colorB.r);
            float gDiff = Mathf.Abs(colorA.g - colorB.g);
            float bDiff = Mathf.Abs(colorA.b - colorB.b);
            float aDiff = Mathf.Abs(colorA.a - colorB.a);

            float totalDiff = rDiff + gDiff + bDiff + aDiff;
            return totalDiff < maxDiff;
        }

        /// <summary>
        /// Checks if input parameter colors are similar. 
        /// </summary>
        /// <param name="colorA">First Color to compare.</param>
        /// <param name="colorB">Second Color to compare.</param>
        /// <returns>Difference value between colors.</returns>
        public static float GetColorDifference(Color colorA, Color colorB) {
            float rDiff = Mathf.Abs(colorA.r - colorB.r);
            float gDiff = Mathf.Abs(colorA.g - colorB.g);
            float bDiff = Mathf.Abs(colorA.b - colorB.b);
            float aDiff = Mathf.Abs(colorA.a - colorB.a);

            float totalDiff = rDiff + gDiff + bDiff + aDiff;
            return totalDiff;
        }
        
        /// <summary>
        /// Shuffle array elements
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="iterations"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShuffleArray<T>(T[] arr, int iterations) {
            for (int i = 0; i < iterations; i++) {
                int rnd = UnityEngine.Random.Range(0, arr.Length);
                T tmp = arr[rnd];
                arr[rnd] = arr[0];
                arr[0] = tmp;
            }
        }
        
        /// <summary>
        /// Shuffle list elements
        /// </summary>
        /// <param name="list"></param>
        /// <param name="iterations"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShuffleList<T>(List<T> list, int iterations) {
            for (int i = 0; i < iterations; i++) {
                int rnd = UnityEngine.Random.Range(0, list.Count);
                T tmp = list[rnd];
                list[rnd] = list[0];
                list[0] = tmp;
            }
        }
        
        /// <summary>
        /// Appliying angle on to vector.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector3 ApplyRotationToVector(Vector3 vec, float angle) {
            return Quaternion.Euler(0, 0, angle) * vec;
        }
        
        /// <summary>
        /// Remove duplicates from array
        /// </summary>
        /// <param name="arr"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] RemoveDuplicates<T>(T[] arr) {
            List<T> list = new List<T>();
            foreach (T t in arr) {
                if (!list.Contains(t)) {
                    list.Add(t);
                }
            }
            return list.ToArray();
        }
        
        /// <summary>
        /// Remove duplicates from list
        /// </summary>
        /// <param name="arr"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RemoveDuplicates<T>(List<T> arr) {
            List<T> list = new List<T>();
            foreach (T t in arr) {
                if (!list.Contains(t)) {
                    list.Add(t);
                }
            }
            return list;
        }

    }
}
