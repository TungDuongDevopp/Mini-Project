using Application.Interface;
using Domain.Entity;
namespace Application.Service;

public class CustomerService : IBaseRepository<Customer>
{
    private readonly IBaseRepository<Customer> _repo;


    public CustomerService(IBaseRepository<Customer> repo)
    {
        _repo = repo;
    }

    public void Create(Customer customer)
    {
        customer.Email = NormalizeEmail(customer.Email);
        customer.PhoneNumber = NormalizePhone(customer.PhoneNumber);

        if (!IsEmailUnique(customer.Email))
            throw new Exception("Email đã tồn tại");

        if (!IsPhoneUnique(customer.PhoneNumber))
            throw new Exception("SĐT đã tồn tại");

        _repo.Create(customer);
    }

    public bool Update(Customer customer)
    {
        var existing = _repo.GetById(customer.CustomerId);
        if (existing == null) return false;

        var email = NormalizeEmail(customer.Email);
        var phone = NormalizePhone(customer.PhoneNumber);

        if (!IsEmailUnique(email, customer.CustomerId))
            throw new Exception("Email đã tồn tại");

        if (!IsPhoneUnique(phone, customer.CustomerId))
            throw new Exception("SĐT đã tồn tại");

        customer.Email = email;
        customer.PhoneNumber = phone;

        return _repo.Update(customer);
    }

    public bool Delete(int id)=> _repo.Delete(id);


    public IReadOnlyList<Customer> GetAll() => _repo.GetAll().ToList();

    public Customer? GetById(int id) => _repo.GetById(id);

    // =============================
    // BUSINESS LOGIC
    // =============================

    public bool IsEmailUnique(string email)
        => !_repo.GetAll()
            .Any(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    public bool IsEmailUnique(string email, int currentId)
        => !_repo.GetAll()
            .Any(c => c.CustomerId != currentId &&
                      c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    public bool IsPhoneUnique(string phone)
        => !_repo.GetAll()
            .Any(c => c.PhoneNumber == phone);

    public bool IsPhoneUnique(string phone, int currentId)
        => !_repo.GetAll()
            .Any(c => c.CustomerId != currentId &&
                      c.PhoneNumber == phone);

    // =============================
    // NORMALIZE
    // =============================

    private string NormalizeEmail(string email)
        => email.Trim().ToLower();

    private string NormalizePhone(string phone)
        => phone.Trim();
}