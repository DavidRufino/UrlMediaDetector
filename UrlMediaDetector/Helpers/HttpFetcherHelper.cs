using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using UrlMediaDetector.Models.Contexts;

namespace UrlMediaDetector.Helpers;

public class HttpFetcherHelper
{
    // Singleton instance of HttpFetcherHelper with lazy initialization.
    private static readonly Lazy<HttpFetcherHelper> _instance = new(() => new HttpFetcherHelper());

    // Access the singleton instance.
    public static HttpFetcherHelper Instance => _instance.Value;

    // HttpClient instance for API requests, reusing the same HttpClient.
    private readonly HttpClient _httpClient;

    public string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.54 Safari/537.36";

    // Private constructor for singleton pattern.
    private HttpFetcherHelper()
    {
        // Initialize HttpClient for reusability.
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30), // Example configuration, can be adjusted.
        };

        _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
    }

    /// <summary>
    /// Sends a GET request to the specified endpoint and returns the response as a deserialized JSON object.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize the JSON response into.</typeparam>
    /// <param name="endpoint">The URL of the endpoint to send the request to.</param>
    /// <returns>The deserialized JSON response.</returns>
    public async Task<T?> GetJsonFromEndpoint<T>(string endpoint, JsonTypeInfo<T> typeInfo)
    {
        Debug.WriteLine($"{nameof(GetJsonFromEndpoint)} {endpoint}");

        try
        {
            var requestUri = new Uri(endpoint);
            var httpResponse = await _httpClient.GetAsync(requestUri);
            httpResponse.EnsureSuccessStatusCode();

            var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

            Debug.WriteLine($"{nameof(GetJsonFromEndpoint)} httpResponseBody {httpResponseBody}");

            if (string.IsNullOrWhiteSpace(httpResponseBody))
            {
                return default;
            }

            var contentType = httpResponse.Content.Headers.ContentType;
            var mediaType = contentType?.MediaType;

            if (!string.Equals(mediaType, "application/json", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"The response from {endpoint} is not in JSON format.");
            }

            return JsonSerializer.Deserialize<T>(httpResponseBody, typeInfo);
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Error fetching {endpoint}: {ex.Message}");
            throw new ArgumentException($"Error fetching {endpoint}: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Debug.WriteLine($"Error deserializing JSON from {endpoint}: {ex.Message}");
            throw new ArgumentException($"Error deserializing JSON from {endpoint}: {ex.Message}");
        }
    }

    /// <summary>
    /// Fetches a web page using HTTP GET method and returns its contents as a string.
    /// </summary>
    /// <param name="endpoint">The URL of the web page to fetch.</param>
    /// <returns>A string containing the contents of the web page, or null if there was an error.</returns>
    public async Task<string?> GetWebPageContents(string endpoint)
    {
        // Log the endpoint being fetched.
        Debug.WriteLine($"[HttpFetcherHelper] GetWebPageContents endpoint: {endpoint}"); 

        var requestUri = new Uri(endpoint); // Parse the endpoint into a Uri object.

        try
        {
            var httpResponse = await _httpClient.GetAsync(requestUri); // Send the GET request and wait for the response.
            httpResponse.EnsureSuccessStatusCode(); // Throw an exception if the response is not successful.

            var httpResponseBody = await httpResponse.Content.ReadAsStringAsync(); // Read the response content as a string.

            return httpResponseBody; // Return the response content.
        }
        catch (Exception ex) // Catch any HTTP request errors.
        {
            Debug.WriteLine($"Error fetching {endpoint}: {ex.Message}"); // Log the error message.
            return null; // Return null to indicate an error occurred.
        }

    }

    /// <summary>
    /// Method para fazer requesição (GET) e obter a url do video via match
    /// </summary>
    /// <param name="defaultUrl">URL da pagina para requisitar</param>
    /// <param name="matchPattern">Regex Pattern para encontrar a URL especifica no REQUEST GET</param>
    /// <returns></returns>
    public async Task<string?> GetUrlFromWebPageContents(string defaultUrl, string matchPattern)
    {
        Debug.WriteLine($"[HttpFetcherHelper] RequestPage start pageURL {defaultUrl}");

        var pageResponse = await GetWebPageContents(defaultUrl);
        if (pageResponse is null) return null;

        Debug.WriteLine("[HttpFetcherHelper] RequestPage pageResponse " + pageResponse);

        var regex = new Regex(matchPattern);
        var result = regex.Matches(pageResponse);

        string urlVideo = pageResponse;
        if (result.Count != 0) urlVideo = result[0].ToString();
        else
        {
            Debug.WriteLine("[HttpFetcherHelper] RequestPage Verificando se existe video de outro servico ");
            //  Verificar se possui um video do Youtube
            regex = new Regex(@"http(?:s?):\/\/(?:www\.)?youtu(?:be\.com\/watch\?v=|\.be\/)([\w\-]*)(&(amp;)[\w\=]*)?");
            result = regex.Matches(pageResponse);

            if (result.Count != 0)
            {
                urlVideo = result[0].ToString();
            }
        }

        Debug.WriteLine("[HttpFetcherHelper] RequestPage urlVideo " + urlVideo);
        return urlVideo;
    }

    /// <summary>
    /// Method para verificar se o HTML esta codificado e decodifica ela
    /// </summary>
    /// <param name="html"></param>
    /// <returns>Retorna o HTML decodificado</returns>
    public string CheckThenDecodeHTML(string html)
    {
        //  Guardar a url recebida em uma nova variavel
        string htmlCompare = html;

        //  Replace o character plus + para o formato codificado, pois o windows transforma o plus em espaco em branco
        html = html.Replace("+", "%2B");
        string htmlDecoded = WebUtility.HtmlDecode(html);

        Debug.WriteLine("[HttpFetcherHelper] CheckThenDecodeHTML urlCompare " + htmlCompare);
        Debug.WriteLine("[HttpFetcherHelper] CheckThenDecodeHTML urlDecoded " + htmlDecoded);

        //  Se a URL codificada for igual
        if (htmlDecoded == htmlCompare)
        {
            Debug.WriteLine("[HttpFetcherHelper] CheckThenDecodeHTML it is already decoded");
            //  A URL esta decodificada
            return htmlCompare;
        }
        else
        {
            Debug.WriteLine("[HttpFetcherHelper] CheckThenDecodeHTML decoding...");
            //  A url ja esta codificada, precisa decodificar
            return WebUtility.HtmlDecode(htmlCompare);
        }
    }
}
