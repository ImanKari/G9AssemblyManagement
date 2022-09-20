[![G9TM](https://raw.githubusercontent.com/ImanKari/G9AssemblyManagement/main/G9AssemblyManagement/Asset/G9AssemblyManagement.png)](http://www.g9tm.com/) **G9AssemblyManagement**

[![NuGet version (G9AssemblyManagement)](https://img.shields.io/nuget/v/G9AssemblyManagement.svg?style=flat-square)](https://www.nuget.org/packages/G9AssemblyManagement/)
[![Azure DevOps Pipeline Build Status](https://raw.githubusercontent.com/ImanKari/G9JSONHandler/main/G9JSONHandler/Asset/AzureDevOpsPipelineBuildStatus.png)](https://g9tm.visualstudio.com/G9AssemblyManagement/_apis/build/status/G9AssemblyManagement-BuldSolution-PushNugetPackages-SyncGitRep?branchName=main)
[![Github Repository](https://raw.githubusercontent.com/ImanKari/G9JSONHandler/main/G9JSONHandler/Asset/GitHub.png)](https://github.com/ImanKari/G9AssemblyManagement)

# G9AssemblyManagement
### An efficient .NET library developed for working on assembly levels and using essential basic structures. This library contains various valuable tools related to [Types](#type-tools), [Instances](#instance-tools), [Objects](#object-and-reflection-tools), [Reflections](#object-and-reflection-tools), [Merging](#merging-two-objects), [Cryptographies](#cryptography-tools), [Performances](#performance-tools), and [General Tools](#general-tools).
### Indeed, there are so many utilities in this library, along with various overloads and parameters. But in the following, we review it as long as it is not beyond the scope of this guide.
# Overview
## Type Tools
### This helper tool provides many various utilities for working by the types.
Tool for getting inherited types from a specified type:
```csharp
var result = G9Assembly.TypeTools.GetInheritedTypesFromType(typeof(IPAddress));
Console.WriteLine(result.Count); //  1
Console.WriteLine(result[0].Name); // ReadOnlyIPAddress
```
- Indeed, the type 'ReadOnlyIPAddress' is derived from the type 'IPAddress.'

This tool checks whether a specified type is a built-in .NET type or not:
```csharp
// Some built-in .NET Types
var v1 = 1;
var v2 = DateTime.Now;
var v3 = IPAddress.Any;
// Custom type
var v4 = new CustomType();

Console.WriteLine(G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v1.GetType())); // true
Console.WriteLine(G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v1.GetType())); // true
Console.WriteLine(G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v3.GetType())); // true
Console.WriteLine(G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v4.GetType())); // false
```
A practical tool for changing the type of an object along with an intelligent value-changing process in possible conditions:
```csharp
// Changes to string and number
var consoleColorString = G9Assembly.TypeTools.SmartChangeType<string>(ConsoleColor.DarkMagenta); 
// "DarkMagenta"
var consoleColorNumber = G9Assembly.TypeTools.SmartChangeType<int>(ConsoleColor.DarkMagenta); 
// 5

// Changes to primary type from string and number
var consoleColor = G9Assembly.TypeTools.SmartChangeType<ConsoleColor>(consoleColorString); 
// ConsoleColor.DarkMagenta
consoleColor = G9Assembly.TypeTools.SmartChangeType<ConsoleColor>(consoleColorNumber); 
// ConsoleColor.DarkMagenta
```
## Instance Tools
### This helper tool provides many practical utilities for working by the instances.
We sometimes need to access some instances of an object that aren't specified where they have been created, a process that is a little similar to dependency injection. In this case, this tool provides two ways of doing that. \
Sample classes:
```csharp
// In this scenario, we must inherit the abstract class 'G9AClassInitializer' in our desired classes.

// First sample
public class MyCustomClassA : G9AClassInitializer
{
    public string GetClassName()
    {
        return nameof(MyCustomClassA);
    }
}

// Second sample
public class MyCustomClassB : G9AClassInitializer
{
    public string GetClassName()
    {
        return nameof(MyCustomClassB);
    }
}
```
First way:
```csharp
// In the following, each created instance of the above samples is accessible in the following way:

private static void Main()
{
    // MyCustomClassA
    var instance1 = new MyCustomClassA();
    var instance2 = new MyCustomClassA();
    var instance3 = new MyCustomClassA();

    // MyCustomClassB
    var instance4 = new MyCustomClassB();
    var instance5 = new MyCustomClassB();

    var instancesOfClassA = G9Assembly.InstanceTools.GetInstancesOfType<MyCustomClassA>();
    Console.WriteLine(instancesOfClassA.Count); // 3
    Console.WriteLine(instancesOfClassA[0].GetClassName()); // MyCustomClassA

    var instancesOfClassB = G9Assembly.InstanceTools.GetInstancesOfType<MyCustomClassB>();
    Console.WriteLine(instancesOfClassA.Count); // 2
    Console.WriteLine(instancesOfClassA[0].GetClassName()); // MyCustomClassB
}
```
Second way:
```csharp
// Or, with a listener, we can control the new instance, removed instance, and exceptions:
private static void Main()
{
     var instanceListener =
        G9Assembly.InstanceTools.AssignInstanceListener<MyCustomClassA>
        (
            // On assign
            newInstance =>
            {
                // Do anything
            },
            // On Unassigning
            instance =>
            {
                // Do anything
            },
            // On receive exception
            ex =>
            {
                // Do anything
            }
        );
}
```
Sometimes, we need to create an instance of a type that there are many ways depending on the conditions:
```csharp
// Method to create an instance from a type, first way:
G9Assembly.InstanceTools.CreateInstanceFromType<CustomType>();
// Method to create an instance from a type, second way:
G9Assembly.InstanceTools.CreateInstanceFromType(typeof(CustomType));
// Method to create an instance from a type with the constructor that has parameters
G9Assembly.InstanceTools.CreateInstanceFromTypeWithConstructorParameters(typeof(CustomType), "param1", "param2");
// Method to create an uninitialized instance from a type
G9Assembly.InstanceTools.CreateUninitializedInstanceFromType<IPAddress>();
// Method to create an instance from a generic type
G9Assembly.InstanceTools.CreateInstanceFromGenericType(
    typeof(GenericCustomType<>), typeof(IPAddress));
// Method to create an instance from a generic type with the constructor that has parameters
G9Assembly.InstanceTools.CreateInstanceFromGenericTypeWithConstructorParameters(
    typeof(GenericCustomType<,,>),
    // Custom generic types
    new[] { typeof(IPAddress), typeof(int), typeof(string) },
    // Specifies parameters
    "Param 1", "param 2" );
```
## Object And Reflection Tools
### This helper tool consists of various tools related to reflections and objects. 
This tool provides many helpful methods for working with object members (reflection). All of the methods shown below have many functional parameters like the access modifier, custom filter, and so on that aren't shown:
```csharp
// Note: The output of the whole reflection methods below is an array.

// Method to get fields of an object
var fields = G9Assembly.ObjectAndReflectionTools.GetFieldsOfObject(myCustomObject);
// Method to get properties of an object
var properties = G9Assembly.ObjectAndReflectionTools.GetPropertiesOfObject(myCustomObject);

// Common methods between fields and properties
Console.WriteLine(fields[0].GetValue()); // Method for getting the value of fields/properties.
properties[0].SetValue("Custom value"); // Method for setting a new value for fields/properties.

// Method to get Methods of an object
var methods = G9Assembly.ObjectAndReflectionTools.GetMethodsOfObject(myCustomObject)
methods[0].CallMethod(); // method without result
var result1 = methods[1].CallMethodWithResult<int>(
    // If the desired method has parameters.
    6, 3); // method with result.

// Method to get generic methods of an object
var generigMethods = G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfObject(object1);
generigMethods[0].CallMethod(); // method without result
var result2 = generigMethods[1].CallMethodWithResult<int>(
    // Specifies the generic types for the method.
    new[] { typeof(int), typeof(string), typeof(IPAddress) },
    // If the desired method has parameters.
    1, "G9TM", IPAddress.Broadcast); // method with result.


// There are also similar methods for working on type members.
// Method to get properties of a type
G9Assembly.ObjectAndReflectionTools.GetPropertiesOfType(typeof(myCustomObject));
// Method to get fields of a type
G9Assembly.ObjectAndReflectionTools.GetFieldsOfType(typeof(myCustomObject));
// Method to get properties of a type
G9Assembly.ObjectAndReflectionTools.GetPropertiesOfType(typeof(myCustomObject));
// Method to get Methods of a type
G9Assembly.ObjectAndReflectionTools.GetMethodsOfType(typeof(myCustomObject));
// Method to get generic methods of a type
G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfType(typeof(myCustomObject));
```
### Merging two objects

The merger tool provides an operational process for merging two objects. With that, you can make a custom process for integrating two things under many conditions. The method of merging is explained below.\
Sample classes
```csharp
public class MyCustomClassA
{
    public int Age = 99;
    public string Name = "St1";
    public DateTime CurrentDateTime = DateTime.Now;
    public float Percent = 99.9f;
}

// Second sample
public class MyCustomClassB
{
    public string Age = "eight";
    public string Name = "St2";
    public DateTime CurrentDateTime = DateTime.Now;
    public string Percent = "39.9f";
}
```
Method of merging
```csharp
private static void Main()
{
     var objectA = new MyCustomClassA();
     var objectB = new MyCustomClassB();

     G9Assembly.ObjectAndReflectionTools
        // In this case, the members' values of object B are set on Object A.
        .MergeObjectsValues(objectA, objectB, 
            // Specifies the desired access modifier.
            G9EAccessModifier.Public,
            // Specifies that if two members' values don't have a shared type and can't transfer their values, the process must ignore them.
            // In this case, both objects have the same member with the name "Percent," But in the first one, it's Float, and in the second one, it's String. So, a mismatching occurs, but the core ignores it according to this setting (AllowMismatchValues).
            G9EValueMismatchChecking.AllowMismatchValues, 
            // Specifies that if a mismatch occurs between two members' values, an automatic try to change type must happen or not.
            // If it is set to 'true,' the value of "Percent" will automatically convert from String to Float.
            false, 
            // Specifies a custom filter for searching object's members if needed.
            // In this case, members with the name "CurrentDateTime" are ignored.
            member => member.Name != "CurrentDateTime",
            // Specifies a custom process for desired members if needed.
            // Notice: The function's result specifies whether the custom process handled merging or not.
            // If it's returned 'true.' Specifies that the custom process has done the merging process, and the core mustn't do anything.
            // If it's returned 'false.' Specifies that the custom process skipped the merging process, So the core must do it.
            // In the below case, the custom process just does the merging process on the member with the name "Age":
            (m1, m2) =>
            {
                // For the "Age" member
                if (m1.Name == "Age")
                {
                    switch (m2.GetValue<string>())
                    {
                        case "nine":
                            m1.SetValue(9);
                            break;
                        case "eight":
                            m1.SetValue(8);
                            break;
                        default:
                            m1.SetValue(0);
                            break;
                    }

                    // Returns true if the desired process performs the value-changing.
                    // In simple words, it's done it.
                    return true;
                }

                // Returns false if the value-changing process needs to pass to the core.
                return false;
            });

            // Result
            Console.WriteLine(objectA.Age); // 8
            Console.WriteLine(objectA.Name); // St2
            Console.WriteLine(objectA.Percent); // 99.9
}
```

## Cryptography Tools
### This small tool provides the essential methods for cryptography.
There are many hashing methods, and the most practical of them is provided here:
```csharp
private static void Main()
{
    const string testText = "Programmers never die because they are tiny gods.";
    // message-digest algorithm
    var md5 = G9Assembly.CryptographyTools
        .StringToCustomHash(G9EHashAlgorithm.MD5, testText);
    // Secure Hash Algorithm 1
    var sha1 = G9Assembly.CryptographyTools
        .StringToCustomHash(G9EHashAlgorithm.SHA1, testText);
    // Secure Hash Algorithm 256
    var sha256 = G9Assembly.CryptographyTools
        .StringToCustomHash(G9EHashAlgorithm.SHA256, testText);
    // Secure Hash Algorithm 384
    var sha384 = G9Assembly.CryptographyTools
        .StringToCustomHash(G9EHashAlgorithm.SHA384, testText);
    // Secure Hash Algorithm 512
    var sha512 = G9Assembly.CryptographyTools
        .StringToCustomHash(G9EHashAlgorithm.SHA512, testText);
    // cyclic redundancy checksum
    var crc32 = G9Assembly.CryptographyTools
        .StringToCustomHash(G9EHashAlgorithm.CRC32, testText);
}
```
And also, there are many cryptography methods, and the most practical of them (AES type) is provided here:
```csharp
private static void Main()
{
    const string testText = "Programmers never die because they are tiny gods.";
    const string standardKey = "eShVmYp3s6v9y$B&";
    const string standardIv = "gUkXp2s5v8x/A?D(";

    // AES encryption algorithm
    var aesEncryptionText =
        G9Assembly.CryptographyTools
            .AesEncryptString(
                // Specifies plain text.
                testText,
                // Specifies custom private key.
                standardKey,
                // Specifies custom initialization vector.
                standardIv,
                // Specifies the custom config for encryption and decryption.
                // It's optional; by default, the values below are set for that.
                new G9DtAESConfig(
                    paddingMode: PaddingMode.PKCS7,
                    cipherMode: CipherMode.CBC,
                    keySize: 128,
                    blockSize: 128,
                    // Specifies that if the key size isn't standard. The process must fix it or not. (With an arbitrary process)
                    enableAutoFixKeySize: false
                ));

    // Note: The only differences between the above and below methods are two things.
    // The first one is their name, and the second one is their first parameter.

    // AES encryption algorithm
    var aesDecryptionText =
        G9Assembly.CryptographyTools
            .AesDecryptString(
                // Specifies cipher text.
                aesEncryptionText,
                // Specifies custom private key.
                standardKey,
                // Specifies custom initialization vector.
                standardIv,
                // Specifies the custom config for encryption and decryption.
                // It's optional; by default, the values below are set for that.
                new G9DtAESConfig(
                    paddingMode: PaddingMode.PKCS7,
                    cipherMode: CipherMode.CBC,
                    keySize: 128,
                    blockSize: 128,
                    // Specifies that if the key size isn't standard. The process must fix it or not. (With an arbitrary process)
                    enableAutoFixKeySize: false
                ));
}
```
## Performance Tools
### A helpful tool that is more used for testing performances.
The first one is used for testing multi-thread requests. It's like a simulated area that runs a custom code block many times:
```csharp
private static void Main()
{
    var desiredNumberOfRepetitions = 999999;
    G9Assembly.PerformanceTools.MultiThreadShockTest(
        // Provides a random number.
        randomNumber =>
        {
            // Your custom code block for testing.
        },
        // Specifies desired a number of repetitions.
        desiredNumberOfRepetitions
    );
}
```
The second method compares two code blocks in terms of speed and memory usage:
```csharp
private static void Main()
{
    // First
    void StringSum1()
    {
        var basisData = string.Empty;
        basisData += "AaA";
        basisData += basisData;
        basisData += basisData;
        basisData += basisData;
        _ = string.IsNullOrEmpty(basisData);
    }

    // Second
    void StringSum2()
    {
        var basisData = string.Empty;
        basisData = basisData + "AaA";
        basisData = basisData + "AaA";
        basisData = basisData + "AaA";
        basisData = basisData + "AaA";
        _ = string.IsNullOrEmpty(basisData);
    }
    
    // Single-core and multi-core test
    var desiredNumberOfRepetitions = 999999;
     var comparativeResult = G9Assembly.PerformanceTools
        .ComparativePerformanceTester(
            // Specifies that test must perform in both states of single-core and multi-core.
            G9EPerformanceTestMode.Both,
            // Specifies desired a number of repetitions.
            desiredNumberOfRepetitions,
            // Specifies the desired code blocks for testing.
            StringSum1, StringSum2);
}
```
- Note: The output of this method has many details that are beyond the scope of this article.

## General Tools
### In simple terms, it provides useful general tools.
Provides a practical method to convert byte size to another size:
```csharp
private static void Main()
{
    const long byteSize = 1000000000;

    var kiloByte = G9Assembly.GeneralTools
        .ConvertByteSizeToAnotherSize(byteSize, G9ESizeUnits.KiloByte); 
    // 976562.5M

    var megaByte = G9Assembly.GeneralTools
        .ConvertByteSizeToAnotherSize(byteSize, G9ESizeUnits.MegaByte); 
    // 953.67431640625M
    
    var gigaByte = G9Assembly.GeneralTools
        .ConvertByteSizeToAnotherSize(byteSize, G9ESizeUnits.GigaByte);
    // 0.931322574615478515625M
    
    var teraByte = G9Assembly.GeneralTools
        .ConvertByteSizeToAnotherSize(byteSize, G9ESizeUnits.TeraByte);
    // 0.0009094947017729282379150391M
    
    var petaByte = G9Assembly.GeneralTools
        .ConvertByteSizeToAnotherSize(byteSize, G9ESizeUnits.PetaByte);
    // 0.0000008881784197001283877825M

    var exaByte = G9Assembly.GeneralTools
        .ConvertByteSizeToAnotherSize(byteSize, G9ESizeUnits.ExaByte);
    // 0.0000000008673617379884012722M

    var zettaByte = G9Assembly.GeneralTools
        .ConvertByteSizeToAnotherSize(byteSize, G9ESizeUnits.ZettaByte);
    // 0.0000000000008470329472543013M

    var yottaByte = G9Assembly.GeneralTools
        .ConvertByteSizeToAnotherSize(byteSize, G9ESizeUnits.YottaByte);
    // 0.000000000000000827180612553M
}
```
Checking a path on the computer in terms of validation can consist of many things, like validation, path existence, and drive existence. Here with a method all of them are handled:
```csharp
private static void Main()
{
    // Note: Assumes that the computer has the I drive but doesn't have the X drive.

    // Directory paths
    const string directory1 = @"I:\Project\!G9TM!\Page";
    const string directory2 = @"\Project\!G9TM!\Page";
    const string directory3 = @"I:\Project\!!!!!!!";
    const string directory4 = @"X:\Project\!G9TM!\Page";
    const string directory5 = @"\asd|asd|asd";

    // Notes about the following methods:
    // Note 1: The second parameter specifies whether the directory drive in terms of existence must be checked or not.
    // Note 2: The third parameter specifies whether the directory path in terms of existence must be checked or not.
    // Note 3: The result of the below methods is an Enum type.

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckDirectoryPathValidation(directory1, true, false));
    // Result: G9EPatchCheckResult.Correct

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckDirectoryPathValidation(directory2, true, false));
    // Result: G9EPatchCheckResult.Correct

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckDirectoryPathValidation(directory3, true, true));
    // Result: G9EPatchCheckResult.PathExistenceIsIncorrect

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckDirectoryPathValidation(directory4, true, true));
    // Result: G9EPatchCheckResult.PathDriveIsIncorrect

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckDirectoryPathValidation(directory5, true, true));
    // Result: G9EPatchCheckResult.PathNameIsIncorrect

    // File paths
    const string file1 = @"I:\Project\!G9TM!\Page\First.png";
    const string file2 = @"\Project\!G9TM!\Page\First.png";
    const string file3 = @"I:\Project\!!!!!!!\First.png";
    const string file4 = @"X:\Project\!G9TM!\Page\First.png";
    const string file5 = @"\Fi|r|st.png";
    const string file6 = @"okay.png";
    const string file7 = @"okay.p-n|g";

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckFilePathValidation(file1, false, false));
    // G9EPatchCheckResult.Correct

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckFilePathValidation(file2, true, false));
    // G9EPatchCheckResult.Correct

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckFilePathValidation(file3, true, true));
    // G9EPatchCheckResult.PathExistenceIsIncorrect

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckFilePathValidation(file4, true, true));
    // G9EPatchCheckResult.PathDriveIsIncorrect

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckFilePathValidation(file5, true, true));
    // G9EPatchCheckResult.PathNameIsIncorrect

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckFilePathValidation(file6, true, false));
    // G9EPatchCheckResult.Correct

    Console.WriteLine(G9Assembly.GeneralTools
        .CheckFilePathValidation(file7, true, true));
    // G9EPatchCheckResult.PathNameIsIncorrect

}
```