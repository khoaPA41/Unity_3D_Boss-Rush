using System;
using System.Collections;
using UnityEngine;

namespace Script.Design_Pattern.New_Folder
{
    public class ResetAfterUseSkill : MonoBehaviour
    {
        public static ResetAfterUseSkill Instance;
        private static readonly int Metallic = Shader.PropertyToID("_Metallic");

        private void Awake()
        {
            Instance = this;
        }

        private static IEnumerator Coroutine(float time)
        {
            yield return new WaitForSecondsRealtime(time);

            // action?.Invoke();
        }

        // public void StartFeature(float time, Action action)
        // {
        //     StartCoroutine(Coroutine(time, action));
        // }

        public void ChangeMaterialsInTime(ref Material[] materials, float time)
        {
            var tempMaterials = materials;
            tempMaterials[0].SetFloat(Metallic, 1f);
            tempMaterials[1].SetFloat(Metallic, 1f);
            materials = tempMaterials;
            StartCoroutine(Coroutine(time));
            tempMaterials[0].SetFloat(Metallic, 0f);
            tempMaterials[1].SetFloat(Metallic, 0f);
        }
    }
}


