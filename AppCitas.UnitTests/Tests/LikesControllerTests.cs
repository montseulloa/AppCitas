using AppCitas.Service.DTOs;
using AppCitas.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AppCitas.UnitTests.Tests;

public class LikesControllerTests
{
    private string apiRoute = "api/likes";
    private readonly HttpClient _client;
    private HttpResponseMessage? httpResponse;
    private string requestUri = String.Empty;
    private string registerObject = String.Empty;
    private string loginObjetct = String.Empty;
    private HttpContent? httpContent;

    public LikesControllerTests()
    {
        _client = TestHelper.Instance.Client;
    }

    // No encuentra el usuario que será likeado
    [Theory]
    [InlineData("NotFound", "lisa", "Pa$$word")]
    public async Task Like_ShouldNotFound(string statusCode, string username, string password)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


        requestUri = $"{apiRoute}/montse";

        // Act
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }


    //No se puede dar like a si mismo
    [Theory]
    [InlineData("BadRequest", "lisa", "Pa$$word")]
    public async Task Like_ShouldBadRequest(string statusCode, string username, string password)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


        requestUri = $"{apiRoute}/lisa";

        // Act
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    //Dar like a alguien
    [Theory]
    [InlineData("OK", "todd", "Pa$$word")]
    public async Task Like_ShouldOK(string statusCode, string username, string password)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


        requestUri = $"{apiRoute}/lisa";

        // Act
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    //No se puede dar mas de un like a la misma persona
    [Theory]
    [InlineData("BadRequest", "lisa", "Pa$$word")]
    public async Task LikeRepeated_ShouldBadRequest(string statusCode, string username, string password)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


        requestUri = $"{apiRoute}/todd";

        // Act
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        httpResponse = await _client.PostAsync(requestUri, httpContent);

        _client.DefaultRequestHeaders.Authorization = null;

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }


    [Theory]
    [InlineData("OK", "todd", "Pa$$word")]
    public async Task GetUserLikes_ShouldOK(string statusCode, string username, string password)
    {
        // Arrange
        var user = await LoginHelper.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


        requestUri = $"{apiRoute}" + "?predicate=LIKEd";

        // Act
        httpResponse = await _client.GetAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }
    #region Privated methods
    private static string GetRegisterObject(RegisterDto registerDto)
    {
        var entityObject = new JObject()
        {
            { nameof(registerDto.Username), registerDto.Username },
            { nameof(registerDto.KnownAs), registerDto.KnownAs },
            { nameof(registerDto.Gender), registerDto.Gender },
            { nameof(registerDto.DateOfBirth), registerDto.DateOfBirth },
            { nameof(registerDto.City), registerDto.City },
            { nameof(registerDto.Country), registerDto.Country },
            { nameof(registerDto.Password), registerDto.Password }
        };

        return entityObject.ToString();
    }

    private static string GetRegisterObject(LoginDto loginDto)
    {
        var entityObject = new JObject()
        {
            { nameof(loginDto.Username), loginDto.Username },
            { nameof(loginDto.Password), loginDto.Password }
        };
        return entityObject.ToString();
    }

    private StringContent GetHttpContent(string objectToEncode)
    {
        return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
    }

    #endregion
}
