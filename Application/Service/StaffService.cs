

using Application.Interface;
using Domain.Entity;

namespace Application.Service;

public class StaffService : IBaseRepository<Staff>

{
    private readonly IBaseRepository<Staff> _staffRepository;
    public StaffService(IBaseRepository<Staff> staffRepository)
    {
        _staffRepository = staffRepository;
    }
    public void Create(Staff entity)
   => _staffRepository.Create(entity);

    public bool Delete(int id)
    => _staffRepository.Delete(id);

    public IReadOnlyList<Staff> GetAll()
    => _staffRepository.GetAll();

    public Staff? GetById(int id)
    => _staffRepository.GetById(id);
    

    public bool Update(Staff entity)
    => _staffRepository.Update(entity);
    
}
