using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class RunLine : ICloneable
    {
        public float Offset { get; }

        private readonly float[] _positions;
        private const int count = 3;
        private int _index;

        public RunLine(float distance, float mediumLinePosition, int initialIndex)
        {
            _positions = new float[count]
            {
                mediumLinePosition-distance,
                mediumLinePosition,
                mediumLinePosition+distance
            };
            _index = initialIndex;
            Offset = distance;
        }

        private RunLine(float[] positions, int index, float offset)
        {
            _positions = positions;
            _index = index;
            Offset = offset;
        }

        public float? GetLeft()
        {
            int x = _index - 1;
            return x >= 0 ? _positions[x] : null;
        }

        public float? GetRight()
        {
            int x = _index + 1;
            return x < _positions.Length ? _positions[x] : null;
        }

        public float GetCurrent() => _positions[_index];

        public void GoLeft()
        {
            int x = _index - 1;
            if (x >= 0)
                _index--;
            else
                throw new ArgumentOutOfRangeException(x.ToString());
        }

        public void GoRight()
        {
            int x = _index - 1;
            if (x < _positions.Length)
                _index++;
            else
                throw new ArgumentOutOfRangeException(x.ToString());
        }

        public void GoFirst()
        {
            _index = 0;
        }

        public object Clone()
        {
            return new RunLine(_positions, _index, Offset);
        }
    }
}
