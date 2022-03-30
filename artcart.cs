using UnityEngine;
using System;
using System.Net.Http;
using System.Text;
using System.Dynamic;
using System.Collections.Generic;
using Newtonsoft.Json;

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

    public async void awardSpecificNFT(string email, string cid)
    {
        using var client = new HttpClient();
        var payload = new Dictionary<string, string>();
        payload.Add("email", email.ToString());
        payload.Add("cid", cid.ToString());

        var json = JsonConvert.SerializeObject(payload);

        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url_awardSpecificNFT, data);
        string result = response.Content.ReadAsStringAsync().Result;
        Debug.Log(result);
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

