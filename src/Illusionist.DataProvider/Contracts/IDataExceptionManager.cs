using System;
using Illusionist.DataProvider.Exceptions;

namespace Illusionist.DataProvider.Contracts
{
    public interface IDataExceptionManager
    {
        DataProviderException Normalize(Exception exception);
        
        bool IsRepeatableException(Exception exception);
    }
}