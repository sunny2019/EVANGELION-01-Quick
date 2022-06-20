using System;
using System.Collections;
using System.Runtime.InteropServices;
using Cinemachine;
using UnityEngine.SceneManagement;

namespace EVANGELION
{
    using System.Collections.Generic;
    using DG.Tweening;
    using HighlightPlus;
    using UnityEngine;

    public static class Extension
    {
        static Extension()
        {
            SceneManager.sceneLoaded += ((arg0, mode) =>
            {
                #region CinemachineVirtualCamera

                cv_Index = 11;

                #endregion
            });
        }

        # region List

        /// <summary>
        /// 从列表中获取一定数量的不重复，注意不要超过列表上限
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetRandomList<T>(List<T> paramList, int count)
        {
            if (paramList.Count < count)
            {
                return paramList;
            }

            System.Random random = new System.Random();
            List<int> tempList = new List<int>();
            List<T> newList = new List<T>();
            int temp = 0;
            for (int i = 0; i < count; i++)
            {
                temp = random.Next(paramList.Count); //将产生的随机数作为被抽list的索引
                if (!tempList.Contains(temp))
                {
                    tempList.Add(temp);
                    newList.Add(paramList[temp]);
                }
                else
                {
                    i--;
                }
            }

            return newList;
        }

        #endregion


        #region HighlightEffect

        public static HighlightEffect HighlightEffectOverlayColorHDR(this HighlightEffect highlightEffect,
            float multiple)
        {
            highlightEffect.overlayColor = new Color(highlightEffect.overlayColor.r * multiple,
                highlightEffect.overlayColor.g * multiple, highlightEffect.overlayColor.b * multiple);
            return highlightEffect;
        }

        #endregion


        #region Color

        /// <summary>
        /// 随机获取一个颜色
        /// </summary>
        /// <returns></returns>
        public static Color RandomColor()
        {
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);
            Color color = new Color(r, g, b);
            return color;
        }

        #endregion

        #region Time

        public static string GetTime(float time)
        {
            float h = Mathf.FloorToInt(time / 3600f);
            float m = Mathf.FloorToInt(time / 60f - h * 60f);
            float s = Mathf.FloorToInt(time - m * 60f - h * 3600f);
            return h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");
        }

        public static string GetTimeToDay(float time)
        {
            float d = Mathf.FloorToInt(time / 86400f);
            float h = Mathf.FloorToInt(time / 3600f - d * 86400f);
            float m = Mathf.FloorToInt(time / 60f - h * 60f - d * 86400f);
            float s = Mathf.FloorToInt(time - m * 60f - h * 3600f - d * 86400f);
            return d.ToString("00") + ":" + h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");
        }

        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.bool bflag = true</param>  
        /// <returns></returns>  
        public static long GetTimeStamp(this DateTime dateTime, bool bflag)
        {
            TimeSpan ts =dateTime - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long temp;
            if (bflag)
                temp = Convert.ToInt64(ts.TotalSeconds);
            else
                temp = Convert.ToInt64(ts.TotalMilliseconds);

            return temp;
        }
        
        #endregion

        #region Transform

        /// <summary>
        /// 对象池加载重置Z轴
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static Transform UIPoolSpawnedResetY(this Transform trans)
        {
            Vector3 v3 = trans.GetComponent<RectTransform>().localPosition;
            v3.z = 0;
            trans.GetComponent<RectTransform>().localPosition = v3;
            return trans;
        }


        public static Transform UIPoolSpawnedSetFirst(this Transform trans)
        {
            trans.SetAsFirstSibling();
            return trans;
        }

        public static Transform UIPoolSpawnedSetLast(this Transform trans)
        {
            trans.SetAsLastSibling();
            return trans;
        }

        /// <summary>
        /// 对象池加载重置XYZ轴
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static Transform UIPoolSpawnedResetXYZ(this Transform trans, Vector3 localPosition)
        {
            trans.GetComponent<RectTransform>().localPosition = localPosition;
            trans.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            return trans;
        }

