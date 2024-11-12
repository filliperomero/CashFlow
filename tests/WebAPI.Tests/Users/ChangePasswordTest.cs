﻿using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebAPI.Test.InlineData;

namespace WebAPI.Test.Users;

public class ChangePasswordTest : CashFlowClassFixture
{
    private const string USER_URI = "api/User/change-password";
    
    private readonly string _token;
    private readonly string _password;
    private readonly string _email;

    public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _password = webApplicationFactory.User_Team_Member.GetPassword();
        _email = webApplicationFactory.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;
        
        var response = await PutAsync(requestUri: USER_URI, token: _token, request: request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        response = await PostAsync("api/Auth", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await PostAsync("api/Auth", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Password_Different_Current_Password(string cultureInfo)
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var response = await PutAsync(requestUri: USER_URI, request: request, token: _token, culture: cultureInfo);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(cultureInfo));

        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}