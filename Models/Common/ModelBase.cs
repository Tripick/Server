using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TripickServer.Models.Common
{
    public interface ModelBase<T> where T : new()
    {

        public T ToDTO();
    }
}
