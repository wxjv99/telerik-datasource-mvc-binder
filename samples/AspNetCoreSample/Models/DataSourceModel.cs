using System.ComponentModel.DataAnnotations;

namespace AspNetCoreSample.Models;

public class DataSourceModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int Value { get; set; }

    public DataSourceModel(int id, string name, int value)
    {
        Id = id;
        Name = name;
        Value = value;
    }
}
