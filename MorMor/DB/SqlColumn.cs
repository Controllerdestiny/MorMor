using MySql.Data.MySqlClient;

namespace MorMor.DB;

public class SqlColumn
{
    //Required
    public string Name { get; set; }
    public MySqlDbType Type { get; set; }


    //Optional
    /// <summary>
    /// Sets/Gets if it's unique 
    /// </summary>
    public bool Unique { get; set; }
    /// <summary>
    /// Sets/Gets if it's primary key
    /// </summary>
    public bool Primary { get; set; }
    /// <summary>
    /// Sets/Gets if it autoincrements
    /// </summary>
    public bool AutoIncrement { get; set; }
    /// <summary>
    /// Sets/Gets if it can be or not null
    /// </summary>
    public bool NotNull { get; set; }
    /// <summary>
    /// Sets the default value
    /// </summary>
    public string DefaultValue { get; set; }
    /// <summary>
    /// Use on DateTime only, if true, sets the default value to the current date when creating the row.
    /// </summary>
    public bool DefaultCurrentTimestamp { get; set; }

    /// <summary>
    /// Length of the data type, null = default
    /// </summary>
    public int? Length { get; set; }

    public SqlColumn(string name, MySqlDbType type)
        : this(name, type, null)
    {
    }

    public SqlColumn(string name, MySqlDbType type, int? length)
    {
        Name = name;
        Type = type;
        Length = length;
    }
}

/// <summary>
/// Used when a SqlColumn has validation errors.
/// </summary>
[Serializable]
public class SqlColumnException : Exception
{
    /// <summary>
    /// Creates a new SqlColumnException with the given message.
    /// </summary>
    /// <param name="message"></param>
    public SqlColumnException(string message) : base(message)
    {
    }
}
