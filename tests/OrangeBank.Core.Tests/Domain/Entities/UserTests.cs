using Xunit;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var name = "John Doe";
        var email = "john@example.com";
        var cpf = "12345678909";
        var passwordHash = "hashed123";
        var birthDate = new DateOnly(1990, 1, 1);
        var phone = "11999999999";

        // Act
        var user = new User(name, email, cpf, passwordHash, birthDate, phone);

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(cpf, user.CPF);
        Assert.Equal(passwordHash, user.Password);
        Assert.Equal(birthDate, user.BirthDate);
        Assert.Equal(phone, user.PhoneNumber);
        Assert.True(user.CreatedAt <= DateTime.UtcNow);
        Assert.True(user.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void UpdateBasicInfo_ShouldUpdateNameAndPhone()
    {
        // Arrange
        var user = new User("Old Name", "test@test.com", "12345678909",
                           "hash", new DateOnly(1990, 1, 1), "11999999999");

        var newName = "New Name";
        var newPhone = "11888888888";
        var originalUpdatedAt = user.UpdatedAt;

        // Act
        user.UpdateBasicInfo(newName, newPhone);

        // Assert
        Assert.Equal(newName, user.Name);
        Assert.Equal(newPhone, user.PhoneNumber);
        Assert.True(user.UpdatedAt > originalUpdatedAt);
    }
}