        public static Transform UIPoolSpawnedResetSizeDelta(this Transform trans, Vector2 sizeDelta)
        {
            trans.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            return trans;
        }

        #endregion

        #region Material

        private static Material TransparentMat;

        public static Material[] TransparentToOpaque(this Material[] mat, bool needProperties = false)
        {
            if (TransparentMat == null)
            {
                TransparentMat = Resources.Load<Material>("TransparentMat");
            }

            Material[] newMats = new Material[mat.Length];
            if (needProperties)
            {
                for (int i = 0; i < mat.Length; i++)
                {
                    newMats[i] = SetMaterialProperties(mat[i], new Material(TransparentMat));
                }
            }
            else
            {
                for (int i = 0; i < mat.Length; i++)
                {
                    newMats[i] = new Material(TransparentMat);
                }
            }

            return newMats;
        }

        public static Material TransparentToOpaque(this Material mat, bool needProperties = false)
        {
            if (TransparentMat == null)
            {
                TransparentMat = Resources.Load<Material>("TransparentMat");
            }

            Material tempMat = new Material(TransparentMat);
            if (needProperties)
            {
                tempMat = SetMaterialProperties(mat, tempMat);
            }

            return tempMat;
        }

        private static Material SetMaterialProperties(Material oldMat, Material newMat)
        {
            newMat.SetFloat("_WorkflowMode", oldMat.GetFloat("_WorkflowMode"));
            newMat.SetColor("_BaseColor", oldMat.GetColor("_BaseColor"));
            newMat.SetTexture("_BaseMap", oldMat.GetTexture("_BaseMap"));
            newMat.SetFloat("_Cutoff", oldMat.GetFloat("_Cutoff"));
            newMat.SetFloat("_Smoothness", oldMat.GetFloat("_Smoothness"));
            newMat.SetFloat("_GlossMapScale", oldMat.GetFloat("_GlossMapScale"));
            newMat.SetFloat("_SmoothnessTextureChannel", oldMat.GetFloat("_SmoothnessTextureChannel"));
            newMat.SetFloat("_Metallic", oldMat.GetFloat("_Metallic"));
            newMat.SetTexture("_MetallicGlossMap", oldMat.GetTexture("_MetallicGlossMap"));
            newMat.SetColor("_SpecColor", oldMat.GetColor("_SpecColor"));
            newMat.SetTexture("_SpecGlossMap", oldMat.GetTexture("_SpecGlossMap"));
            newMat.SetFloat("_SpecularHighlights", oldMat.GetFloat("_SpecularHighlights"));
            newMat.SetFloat("_EnvironmentReflections", oldMat.GetFloat("_EnvironmentReflections"));

            newMat.SetFloat("_BumpScale", oldMat.GetFloat("_BumpScale"));
            newMat.SetTexture("_BumpMap", oldMat.GetTexture("_BumpMap"));

            newMat.SetFloat("_OcclusionStrength", oldMat.GetFloat("_OcclusionStrength"));
            newMat.SetTexture("_OcclusionMap", oldMat.GetTexture("_OcclusionMap"));

            newMat.SetColor("_EmissionColor", oldMat.GetColor("_EmissionColor"));
            newMat.SetTexture("_EmissionMap", oldMat.GetTexture("_EmissionMap"));

            //HideInInspector
            // newMat.SetFloat("_Surface",oldMat.GetFloat("_Surface"));
            // newMat.SetFloat("_Blend",oldMat.GetFloat("_Blend"));
            // newMat.SetFloat("_SrcBlend",oldMat.GetFloat("_SrcBlend"));
            // newMat.SetFloat("_DstBlend",oldMat.GetFloat("_DstBlend"));
            // newMat.SetFloat("_ZWrite",oldMat.GetFloat("_ZWrite"));
            // newMat.SetFloat("_Cull",oldMat.GetFloat("_Cull"));

            newMat.SetFloat("_ReceiveShadows", oldMat.GetFloat("_ReceiveShadows"));


            newMat.SetFloat("_ReceiveShadows", oldMat.GetFloat("_ReceiveShadows"));
            newMat.SetFloat("_QueueOffset", oldMat.GetFloat("_QueueOffset"));
            newMat.SetTexture("_MainTex", oldMat.GetTexture("_MainTex"));
            newMat.SetColor("_Color", oldMat.GetColor("_Color"));

            newMat.SetFloat("_GlossMapScale", oldMat.GetFloat("_GlossMapScale"));
            newMat.SetFloat("_Glossiness", oldMat.GetFloat("_Glossiness"));
            newMat.SetFloat("_GlossyReflections", oldMat.GetFloat("_GlossyReflections"));
            return newMat;
        }

