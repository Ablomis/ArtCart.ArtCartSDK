
using System.Net;
using System;
using System.IO;
using Assets;
using System.Text.Json;
public class artcart: MonoBehaviour
{
    public string clientID;

    private string accessToken;

    // Start is called before the first frame update
    void Start()
    {
        this.connect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This method returns
    public string GetSteamAuthTicket() {
        byte[] ticketBlob = new byte[1024];
        uint ticketSize;

        // Retrieve ticket; hTicket should be a field in the class so you can use it to cancel the ticket later
        // When you pass an object, the object can be modified by the callee. This function modifies the byte array you've passed to it.
        HAuthTicket hTicket = SteamUser.GetAuthSessionTicket(ticketBlob, ticketBlob.Length, out ticketSize);

        // Resize the buffer to actual length
        Array.Resize(ref ticketBlob, (int)ticketSize);

        // Convert bytes to string
        StringBuilder sb = new StringBuilder();
        foreach (byte b in ticketBlob) {
            sb.AppendFormat("{0:x2}", b);
        }
        return sb.ToString();
    }

    public async void connect(string clientID){
        using var client = new HttpClient();

        string json = new JavaScriptSerializer().Serialize(new
                {
                    //steamTicket = GetSteamAuthTicket(),
                    steamTicket = "Test ticket",
                    clientID = this.clientID
                });

        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var url = "https://artcart-server.herokuapp.com/api/steam/auth";

        var response = await client.PostAsync(url, data);
        string result = response.Content.ReadAsStringAsync().Result;
        Console.WriteLine(result);
    }

        // Utility callbacks to log the result
    private void OnComplete(LoginResult obj) {
        Debug.Log("Success!");
    }

    private void OnFailed(PlayFabError error) {
        Debug.Log("Failed: " + error.GenerateErrorReport());
    }

}

public class nft
{
    private string imgURL;
    private string metaURL;
    private string nftAddress;
    private int nftID;
}

