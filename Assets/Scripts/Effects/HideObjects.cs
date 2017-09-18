using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeoutLOSInfo
{
    public Renderer m_renderer;
    public Material[] m_originalMaterials;
    public Material[] m_alphaMaterials;
    public bool m_needFadeOut = true;
}

public class HideObjects : MonoBehaviour {

    private bool m_hideObject = false;
    private float m_fadeSpeed = 1.0f;
    private float m_occlusionRadius = 0.3f;
    private float m_fadedOutAlpha = 0.3f;


    private List<FadeoutLOSInfo> m_fadedOutObjects = new List<FadeoutLOSInfo>();

    // Use this for initialization
    void Start () {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in renderers)
        {
            FadeoutLOSInfo info = new FadeoutLOSInfo();
            info.m_renderer = r;
            info.m_originalMaterials = r.sharedMaterials;
            info.m_alphaMaterials = new Material[info.m_originalMaterials.Length];
            for (int i = 0; i<info.m_originalMaterials.Length; i++)
            {
                Material newMaterial = new Material(Shader.Find("Alpha/Diffuse"));
                newMaterial.mainTexture = info.m_originalMaterials[i].mainTexture;
                Color c = info.m_originalMaterials[i].color;
                c.a = 0.2f;
                newMaterial.color = c;
                info.m_alphaMaterials[i] = newMaterial;
            }
            m_fadedOutObjects.Add(info);
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Now go over all renderers and do the actual fading!
        float fadeDelta = m_fadeSpeed * Time.deltaTime;
        if (m_hideObject)
        {
            foreach (FadeoutLOSInfo info in m_fadedOutObjects)
            {
                if (info.m_renderer.sharedMaterials != info.m_alphaMaterials)
                {
                    info.m_renderer.sharedMaterials = info.m_alphaMaterials;
                }
            }
        }
        else
        {
            foreach(FadeoutLOSInfo info in m_fadedOutObjects)
            {
                if(info.m_renderer.sharedMaterials != info.m_originalMaterials)
                {
                    info.m_renderer.sharedMaterials = info.m_originalMaterials;
                }
            }
        }
        m_hideObject = false;
        //for (i = 0; i < fadedOutObjects.Count; i++)
        //{
        //    var fade = fadedOutObjects[i];
        //    // Fade out up to minimum alpha value
        //    if (fade.needFadeOut)
        //    {
        //        for (var alphaMaterial : Material in fade.alphaMaterials)
        //        {
        //            var alpha = alphaMaterial.color.a;
        //            alpha -= fadeDelta;
        //            alpha = Mathf.Max(alpha, fadedOutAlpha);
        //            alphaMaterial.color.a = alpha;
        //        }
        //    }
        //    // Fade back in
        //    else
        //    {
        //        var totallyFadedIn = 0;
        //        for (var alphaMaterial : Material in fade.alphaMaterials)
        //        {
        //            alpha = alphaMaterial.color.a;
        //            alpha += fadeDelta;
        //            alpha = Mathf.Min(alpha, 1.0);
        //            alphaMaterial.color.a = alpha;
        //            if (alpha >= 0.99)
        //                totallyFadedIn++;
        //        }

        //        // All alpha materials are faded back to 100%
        //        // Thus we can switch back to the original materials
        //        if (totallyFadedIn == fade.alphaMaterials.length)
        //        {
        //            if (fade.renderer)
        //                fade.renderer.sharedMaterials = fade.originalMaterials;
                   
        //        for (var newMaterial in fade.alphaMaterials)
        //                Destroy(newMaterial);

        //            fadedOutObjects.RemoveAt(i);
        //            i--;
        //        }
        //    }
        //}
    }
    
    public bool HideObject
    {
        set
        {
            m_hideObject = value;
        }
        get
        {
            return m_hideObject;
        }
    }
}
