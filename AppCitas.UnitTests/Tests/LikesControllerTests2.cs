//using AppCitas.Service.DTOs;
//using AppCitas.UnitTests.Helpers;
//using Newtonsoft.Json.Linq;
//using System.Net;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;

//namespace AppCitas.UnitTests.Tests;

//public class LikesControllerTests2
//{
//    private string apiRoute = "api/likes";
//    private readonly HttpClient _client;
//    private HttpResponseMessage? httpResponse;
//    private string requestUri = String.Empty;
//    private string requestUriLogin = String.Empty;
//    private string registerObject = String.Empty;
//    private string loginObjetct = String.Empty;
//    private HttpContent? httpContent;

//    public LikesControllerTests2()
//    {
//        _client = TestHelper.Instance.Client;
//    }

//    [Theory]
//    [InlineData("BadRequest", "lisa", "Pa$$w0rd")]
//    public async Task Like_ShouldOk(string statusCode, string username, string password)
//    {
//        // Arrange
//        //Login
//        requestUriLogin = $"{apiRoute}/login";
//        var loginDto = new LoginDto
//        {
//            Username = username,
//            Password = password
//        };
//        loginObjetct = GetRegisterObject(loginDto);
//        httpContent = GetHttpContent(loginObjetct);
//        var result = await _client.PostAsync(requestUriLogin, httpContent);
//        var userJson = await result.Content.ReadAsStringAsync();
//        var user = JsonSerializer.Deserialize<UserDto>(userJson, new JsonSerializerOptions
//        {
//            PropertyNameCaseInsensitive = true
//        });
//        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user?.Token);
//        //Login

//        requestUri = $"{apiRoute}/todd";

//        // Act
//        httpResponse = await _client.PostAsync(requestUri, httpContent);

//        // Assert
//        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
//        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
//    }

//    //[Theory]
//    //[InlineData("BadRequest")]
//    //public async Task Like_ShouldNotFound(string statusCode)
//    //{
//    //    // Arrange
//    //    requestUri = $"{apiRoute}/notRegistered";


//    //    // Act
//    //    httpResponse = await _client.PostAsync(requestUri, httpContent);

//    //    // Assert
//    //    Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
//    //    Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
//    //}

//    //[Theory]
//    //[InlineData("Unauthorized", "lisa", "password")]
//    //public async Task ike_ShouldUnauthorized(string statusCode, string username, string password)
//    //{
//    //    // Arrange
//    //    requestUri = $"{apiRoute}/login";
//    //    var loginDto = new LoginDto
//    //    {
//    //        Username = username,
//    //        Password = password
//    //    };

//    //    // Act
//    //    httpResponse = await _client.PostAsync(requestUri, httpContent);

//    //    // Assert
//    //    Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
//    //    Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
//    //}

//    //[Theory]
//    //[InlineData("OK", "lisa", "Pa$$w0rd")]
//    //public async Task Login_ShouldOK(string statusCode, string username, string password)
//    //{
//    //    // Arrange
//    //    requestUri = $"{apiRoute}/login";
//    //    var loginDto = new LoginDto
//    //    {
//    //        Username = username,
//    //        Password = password
//    //    };
//    //    loginObjetct = GetRegisterObject(loginDto);
//    //    httpContent = GetHttpContent(loginObjetct);

//    //    // Act
//    //    httpResponse = await _client.PostAsync(requestUri, httpContent);

//    //    // Assert
//    //    Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
//    //    Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
//    //}

//    private static string GetRegisterObject(RegisterDto registerDto)
//    {
//        var entityObject = new JObject()
//        {
//            { nameof(registerDto.Username), registerDto.Username },
//            { nameof(registerDto.KnownAs), registerDto.KnownAs },
//            { nameof(registerDto.Gender), registerDto.Gender },
//            { nameof(registerDto.DateOfBirth), registerDto.DateOfBirth },
//            { nameof(registerDto.City), registerDto.City },
//            { nameof(registerDto.Country), registerDto.Country },
//            { nameof(registerDto.Password), registerDto.Password }
//        };

//        return entityObject.ToString();
//    }

//    private static string GetRegisterObject(LoginDto loginDto)
//    {
//        var entityObject = new JObject()
//        {
//            { nameof(loginDto.Username), loginDto.Username },
//            { nameof(loginDto.Password), loginDto.Password }
//        };
//        return entityObject.ToString();
//    }

//    private StringContent GetHttpContent(string objectToEncode)
//    {
//        return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
//    }
//}
