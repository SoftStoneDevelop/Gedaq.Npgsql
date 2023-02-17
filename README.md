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

Attributes required for Gedaq ORM operation for PostgreSQL database.

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

|                     Method | Size |      Mean | Ratio |    Gen0 | Allocated | Alloc Ratio |
|--------------------------- |----- |----------:|------:|--------:|----------:|------------:|
|       **&#39;Gedaq.Npgsql Async&#39;** |   **50** |  **8.636 ms** |  **1.06** |       **-** | **102.89 KB** |        **0.78** |
| &#39;Gedaq.DbConnection Async&#39; |   50 |  8.110 ms |  1.00 | 15.6250 | 132.63 KB |        1.00 |
|                            |      |           |       |         |           |             |
|       **&#39;Gedaq.Npgsql Async&#39;** |  **100** | **14.977 ms** |  **0.92** | **15.6250** | **205.68 KB** |        **0.78** |
| &#39;Gedaq.DbConnection Async&#39; |  100 | 16.301 ms |  1.00 | 31.2500 | 265.07 KB |        1.00 |
|                            |      |           |       |         |           |             |
|       **&#39;Gedaq.Npgsql Async&#39;** |  **200** | **31.782 ms** |  **1.00** |       **-** |  **411.2 KB** |        **0.78** |
| &#39;Gedaq.DbConnection Async&#39; |  200 | 31.897 ms |  1.00 | 62.5000 | 529.95 KB |        1.00 |
