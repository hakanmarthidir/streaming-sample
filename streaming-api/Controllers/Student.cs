namespace streaming_api.Controllers;

public class Student
{
    public string Name { get; set; }

    public override string ToString()
    {
        return this.Name;
    }
}
