using System;
using Illusionist.DataProvider.Postgres;
using Npgsql;

namespace Illusionist.DataProvider.Tests.Helpers
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