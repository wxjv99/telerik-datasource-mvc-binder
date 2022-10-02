namespace AspNetCoreSample.Entities;

public class DataSourceEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Value { get; set; }

    public DataSourceEntity(int id, string name, int value)
    {
        Id = id;
        Name = name;
        Value = value;
    }
}

