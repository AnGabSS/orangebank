using System;

public class User : IEntity
{
	public Guid Id { get; protected set; }
	public string Name { get; protected set; }
	public string Email { get; protected set; }
	public string CPF { get; protected set; }
    public string Password { get; protected set; }
	public DateOnly BirthDate { get; protected set; }
	public string PhoneNumber { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
	public DateTime UpdatedAt { get; protected set; }


    protected User() { }

    public User(string name, string email, string cpf, string password, DateOnly birthdate, string phonenumber)
	{
		Id = Guid.NewGuid();
		Name = name;
		Email = email;
		CPF = cpf;
		Password = password;
		BirthDate = birthdate;
		PhoneNumber = phonenumber;
		CreatedAt = DateTime.UtcNow;
    }

    public void UpdateBasicInfo(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }

}
