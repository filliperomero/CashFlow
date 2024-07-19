using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Auth;

public class LoginUseCase : ILoginUseCase
{
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    
    public LoginUseCase(
        IPasswordEncrypter passwordEncrypter,
        IUserReadOnlyRepository userReadOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _passwordEncrypter = passwordEncrypter;
        _userReadOnlyRepository = userReadOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);

        if (user is null || _passwordEncrypter.Verify(request.Password, user.Password) is false)
        {
            throw new InvalidCredentialsException();
        }
        
        return new ResponseRegisteredUserJson()
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }
}