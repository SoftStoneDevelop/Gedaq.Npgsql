Static query:

```C#

public class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public Identification Identification { get; set; }
}

public class Identification
{
    public int Id { get; set; }
    public string TypeName { get; set; }
    public Country Country { get; set; }
}

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
}

```

Usage from table:

```C#

[BinaryExport(query:
            query: @"
COPY person 
(
id,
firstname,
~StartInner::Identification:id~
    ~Reinterpret::id~
identification_id,
~EndInner::Identification~
middlename,
lastname
) TO STDOUT (FORMAT BINARY)
", 
            methodName: "BinaryExportTable",
            queryMapTypes: [typeof(Person)], 
            methodType: Gedaq.Common.Enums.MethodType.Sync | Gedaq.Common.Enums.MethodType.Async),
            Gedaq.Npgsql.Attributes.DbTypesOverride(0, new NpgsqlDbType[]
            {
                NpgsqlTypes.NpgsqlDbType.Integer,
                NpgsqlTypes.NpgsqlDbType.Text,
                NpgsqlTypes.NpgsqlDbType.Integer,
                NpgsqlTypes.NpgsqlDbType.Text,
            })]
public async Task SomeMethod(NpgsqlConnection connection)
{
    var persons = BinaryExportTable(connection);
    var personsAsync = await BinaryExportTableAsync(connection);
}
```
DbTypesOverride specifies how exactly to read the data; if no override is specified, then attributes above the properties in the type must be specified.

We had to use `~Reinterpret::id~` because `COPY` doesn't support aliases.

Usage from subquery:

```C#

[BinaryExport(query: @"
COPY 
(
SELECT 
    p.id,
~StartInner::Identification:id~
    i.id,
~StartInner::Country:id~
    c.id,
    c.name,
~EndInner::Country~
    i.typename,
~EndInner::Identification~
    p.firstname,
    p.middlename,
    p.lastname
FROM person p
LEFT JOIN identification i ON i.id = p.identification_id
LEFT JOIN country c ON c.id = i.country_id
ORDER BY p.id ASC
) TO STDOUT (FORMAT BINARY)
", 
            methodName: "BinaryExportSubquery",
            queryMapTypes: [typeof(Person)], 
            methodType: Gedaq.Common.Enums.MethodType.Sync | Gedaq.Common.Enums.MethodType.Async),
            Gedaq.Npgsql.Attributes.DbTypesOverride(0, new NpgsqlDbType[]
            {
                NpgsqlTypes.NpgsqlDbType.Integer,
                NpgsqlTypes.NpgsqlDbType.Text,
                NpgsqlTypes.NpgsqlDbType.Integer,
                NpgsqlTypes.NpgsqlDbType.Text,
            })]
public async Task SomeMethod(NpgsqlConnection connection)
{
    var persons = BinaryExportSubqueryTable(connection);
    var personsAsync = await BinaryExportSubqueryAsync(connection);
}
```

Otherwise, the behavior is the same as [the Query attribute](https://github.com/SoftStoneDevelop/Gedaq.DbConnection/blob/main/Documentation/Query.md). Both dynamic queries and multimapping are supported.
