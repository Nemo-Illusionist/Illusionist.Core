using System;
using Illusionist.Core.DataProvider.Exceptions;

namespace Illusionist.Core.DataProvider.Contracts
{
    public interface IDataExceptionManager
    {
        DataProviderException Normalize(Exception exception);
        
        bool IsRepeatableException(Exception exception);
    }
}