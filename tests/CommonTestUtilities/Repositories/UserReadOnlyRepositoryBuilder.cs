﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;
    
    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public void ConfigureExistActiveUserWithEmail(string email)
    {
        _repository
            .Setup(userReadOnly => userReadOnly.ExistActiveUserWithEmail(email))
            .ReturnsAsync(true);
    }

    public UserReadOnlyRepositoryBuilder ConfigureGetUserByEmail(User user)
    {
        _repository
            .Setup(userReadOnly => userReadOnly.GetUserByEmail(user.Email))
            .ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}