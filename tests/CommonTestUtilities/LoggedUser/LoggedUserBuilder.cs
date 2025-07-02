﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.LoggedUser;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Moq.Mock<ILoggedUser>();

        mock.Setup(u => u.Get()).ReturnsAsync(user);

        return mock.Object;
    }
}
