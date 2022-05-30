using System;
using Illusionist.Core.DataProvider.Postgres;
using Npgsql;

namespace Illusionist.Core.DataProvider.Tests.Helpers
{
    internal class PostgresDataExceptionManagerTest : PostgresDataExceptionManager
    {
        public void NormalizePostgresExceptionTest(
            PostgresException postgresException,
            Exception innerException)
        {
            NormalizePostgresException(postgresException, innerException);
        }
    }
}