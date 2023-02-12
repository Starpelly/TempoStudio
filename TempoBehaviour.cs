using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace TempoStudio
{
    public class TempoBehaviour : MonoBehaviour
    {
        public IEnumerator StartCoroutines(params IEnumerator[] enumerators)
        {
            bool[] completions = new bool[enumerators.Length];
            for (int i = 0; i < enumerators.Length; i++)
            {
                base.StartCoroutine(this.Wrapper(enumerators[i], completions, i));
            }
            for (; ; )
            {
                if (completions.All((bool x) => x))
                {
                    break;
                }
                yield return null;
            }
            yield break;
        }

        private IEnumerator Wrapper(IEnumerator enumerator, bool[] completions, int index)
        {
            try
            {
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    yield return obj;
                }
            }
            finally
            {
                completions[index] = true;
            }
            yield break;
        }

        public TempoBehaviour()
        {
        }
    }
}
