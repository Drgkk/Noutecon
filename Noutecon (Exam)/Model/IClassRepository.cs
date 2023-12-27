using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public interface IClassRepository
    {
        void Add(ClassModel classModel);
        void Edit(ClassModel classModel);
        void Remove(ClassModel classModel);
        ClassModel GetById(int id);
        ClassModel GetByUniqueId(string uniqueId);
        IEnumerable<ClassModel> GetAll();
    }
}
