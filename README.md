<h1 align="center">
  <a>Gedaq.Npgsql</a>
</h1>

<h3 align="center">

  [![Nuget](https://img.shields.io/nuget/v/Gedaq.Npgsql?logo=Gedaq.Npgsql)](https://www.nuget.org/packages/Gedaq.Npgsql/)
  [![Downloads](https://img.shields.io/nuget/dt/Gedaq.Npgsql.svg)](https://www.nuget.org/packages/Gedaq.Npgsql/)
  [![Stars](https://img.shields.io/github/stars/SoftStoneDevelop/Gedaq.Npgsql?color=brightgreen)](https://github.com/SoftStoneDevelop/Gedaq.Npgsql/stargazers)
  [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

</h3>

<h3 align="center">
  <a href="https://github.com/SoftStoneDevelop/Gedaq.Npgsql/tree/main/Documentation/Readme.md">Documentation</a>
</h3>

Attributes required for [Gedaq ORM](https://github.com/SoftStoneDevelop/Gedaq) operation for PostgreSQL database and [Npgsql](https://github.com/npgsql/npgsql).

Simple single parameter(int) query comparison Gedaq.Npgsql vs Gedaq.DbConnection:

| Method             | Calls | Mean      | Ratio | Allocated | Alloc Ratio |
|------------------- |------ |----------:|------:|----------:|------------:|
| **Gedaq.Npgsql**       | **50**    |  **35.49 ms** |  **1.06** |   **45.9 KB** |        **0.65** |
| Gedaq.DbConnection | 50    |  33.52 ms |  1.00 |  70.59 KB |        1.00 |
|                    |       |           |       |           |             |
| **Gedaq.Npgsql**       | **100**   |  **65.87 ms** |  **0.97** |  **91.21 KB** |        **0.65** |
| Gedaq.DbConnection | 100   |  68.24 ms |  1.00 | 140.52 KB |        1.00 |
|                    |       |           |       |           |             |
| **Gedaq.Npgsql**       | **200**   | **126.53 ms** |  **0.95** | **181.84 KB** |        **0.65** |
| Gedaq.DbConnection | 200   | 134.23 ms |  1.00 | 280.36 KB |        1.00 |


Something is still not clear and [Documentation](https://github.com/SoftStoneDevelop/Gedaq.Npgsql/tree/main/Documentation/Readme.md) does not help? [Feel free to ask a question on StackOverflow](https://stackoverflow.com/questions/ask?tags=gedaq,npgsql,c%23)
