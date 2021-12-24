using System;
using System.Collections.Generic;

namespace Illusionist.DataProvider.Ef.Contracts
{
    public interface IModelStore
    {
        IReadOnlyCollection<Type> GetModels();
    }
}