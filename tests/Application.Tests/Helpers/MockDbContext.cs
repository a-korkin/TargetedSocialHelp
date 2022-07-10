using Application.Interfaces;
using Domain.Entities.Admin;
using MockQueryable.Moq;
using Moq;

namespace Application.Tests.Helpers;

public static class MockDbContext
{
    public static User AdminUser => new()
    {
        Id = Guid.Parse("97159bc1-2ffd-421e-acb2-a07d869526c6"),
        UserName = "admin",
        Password = "$2a$12$e5V40L6Xqu.crMn5Qe3.JOr5PjBrUxFqebkGROZ0Yons0U4x6a.J."
    };

    public static Mock<IApplicationDbContext> Create()
    {
        Mock<IApplicationDbContext> mockContext = new();
        List<User> users = new() { AdminUser };
        var mockUsers = users.AsQueryable().BuildMockDbSet();
        mockContext.Setup(x => x.Set<User>()).Returns(mockUsers.Object);

        return mockContext;
    }
}