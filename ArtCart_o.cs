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

    public IEnumerator awardSpecificNFT(string email, string cid)
    {
        var payload = new Dictionary<string, string>();
        payload.Add("email", email.ToString());
        payload.Add("cid", cid.ToString());

        var json = JsonConvert.SerializeObject(payload);

        var request = new UnityWebRequest(url_awardSpecificNFT, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
    }

    public async void awardNFT(string email)
    {
        using var client = new HttpClient();
        var payload = new Dictionary<string, string>();
        payload.Add("email", email.ToString());

        var json = JsonConvert.SerializeObject(payload);

        var data = new StringContent(json, Encoding.UTF8, "application/json"); 

        var response = await client.PostAsync(url_awardNFT, data);
        string result = response.Content.ReadAsStringAsync().Result;
        Debug.Log(result);
    }
}

