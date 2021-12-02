// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Net.Http.Json;

// Swagger
// https://involved-htf-challenge.azurewebsites.net/swagger/index.html

// De httpclient die we gebruiken om http calls te maken
var client = new HttpClient();
// De base url die voor alle calls hetzelfde is
client.BaseAddress = new Uri("http://involved-htf-challenge.azurewebsites.net");

// De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + password
var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiNDUiLCJuYmYiOjE2Mzg0MzYxNTUsImV4cCI6MTYzODUyMjU1NSwiaWF0IjoxNjM4NDM2MTU1fQ.TX0DcMp2vMIgAlYINUQZgN0mHHZl3Yza1R0NAw9mJ7NwHDWEgHYyo27Dx_RziCF-X61U2rwss1XlSsO-bC2RZA";
// We stellen de token in zodat die wordt meegestuurd bij alle calls, anders krijgen we een 401 Unauthorized response op onze calls
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

/*
// De url om de challenge te starten
var startUrl = "api/path/1/easy/Start";
// We voeren de call uit en wachten op de response
// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/await
// De start endpoint moet je 1 keer oproepen voordat je aan een challenge kan beginnen
// Krijg je een 403 Forbidden response op je Sample of Puzzle calls? Dat betekent dat de challenge niet gestart is
var startResponse = await client.GetAsync(startUrl);
Console.WriteLine(startResponse);
*/

// De url om de sample challenge data op te halen
var sampleUrl = "api/path/1/easy/Sample";
// We doen de GET request en wachten op de het antwoord
// De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
var sampleGetResponse = await client.GetFromJsonAsync<List<int>>(sampleUrl);

// Je zoekt het antwoord
var sampleAnswer = GetAnswer(sampleGetResponse);

// We sturen het antwoord met een POST request
// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
// De response die we krijgen zal ons zeggen of ons antwoord juist was
var samplePostResponse = await client.PostAsJsonAsync<int>(sampleUrl, sampleAnswer);
// Om te zien of ons antwoord juist was moeten we de response uitlezen
// Een 200 status code betekent dus niet dat je antwoord juist was!
var samplePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();

// De url om de puzzle challenge data op te halen
var puzzleUrl = "api/path/1/easy/Puzzle";
// We doen de GET request en wachten op de het antwoord
// De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
var puzzleGetResponse = await client.GetFromJsonAsync<List<int>>(puzzleUrl);

// Je zoekt het antwoord
var puzzleAnswer = GetAnswer(puzzleGetResponse);

// We sturen het antwoord met een POST request
// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
// De response die we krijgen zal ons zeggen of ons antwoord juist was
var puzzlePostResponse = await client.PostAsJsonAsync<int>(sampleUrl, puzzleAnswer);
// Om te zien of ons antwoord juist was moeten we de response uitlezen
// Een 200 status code betekent dus niet dat je antwoord juist was!
var puzzlePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();
Console.WriteLine(puzzlePostResponseValue);

int GetAnswer(List<int> getallen)
{
    int som = getallen.Sum();
    int res = 0;

    while(res > 10 || res == 0)
    {
        res = 0;
        while(som != 0)
        {
            res += (som % 10);
            som /= 10;
        }
        som = res;
    }
    return res;
}

