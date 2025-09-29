namespace Extra;

internal interface A
{
    void DoSomething();
}

internal class B : A
{
    public void DoSomething()
    {
        Console.WriteLine("Doing something in B");
    }
}