using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Auth;

public interface ILoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}