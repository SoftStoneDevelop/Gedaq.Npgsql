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
|       **Gedaq.Npgsql** |   **50** |  **6.833 ms** |  **0.94** |  **39.85 KB** |        **0.62** |
| Gedaq.DbConnection |   50 |  7.266 ms |  1.00 | 64.07 KB |        1.00 |
|                    |      |           |       |           |             |
|       **Gedaq.Npgsql** |  **100** | **14.166 ms** |  **1.02** |  **79.71 KB** |        **0.62** |
| Gedaq.DbConnection |  100 | 13.852 ms |  1.00 | 128.14 KB |        1.00 |
|                    |      |           |       |           |             |
|       **Gedaq.Npgsql** |  **200** | **28.363 ms** |  **0.99** | **159.41 KB** |        **0.62** |
| Gedaq.DbConnection |  200 | 28.745 ms |  1.00 | 256.28 KB |        1.00 |
