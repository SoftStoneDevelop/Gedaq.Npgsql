Constructors:

```C#

public BinaryExportAttribute(
        string query,
        string methodName,
        Type queryMapType,
        MethodType methodType = MethodType.Sync,
        SourceType sourceType = SourceType.Connection
        )

```
Parametrs:<br>
`query`: sql query<br>
`methodName`: name of the generated method<br>
`queryMapType`: Type of result mapping collection<br>
`methodType`: type of generated method(sync/async, flags enum)<br>
`sourceType`: type of connection source<br>

Model classes in example:
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

```

Usage from table:

```C#

[BinaryExport(@"
COPY readfixtureperson 
(
id,
firstname,
middlename,
lastname,
~StartInner::Identification:id~
    ~Reinterpret::id~
readfixtureidentification_id
~EndInner::Identification~
) TO STDOUT (FORMAT BINARY)
", 
            "BinaryExportTable",
            typeof(Person), 
            Gedaq.Common.Enums.MethodType.Sync | Gedaq.Common.Enums.MethodType.Async
            )]
public async Task SomeMethod(NpgsqlConnection connection)
{
    var persons = connection.BinaryExportTable().ToList();
    var personsAsync = await connection.BinaryExportTableAsync().ToListAsync();
}
```
We had to use `~Reinterpret::id~` because `COPY` doesn't support aliases.

Usage from subquery:

```C#

[BinaryExport(@"
COPY 
(
SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
~StartInner::Identification:id~
    i.id,
    i.typename,
~StartInner::Country:id~
    c.id,
    c.name
~EndInner::Country~
~EndInner::Identification~
FROM readfixtureperson p
LEFT JOIN readfixtureidentification i ON i.id = p.readfixtureidentification_id
LEFT JOIN readfixturecountry c ON c.id = i.readfixturecountry_id
ORDER BY p.id ASC
) TO STDOUT (FORMAT BINARY)
", 
            "BinaryExportSubquery",
            typeof(Person), 
            Gedaq.Common.Enums.MethodType.Sync | Gedaq.Common.Enums.MethodType.Async
            )]
public async Task SomeMethod(NpgsqlConnection connection)
{
    var persons = connection.BinaryExportSubqueryTable().ToList();
    var personsAsync = await connection.BinaryExportSubqueryAsync().ToListAsync();
}
```

Note that simple properties(like `p.firstname`) always precede `Inner`(like `~StartInner::Identification:id~`) properties in queries. This is a feature of the generator and the `BinaryExport` command.
