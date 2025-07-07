using Microsoft.EntityFrameworkCore;
using MinimalAPIKickoff.Application.Services;
using MinimalAPIKickoff.Domain.Entities;
using MinimalAPIKickoff.Infrastructure.Data;

namespace MinimalAPIKickoff.Tests.Units;

public class UserServiceTests
{
    private readonly DbContextOptions<MinimalAPIKickoffContext> _options;

    public UserServiceTests()
    {
        _options = new DbContextOptionsBuilder<MinimalAPIKickoffContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public async Task AoCadastrarUserSemNomeDeveRetornarFalha()
    {
        // Arrange
        using var context = new MinimalAPIKickoffContext(_options);
        context.Users.Add(new User
        {
            Email = "teste@abc.com",
        });

        //act
        var ex = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await context.SaveChangesAsync(); 
        });
        var service = new UserService(context);

        // Assert
        Assert.Contains("Required properties", ex.Message);
    }

    [Fact]
    public async Task AoCadastrarUserComTodasInformacoesDeveRetornarSucesso()
    {
        // Arrange
        using var context = new MinimalAPIKickoffContext(_options);
        context.Users.Add(new User { Email = "teste@abc.com",
                                     Name = "teste" });
        context.SaveChanges();

        var service = new UserService(context);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Single(result);
    }

}
