 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
   public interface IDataStore<T>
    {
        List<T> Load();
        void Save(List<T> data);
    }
}
