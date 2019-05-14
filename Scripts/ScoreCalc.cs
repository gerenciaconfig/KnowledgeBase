using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class ScoreCalc
    {
        private int baseBubbleScore = 10;
        private int resultMoves = 0;
        private int bubblesCount = 0;

        public int GetScore(out int bubbleScore)
        {
            int tm = (resultMoves > 0) ? resultMoves - 1 : 0;
            int res = bubblesCount * (baseBubbleScore + 5 * tm + 15 + 5 * tm);
            bubbleScore = baseBubbleScore;

            return res;
        }
    }
}