using Payment.Service.Entity;
using System.Threading.Tasks;

namespace Payment.Service.Business
{
    public interface IRepository
    {
        Task<Student> GetStudentByID(int studentId);
    }
}