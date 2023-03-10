// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

/// <summary>
/// SelaedKeyword class can inherite from BasedClassOne ,however can't inherite from
/// BasedClassSealed because it is a sealed class.
///
/// If you don't want other classes to inherit from a class, use the "sealed" keyword.
/// 
/// Ref : https://www.w3schools.com/cs/cs_inheritance.php
/// </summary>
public class SealedKeyword : BasedClassOne
{
    
}

public class BasedClassOne
{
    
}

public sealed class BasedClassSealed
{
    
}