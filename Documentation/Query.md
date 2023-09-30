Constructors:

```C#

public QueryAttribute(
  string query,
  string methodName,
  Type queryMapType = null,
  MethodType methodType = MethodType.Sync,
  SourceType sourceType = SourceType.Connection,
  QueryType queryType = QueryType.Read,
  bool generate = true,
  AccessModifier accessModifier = AccessModifier.AsContainingClass,
  AsyncResult asyncResultType = AsyncResult.ValueTask,
  Type asPartInterface = null
  )

```
Unique parametrs:<br>
`sourceType`: source type(`NpgsqlConnection`/`NpgsqlDataSource`)<br>

Rest parametrs and usage same as [Query](https://github.com/SoftStoneDevelop/Gedaq.DbConnection/blob/main/Documentation/Query.md).
