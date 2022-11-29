using AppCitas.Service.DTOs;
using AppCitas.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace AppCitas.UnitTests.Tests;

public class MessagesControllerTests
{
    private string apiRoute = "api/messages";
    private readonly HttpClient _client;
    private HttpResponseMessage? httpResponse;
    private string requestUri = String.Empty;
    private string registeredObject = String.Empty;
    private HttpContent? httpContent;

    public MessagesControllerTests()
    {
        _client = TestHelper.Instance.Client;
    }

    // No se puede mandar mensaje a si misma
    [Theory]
    [InlineData("BadRequest", "lisa", "Pa$$w0rd", "lisa", "Hola bb")]
    public async Task SendMessage_ShouldBadRequest(string statusCode, string username, string password, string recipientUsername, string content)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var messageDto = new MessageDto
        {
            RecipientUserName = recipientUsername,
            Content = content
        };
        registeredObject = GetRegisterObject(messageDto);
        httpContent = GetHttpContent(registeredObject);
        requestUri = $"{apiRoute}";

        // Act
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    // No se encuentra el usuario al que se manda e mensaje
    [Theory]
    [InlineData("NotFound", "lisa", "Pa$$w0rd", "no", "Hola bb")]
    public async Task SendMessage_ShouldNotFound(string statusCode, string username, string password, string recipientUsername, string content)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var messageDto = new MessageDto
        {
            RecipientUserName = recipientUsername,
            Content = content
        };
        registeredObject = GetRegisterObject(messageDto);
        httpContent = GetHttpContent(registeredObject);
        requestUri = $"{apiRoute}";

        // Act
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    // Sí se manda el mensaje
    [Theory]
    [InlineData("OK", "todd", "Pa$$w0rd", "lisa", "Hola bb")]
    public async Task SendMessage_ShouldOK(string statusCode, string username, string password, string recipientUsername, string content)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var messageDto = new MessageDto
        {
            RecipientUserName = recipientUsername,
            Content = content
        };
        registeredObject = GetRegisterObject(messageDto);
        httpContent = GetHttpContent(registeredObject);
        requestUri = $"{apiRoute}";

        // Act
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    // Traer todos los mensajes
    [Theory]
    [InlineData("OK", "todd", "Pa$$w0rd")]
    public async Task GetMessagesForUser_ShouldOK(string statusCode, string username, string password)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        requestUri = $"{apiRoute}";

        // Act
        httpResponse = await _client.GetAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }


    //Traer toda la conversación entre dos usuarios
    [Theory]
    [InlineData("OK", "todd", "Pa$$w0rd", "lisa")]
    public async Task GetMessagesThread_ShouldOK(string statusCode, string username, string password, string recipient)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        requestUri = $"{apiRoute}/thread/" + recipient;

        // Act
        httpResponse = await _client.GetAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    // Borrar un mensaje
    [Theory]
    [InlineData("OK", "todd", "Pa$$w0rd", "lisa", "Hola bb")]
    public async Task DeleteMessage_ShouldOK(string statusCode, string username, string password, string recipientUsername, string content)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var messageDto = new MessageDto
        {
            RecipientUserName = recipientUsername,
            Content = content
        };
        registeredObject = GetRegisterObject(messageDto);
        httpContent = GetHttpContent(registeredObject);
        requestUri = $"{apiRoute}";
        var result = await _client.PostAsync(requestUri, httpContent);
        var messageJson = await result.Content.ReadAsStringAsync();
        var message = messageJson.Split(',');
        var id = message[0].Split("\"")[2].Split(":")[1];
        requestUri = $"{apiRoute}/" + id;

        // Act
        httpResponse = await _client.DeleteAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;
        user = await LoginHelper.LoginUser(recipientUsername, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        // Act
        httpResponse = await _client.DeleteAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }


    [Theory]
    [InlineData("Unauthorized", "bob", "Pa$$w0rd", "todd", "Hola bb", "lisa")]
    public async Task DeleteMessage_ShouldUnauthorized(string statusCode, string username, string password, string recipientUsername, string content, string senderUsername)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var messageDto = new MessageDto
        {
            RecipientUserName = recipientUsername,
            Content = content
        };
        registeredObject = GetRegisterObject(messageDto);
        httpContent = GetHttpContent(registeredObject);
        requestUri = $"{apiRoute}";
        var result = await _client.PostAsync(requestUri, httpContent);
        var messageJson = await result.Content.ReadAsStringAsync();
        _client.DefaultRequestHeaders.Authorization = null;
        var message = messageJson.Split(',');
        var id = message[0].Split("\"")[2].Split(":")[1];
        requestUri = $"{apiRoute}/" + id;

        user = await LoginHelper.LoginUser(senderUsername, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        // Act
        httpResponse = await _client.DeleteAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }


    #region Privated methods
    private static string GetRegisterObject(MessageDto message)
    {
        var entityObject = new JObject()
        {
            { nameof(message.RecipientUserName), message.RecipientUserName },
            { nameof(message.Content), message.Content }
        };
        return entityObject.ToString();
    }

    private StringContent GetHttpContent(string objectToEncode)
    {
        return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
    }

    #endregion
}