        #endregion


        #region MeshRenderer

        private static Dictionary<MeshRenderer, Material[]>
            OriginMaterials = new Dictionary<MeshRenderer, Material[]>();

        private static Material[] SaveMaterial(this MeshRenderer meshRenderer)
        {
            if (OriginMaterials.ContainsKey(meshRenderer))
                return OriginMaterials[meshRenderer];
            else
                return OriginMaterials[meshRenderer] = meshRenderer.sharedMaterials;
        }

        public static void SetAlpha(this MeshRenderer meshRenderer, float alpha = 0.5f, float duration = 0.5f,
            bool needProperties = false)
        {
            Material[] tempMats =
                meshRenderer.materials = SaveMaterial(meshRenderer).TransparentToOpaque(needProperties);
            for (int i = 0; i < tempMats.Length; i++)
            {
                Color cr = tempMats[i].color;
                tempMats[i].DOColor(new Color(cr.r, cr.g, cr.b, alpha), duration);
            }
        }

        public static void ResetMaterials(this MeshRenderer meshRenderer)
        {
            meshRenderer.materials = OriginMaterials[meshRenderer];
        }

        public static void SetAlphas(this GameObject go, float alpha = 0.5f, float duration = 0.5f,
            bool needProperties = false)
        {
            MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].SetAlpha(alpha, duration, needProperties);
            }
        }

        public static void ResetMaterials(this GameObject go)
        {
            MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].ResetMaterials();
            }
        }

        #endregion


        #region Random

        // Number随机数个数
// minNum随机数下限
// maxNum随机数上限
        public static int[] GetRandomArray(int Number, int minNum, int maxNum)
        {
            int j;
            int[] b = new int[Number];
            System.Random r = new System.Random();
            for (j = 0; j < Number; j++)
            {
                int i = r.Next(minNum, maxNum + 1);
                int num = 0;
                for (int k = 0; k < j; k++)
                {
                    if (b[k] == i)
                    {
                        num = num + 1;
                    }
                }

                if (num == 0)
                {
                    b[j] = i;
                }
                else
                {
                    j = j - 1;
                }
            }

            return b;
        }

        #endregion


        #region Coroutine

        public static IEnumerator WaitSecond(float second, Action callBack)
        {
            yield return new WaitForSeconds(second);
            callBack?.Invoke();
        }

        #endregion


        #region CinemachineVirtualCamera

        private static int cv_Index = 11;

        private static int CV_Index
        {
            get => ++cv_Index;
        }

        public static void SetActiveCamera(this CinemachineVirtualCamera camera)
        {
            camera.Priority = CV_Index;
        }

        #endregion



        #region OpenUrl

        [DllImport("__Internal")]
        private static extern void OpenPage(string str);


        public static void OpenURL(string url)
        {
#if UNITY_EDITOR
            Application.OpenURL(url);
#elif UNITY_WEBGL
            OpenPage(url);
#else
            Application.OpenURL(url);
#endif
        }

        #endregion
    }
}