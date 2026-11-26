using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}
