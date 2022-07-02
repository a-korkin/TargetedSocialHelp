using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Attributes;

public class TableDescriptionAttribute : TableAttribute
{
    public string RuName { get; set; } = string.Empty;

    public TableDescriptionAttribute(string name, string schema, string ruName) : base(name)
    {
        Schema = schema;
        RuName = ruName;
    }
}