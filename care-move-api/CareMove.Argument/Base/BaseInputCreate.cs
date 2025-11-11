namespace CareMove.Argument;

public abstract class BaseInputCreate<TInputCreate>
    where TInputCreate : BaseInputCreate<TInputCreate>
{ }

public class BaseInputCreate_0 : BaseInputCreate<BaseInputCreate_0> { }