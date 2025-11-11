using System.Text.Json.Serialization;

namespace CareMove.Argument.Base;

public class InputGenericDelete : BaseInputDelete<InputGenericDelete>
{
    public InputGenericDelete() { }

    [JsonConstructor]
    public InputGenericDelete(long id) : base(id) { }
}