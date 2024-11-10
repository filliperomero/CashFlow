﻿using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToRequest();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestExpenseJson, Expense>();
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());
    }

    private void EntityToRequest()
    {
        CreateMap<Expense, ResponseRegisterExpenseJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<Expense, ResponseExpenseJson>();
        CreateMap<User, ResponseUserProfileJson>();
    }
}
