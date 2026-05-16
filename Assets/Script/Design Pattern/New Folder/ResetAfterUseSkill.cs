using System;
using System.Collections;
using UnityEngine;

public class ResetAfterUseSkill : MonoBehaviour
{
   
        public static ResetAfterUseSkill instance;

        void Awake()
        {
            instance = this;
        }
        public IEnumerator Coroutine(float time, Action action)
        {
            yield return new WaitForSecondsRealtime(time);
            action?.Invoke();
        }

        public void StartFeature(float time, Action action)
        {
            StartCoroutine(Coroutine(time, action));

        }
    }


