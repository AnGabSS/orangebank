namespace OrangeBank.Core.Domain.Entities;

public class User : IEntity
{
    private DateTime birthDate;

    public Guid Id { get; }
	public string Name { get;  set; }
	public string Email { get;  set; }
	public string CPF { get;  set; }
    public string Password { get;  set; }
	public DateOnly BirthDate { get;  set; }
	public string PhoneNumber { get;  set; }
    public DateTime CreatedAt { get;  set; }
	public DateTime UpdatedAt { get;  set; }


    protected User()
    {
        Name = string.Empty;
        Email = string.Empty;
        CPF = string.Empty;
        Password = string.Empty;
        PhoneNumber = string.Empty;
    }

    public User(string name, string email, string cpf, string password, DateOnly birthdate, string phonenumber)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        CPF = cpf ?? throw new ArgumentNullException(nameof(cpf));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        BirthDate = birthdate;
        PhoneNumber = phonenumber ?? throw new ArgumentNullException(nameof(phonenumber));
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow; 
    }


    public void UpdateBasicInfo(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }

}
