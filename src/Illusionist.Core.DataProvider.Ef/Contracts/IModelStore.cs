using System;
using System.Collections.Generic;

namespace Illusionist.Core.DataProvider.Ef.Contracts
{
    public interface IModelStore
    {
        IReadOnlyCollection<Type> GetModels();
    }
}