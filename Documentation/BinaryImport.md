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
}

[BinaryImport(
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
) 
FROM STDIN (FORMAT BINARY)
", 
            methodName: "BinaryImport",
            queryMapType: typeof(Person),
            dbTypes: new NpgsqlDbType[] 
            { 
                NpgsqlDbType.Integer,//id
                NpgsqlDbType.Text,//firstname
                NpgsqlDbType.Integer,//identification_id
                NpgsqlDbType.Text,//middlename
                NpgsqlDbType.Text//lastname
            },
            methodType: Gedaq.Common.Enums.MethodType.Sync | Gedaq.Common.Enums.MethodType.Async)]
public async Task SomeMethod(NpgsqlConnection connection, List<Person> list)
{
    connection.BinaryImport(list);
    await connection.BinaryImportAsync(list);
}
```
________

Dynamic query:
Dynamic queries do not support special syntax, so the class model is different from a static query.

```C#

public class Person
{
    [Gedaq.Common.Attributes.Alias(order: 0)]
    public int Id { get; set; }

    [Gedaq.Common.Attributes.Alias(order: 1)]
    public string FirstName { get; set; }

    [Gedaq.Common.Attributes.Alias(order: 3)]
    public string MiddleName { get; set; }

    [Gedaq.Common.Attributes.Alias(order: 4)]
    public string LastName { get; set; }

    [Gedaq.Common.Attributes.Alias(order: 2)]
    public int IdentificationId { get; set; }
}

[BinaryImport(
            query: null,
            methodName: "BinaryImport",
            queryMapType: typeof(Person),
            dbTypes: new NpgsqlDbType[] 
            { 
                NpgsqlDbType.Integer,
                NpgsqlDbType.Text,
                NpgsqlDbType.Integer,
                NpgsqlDbType.Text,
                NpgsqlDbType.Text
            },
            methodType: Gedaq.Common.Enums.MethodType.Sync | Gedaq.Common.Enums.MethodType.Async)]
public async Task SomeMethod(NpgsqlConnection connection, List<Person> list)
{
    BinaryImport(connection, list, @"
COPY person 
(
id,
firstname,
identification_id,
middlename,
lastname
) 
FROM STDIN (FORMAT BINARY)
");
}
```

For information on how to use the alias attribute, see [Query](https://github.com/SoftStoneDevelop/Gedaq.DbConnection/blob/main/Documentation/Query.md).

DbTypes explicitly allows overriding the behavior of specified DbType attributes. It's mandatory that either the attributes be specified or the overrides be specified in the query.
Here's what it looks like with attributes.

```C#

public class Person
{
    [Gedaq.Npgsql.Attributes.DbType(NpgsqlTypes.NpgsqlDbType.Integer)]
    [Gedaq.Common.Attributes.Alias(order: 0)]
    public int Id { get; set; }

    [Gedaq.Npgsql.Attributes.DbType(NpgsqlTypes.NpgsqlDbType.Text)]
    [Gedaq.Common.Attributes.Alias(order: 1)]
    public string FirstName { get; set; }

    [Gedaq.Npgsql.Attributes.DbType(NpgsqlTypes.NpgsqlDbType.Text)]
    [Gedaq.Common.Attributes.Alias(order: 3)]
    public string MiddleName { get; set; }

    [Gedaq.Npgsql.Attributes.DbType(NpgsqlTypes.NpgsqlDbType.Text)]
    [Gedaq.Common.Attributes.Alias(order: 4)]
    public string LastName { get; set; }

    [Gedaq.Npgsql.Attributes.DbType(NpgsqlTypes.NpgsqlDbType.Integer)]
    [Gedaq.Common.Attributes.Alias(order: 2)]
    public int IdentificationId { get; set; }
}

[BinaryImport(
            query: null,
            methodName: "BinaryImport",
            queryMapType: typeof(Person),
            dbTypes: null,
            methodType: Gedaq.Common.Enums.MethodType.Sync | Gedaq.Common.Enums.MethodType.Async)]
public async Task SomeMethod(NpgsqlConnection connection, List<Person> list)
{
    BinaryImport(connection, list, @"
COPY person 
(
id,
firstname,
identification_id,
middlename,
lastname
) 
FROM STDIN (FORMAT BINARY)
");
}
```
