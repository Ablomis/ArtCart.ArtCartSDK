using UnityEngine;
using System;
using System.Net.Http;
using System.Text;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class ArtCart: MonoBehaviour
{
    public static ArtCart Instance;

    private string url_awardNFT = "https://artcart-serverless.netlify.app/.netlify/functions/awardNFT";
    private string url_awardSpecificNFT = "https://artcart-serverless.netlify.app/.netlify/functions/awardSpecificNFT";

    private void Awake()
    {
        // First time run
        if (Instance == null)
        {
            // Tell Unity not to destory the
            //  the GameObject this script component
            //  is attached to (thus keeping it alive
            //  as well).
            DontDestroyOnLoad(gameObject);

            // Save a reference to 'this'
            Instance = this;
        }
        else if (Instance != this)
        {
            // If Instance is ever not its first 'this',
            //  destroy it.
            Destroy(gameObject);
        }

    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
    }

    public void awardSpecificNFT(string email, string cid)
    {
        var payload = new Dictionary<string, string>();
        payload.Add("email", email.ToString());
        payload.Add("cid", cid.ToString());

        var json = JsonConvert.SerializeObject(payload);

        StartCoroutine(Post(url_awardSpecificNFT, json));
    }

    public void awardNFT(string email)
    {
        using var client = new HttpClient();
        var payload = new Dictionary<string, string>();
        payload.Add("email", email.ToString());

        var json = JsonConvert.SerializeObject(payload);

        StartCoroutine(Post(url_awardNFT, json));
    }
}

