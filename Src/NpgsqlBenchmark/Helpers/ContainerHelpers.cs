using DotNet.Testcontainers.Containers;
using Npgsql;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace NpgsqlBenchmark.Helpers
{
    internal static class ContainerHelpers
    {
        /// <summary>
        /// Wait untill container start
        /// </summary>
        public static async ValueTask WaitContainerStateRunningAsync(this DockerContainer dockerContainer, TimeSpan timeout)
        {
            var sw = Stopwatch.StartNew();
            while (true)
            {
                if (dockerContainer.State == TestcontainersStates.Running)
                {
                    break;
                }

                if (sw.Elapsed >= timeout)
                {
                    throw new Exception($"Container start timeout ({timeout}) exceeded, benchmark stopped, current container state is {dockerContainer.State}.");
                }

                await Task.Delay(100);
            }
        }

        /// <summary>
        /// Wait until the container responds to at least one request
        /// </summary>
        public static async ValueTask WaitResponseAsync(this PostgreSqlContainer container, TimeSpan timeout)
        {
            var sw = Stopwatch.StartNew();
            while (true)
            {
                if (sw.Elapsed >= timeout)
                {
                    throw new Exception($"Postgres has not responded to any queries in {timeout}. Container state {container.State}");
                }

                var connectionString = container.GetConnectionString();
                try
                {
                    await using (var conn = new NpgsqlConnection(connectionString))
                    {
                        await conn.OpenAsync();
                        await using var command = conn.CreateCommand();
                        command.CommandText = "SELECT 1";
                        var result = await command.ExecuteScalarAsync();
                        if (result is int value && value == 1)
                        {
                            break;
                        }
                    }
                }
                catch
                {
                    // игнорим
                }

                await Task.Delay(100);
            }
        }
    }
}
