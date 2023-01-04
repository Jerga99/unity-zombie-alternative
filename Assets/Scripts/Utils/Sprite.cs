using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace Eincode.ZombieSurvival.Utils
{
    public class EincodeUtils
    {
        public static List<T> CloneList<T>(List<T> oldList)
        {
            BinaryFormatter formatter = new();
            MemoryStream stream = new();
            formatter.Serialize(stream, oldList);
            stream.Position = 0;
            return (List<T>)formatter.Deserialize(stream);
        }

        public static void Flip(SpriteRenderer sprite, Action callback)
        {
            sprite.flipX = !sprite.flipX;
            callback();
        }

        public static Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
        {
            Vector2 _origin = origin;

            if (origin == Vector2.zero)
            {
                _origin = Vector2.one;
            }

            var randomDirection = (UnityRandom.insideUnitCircle * _origin).normalized;
            var randomDistance = UnityRandom.Range(minRadius, maxRadius);
            var point = _origin + randomDirection * randomDistance;

            return point;
        }

        public static Quaternion GetRandomRotation()
        {
            var direction = UnityRandom.insideUnitCircle.normalized;

            // revert directions because object is other wauy around
            float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static T[] Shuffle<T>(T[] items)
        {
            System.Random rand = new();
            for (int i = 0; i < items.Length - 1; i++)
            {
                int j = rand.Next(i, items.Length);
                (items[j], items[i]) = (items[i], items[j]);
            }

            return items;
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}

