[Code](https://github.com/SoftStoneDevelop/Gedaq.Npgsql/blob/main/Src/NpgsqlBenchmark/Benchmarks/ReadInnerMap.cs)

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
