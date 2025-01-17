using System.ComponentModel.DataAnnotations;

namespace AspNetCoreSample.Models;

public class DataSourceModel
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int Value { get; set; }

    public DataSourceModel()
    {
        Id = 0;
        Name = string.Empty;
        Value = 0;
    }

    public DataSourceModel(int id, string name, int value)
    {
        Id = id;
        Name = name;
        Value = value;
    }
}
