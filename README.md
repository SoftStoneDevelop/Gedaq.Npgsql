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

|             Method | Size |      Mean | Ratio | Allocated | Alloc Ratio |
|------------------- |----- |----------:|------:|----------:|------------:|
|       **Gedaq.Npgsql** |   **50** |  **7.307 ms** |  **0.99** |  **39.85 KB** |        **0.63** |
| Gedaq.DbConnection |   50 |  7.396 ms |  1.00 |  63.29 KB |        1.00 |
|                    |      |           |       |           |             |
|       **Gedaq.Npgsql** |  **100** | **14.781 ms** |  **0.99** |  **79.71 KB** |        **0.63** |
| Gedaq.DbConnection |  100 | 14.974 ms |  1.00 | 126.58 KB |        1.00 |
|                    |      |           |       |           |             |
|       **Gedaq.Npgsql** |  **200** | **29.524 ms** |  **1.00** | **159.44 KB** |        **0.63** |
| Gedaq.DbConnection |  200 | 29.570 ms |  1.00 | 253.16 KB |        1.00 |
