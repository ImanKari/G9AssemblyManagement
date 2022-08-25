using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using G9AssemblyManagement;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;
using G9AssemblyManagement_NUnitTest.DataType;
using G9AssemblyManagement_NUnitTest.Inherit;
using G9AssemblyManagement_NUnitTest.InstanceListener;
using G9AssemblyManagement_NUnitTest.InstanceTest;
using G9AssemblyManagement_NUnitTest.MismatchTypeTest;
using G9AssemblyManagement_NUnitTest.ObjectMembers;
using G9AssemblyManagement_NUnitTest.StaticType;
using G9AssemblyManagement_NUnitTest.Types;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace G9AssemblyManagement_NUnitTest
{
    public class G9AssemblyManagementUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Order(1)]
        public void TestGetInheritedTypesOfType()
        {
            // Test get inherited types from class type
            var typesName = new[] { nameof(G9CClassInheritTest) };
            var objectItem = new G9CTestType();
            var getInheritClassType = G9Assembly.TypeTools.GetInheritedTypesFromType(typeof(G9CTestType));
            Assert.True(getInheritClassType.Count == 1);
            Assert.True(getInheritClassType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from interface type
            typesName = new[] { nameof(G9CInterfaceInheritTest), nameof(G9DtStructInheritTest) };
            var getInheritInterfaceType = G9Assembly.TypeTools.GetInheritedTypesFromType<G9ITestType>();
            Assert.True(getInheritInterfaceType.Count == 2);
            Assert.True(getInheritInterfaceType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from abstract generic class
            typesName = new[]
                { nameof(G9CGenericAbstractClassInheritAbstractTest), nameof(G9CGenericAbstractClassTest) };
            var getInheritGenericAbstractClassType =
                G9Assembly.TypeTools.GetInheritedTypesFromType(
                    typeof(G9ATestGenericAbstractClass<,,,>));
            Assert.True(getInheritGenericAbstractClassType.Count == 2);
            Assert.True(getInheritGenericAbstractClassType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from abstract generic class (Without Ignore Interface And AbstractType)
            typesName = new[]
            {
                typeof(G9ATestGenericAbstractClassInheritAbstractClass<,,,,>).Name, nameof(G9CGenericAbstractClassTest),
                nameof(G9CGenericAbstractClassInheritAbstractTest)
            };
            var getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType =
                G9Assembly.TypeTools.GetInheritedTypesFromType(typeof(G9ATestGenericAbstractClass<,,,>),
                    false,
                    false);
            Assert.True(getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType.Count == 3);
            Assert.True(
                getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType.All(s =>
                    typesName.Contains(s.Name)));

            // Test get inherited types from generic interface (in custom assembly)
            typesName = new[] { nameof(G9CGenericInterfaceTest), nameof(G9CGenericInterfaceInheritInterfaceTest) };
            var getInheritGenericInterfaceType =
                G9Assembly.TypeTools.GetInheritedTypesFromType(typeof(G9ITestGenericInterface<,,,>),
                    true, true,
                    Assembly.GetExecutingAssembly());
            Assert.True(getInheritGenericInterfaceType.Count == 2);
            Assert.True(getInheritGenericInterfaceType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from generic interface (in custom assembly) (Without Ignore Interface And AbstractType)
            typesName = new[]
            {
                nameof(G9CGenericInterfaceTest), nameof(G9CGenericInterfaceInheritInterfaceTest),
                typeof(G9ITestGenericInterfaceInheritInterface<,,,,>).Name
            };
            var getInheritGenericInterfaceTypeWithoutIgnoreInterfaceAndAbstractType =
                G9Assembly.TypeTools.GetInheritedTypesFromType(typeof(G9ITestGenericInterface<,,,>),
                    false,
                    false,
                    Assembly.GetExecutingAssembly());
            Assert.True(getInheritGenericInterfaceTypeWithoutIgnoreInterfaceAndAbstractType.Count == 3);
            Assert.True(
                getInheritGenericInterfaceTypeWithoutIgnoreInterfaceAndAbstractType.All(s =>
                    typesName.Contains(s.Name)));
        }

        [Test]
        [Order(1)]
        public void TestEfficientTypeFeatures()
        {
            // Some built-in .NET Types
            var v1 = 1;
            var v2 = 9.9f;
            var v3 = DateTime.Now;
            var v4 = new TimeSpan(9, 9, 9);
            var v5 = IPAddress.Any;

            // Checking they are built-in .NET types
            // First way
            Assert.True(
                G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v1.GetType()) &&
                G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v2.GetType()) &&
                G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v3.GetType()) &&
                G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v4.GetType()) &&
                G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v5.GetType())
            );

            // Second way
            Assert.True(
                G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v1.GetType()) &&
                G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v2.GetType()) &&
                G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v3.GetType()) &&
                G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v4.GetType()) &&
                G9Assembly.TypeTools.IsTypeBuiltInDotNetType(v5.GetType())
            );

            // A custom object
            var cObject = new G9CClassInheritTest();
            // Checking they are built-in .NET types
            // First way
            Assert.False(G9Assembly.TypeTools.IsTypeBuiltInDotNetType(cObject.GetType()));
            // First way
            Assert.False(G9Assembly.TypeTools.IsTypeBuiltInDotNetType(cObject.GetType()));
        }

        [Test]
        [Order(1)]
        public void TestChangeTypeMethods()
        {
            var v1 = 'a';
            var v2 = "G9TM";
            var v3 = 9;
            var v4 = 9.9f;
            var v5 = new TimeSpan(9, 9, 9);
            var v6 = IPAddress.IPv6Loopback;
            var v7 = GenericParameterAttributes.NotNullableValueTypeConstraint;
            var v7b = GenericParameterAttributes.NotNullableValueTypeConstraint;
            var v8 = DateTime.Now;
            var v9 = true;


            var r1 = G9Assembly.TypeTools.SmartChangeType<string>(v1);
            var r2 = (string)G9Assembly.TypeTools.SmartChangeType(v2, typeof(string));
            var r3 = G9Assembly.TypeTools.SmartChangeType<string>(v3);
            var r4 = G9Assembly.TypeTools.SmartChangeType<string>(v4);
            var r5 = G9Assembly.TypeTools.SmartChangeType<string>(v5);
            var r6 = G9Assembly.TypeTools.SmartChangeType<string>(v6);
            var r7 = G9Assembly.TypeTools.SmartChangeType<string>(v7);
            var r7b = G9Assembly.TypeTools.SmartChangeType<string>(v7b);
            var r8 = G9Assembly.TypeTools.SmartChangeType<string>(v8);
            var r9 = G9Assembly.TypeTools.SmartChangeType<string>(v9);

            var b1 = G9Assembly.TypeTools.SmartChangeType<char>(r1);
            var b2 = (string)G9Assembly.TypeTools.SmartChangeType(r2, typeof(string));
            var b3 = G9Assembly.TypeTools.SmartChangeType<int>(r3);
            var b4 = G9Assembly.TypeTools.SmartChangeType<float>(r4);
            var b5 = G9Assembly.TypeTools.SmartChangeType<TimeSpan>(r5);
            var b6 = G9Assembly.TypeTools.SmartChangeType<IPAddress>(r6);
            var b7 = G9Assembly.TypeTools.SmartChangeType<GenericParameterAttributes>(r7);
            var b7b = G9Assembly.TypeTools.SmartChangeType<GenericParameterAttributes>(r7b);
            var b8 = G9Assembly.TypeTools.SmartChangeType<DateTime>(r8);
            var b9 = G9Assembly.TypeTools.SmartChangeType<bool>(r9);

            Assert.True(
                v1 == b1 &&
                v2 == b2 &&
                v3 == b3 &&
                v4 == b4 &&
                v5 == b5 &&
                v6.Equals(b6) &&
                v7 == b7 &&
                v7b == b7b &&
                v8.ToString("s") == b8.ToString("s") &&
                v9 == b9
            );
        }

        [Test]
        [Order(2)]
        public void TestG9AttrAddListenerOnGenerate()
        {
            // New instance of class 
            var firstTestClass = new G9CInstanceTest();

            // Get instances of type - three way
            // The first way
            var firstTestClassInstances1 =
                G9Assembly.InstanceTools.GetInstancesOfType<G9CInstanceTest>();
            Assert.True(firstTestClassInstances1.First().GetClassName() == nameof(G9CInstanceTest));
            // The second way
            var firstTestClassInstances2 = G9Assembly.InstanceTools
                .GetInstancesOfType(typeof(G9CInstanceTest))
                .Select(s => (G9CInstanceTest)s);
            Assert.True(firstTestClassInstances2.First().GetClassName() == nameof(G9CInstanceTest));


            // New instance of class
            const string firstName = "Iman";
            var firstTestStruct = new G9DtInstanceTest(firstName);

            // Get instances of type - three way
            // The first way
            var firstTestStructInstances1 =
                G9Assembly.InstanceTools.GetInstancesOfType<G9DtInstanceTest>();
            Assert.True(firstTestStructInstances1.First().GetFirstName() == firstName);
            // The second way
            var firstTestStructInstances2 = G9Assembly.InstanceTools
                .GetInstancesOfType(typeof(G9DtInstanceTest))
                .Select(s => (G9DtInstanceTest)s);
            Assert.True(firstTestStructInstances2.First().GetFirstName() == firstName);


            // New instance of custom type
            var arrayValue = new[] { "firstTest", "secondTest", "thirdTest" };
            var firstTestCustomType = new Trait(arrayValue[0], arrayValue[0]);
            var secondTestCustomType = new Trait(arrayValue[1], arrayValue[1]);
            var thirdTestCustomType = new Trait(arrayValue[2], arrayValue[2]);
            // Assign instances - Used for classes not implemented by us
            G9Assembly.InstanceTools.AssignInstanceOfType(firstTestCustomType);
            G9Assembly.InstanceTools.AssignInstanceOfType(secondTestCustomType);
            G9Assembly.InstanceTools.AssignInstanceOfType(thirdTestCustomType);
            // Get instances of custom type - three way
            // The first way
            var firstTestCustomTypeInstances1 = G9Assembly.InstanceTools.GetInstancesOfType<Trait>();
            Assert.True(firstTestCustomTypeInstances1.Count == 3);
            // The second way
            var firstTestCustomTypeInstances2 = G9Assembly.InstanceTools
                .GetInstancesOfType(typeof(Trait))
                .Select(s => (Trait)s);
            Assert.True(firstTestCustomTypeInstances2.Count() == 3);
            // Test values
            Assert.True(firstTestCustomTypeInstances1.All(s => arrayValue.Contains(s.Value)));

            // Test Unassigning
            G9Assembly.InstanceTools.UnassignInstanceOfType(thirdTestCustomType);
            G9Assembly.InstanceTools.UnassignInstanceOfType(secondTestCustomType);
            firstTestCustomTypeInstances1 = G9Assembly.InstanceTools.GetInstancesOfType<Trait>();
            Assert.True(firstTestCustomTypeInstances1.Count == 1);

            // Test automatic Unassigning (Notice: Worked just for types inherited from the abstract class "G9AClassInitializer")
            // New instance of class
            IList<G9CMultiInstanceTest> instances;
            var firstClass = new G9CMultiInstanceTest();
            var secondClass = new G9CMultiInstanceTest();
            using (var thirdClass = new G9CMultiInstanceTest())
            {
                Assert.True(thirdClass.GetClassName() == nameof(G9CMultiInstanceTest));
                instances = G9Assembly.InstanceTools.GetInstancesOfType<G9CMultiInstanceTest>();
                Assert.True(instances.Count == 3);
            }

            // Automatic unassigning after block using
            instances = G9Assembly.InstanceTools.GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 2);
            // Automatic Unassigning with dispose
            secondClass.Dispose();
            instances = G9Assembly.InstanceTools.GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 1);
            firstClass.Dispose();
            instances = G9Assembly.InstanceTools.GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 0);
            instances = G9Assembly.InstanceTools.GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 0);
        }

        [Test]
        [Order(3)]
        public void TestInstanceListener()
        {
            var exceptionMessage = "Just for test, receive it in 'On receive exception'";
            // Save count of receive new instance
            var instanceCount = 0;
            var instanceListener =
                G9Assembly.InstanceTools.AssignInstanceListener<G9CInstanceListenerTest>
                (
                    // On assign
                    newInstance =>
                    {
                        Assert.True(newInstance.GetUseCounter() == 1);
                        // ReSharper disable once AccessToModifiedClosure
                        instanceCount++;
                    },
                    // On Unassigning
                    instance =>
                    {
                        Assert.True(instance.GetUseCounter() == 2);
                        instanceCount--;
                        throw new Exception(exceptionMessage);
                    },
                    // On receive exception
                    ex =>
                    {
                        Debug.WriteLine(ex.Message);
                        Assert.True(ex.Message == exceptionMessage);
                        // Ignore
                    }
                );
            // At first is 0
            Assert.True(instanceCount == 0);
            // New instance
            var testClass1 = new G9CInstanceListenerTest();
            Assert.True(instanceCount == 1);
            // Dispose Test
            testClass1.Dispose();
            Assert.True(instanceCount == 0);
            // Initialize 3 class test
            // ReSharper disable once UnusedVariable
            var arrayClass = new object[]
            {
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest()
            };
            Assert.True(instanceCount == 3);

            // Stop listener test
            instanceListener.StopListener();
            // ReSharper disable once UnusedVariable
            var arrayClass2 = new object[]
            {
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest()
            };
            Assert.True(instanceCount == 3);

            // Resume listener test
            instanceListener.ResumeListener();
            // ReSharper disable once UnusedVariable
            var arrayClass3 = new object[]
            {
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest()
            };
            Assert.True(instanceCount == 6);

            // Dispose listener test
            instanceListener.Dispose();
            // ReSharper disable once UnusedVariable
            var arrayClass4 = new object[]
            {
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest()
            };
            Assert.True(instanceCount == 6);

            // Test dispose exception
            try
            {
                instanceListener.ResumeListener();
            }
            catch (Exception e)
            {
                Assert.True(e is ObjectDisposedException);
            }

            // Test block area
            if (true)
            {
                // Test listener -> receive all instance initialized (justListenToNewInstance = false)
                instanceCount = 0;
                // ReSharper disable once UnusedVariable
                var instanceListener2 = G9Assembly.InstanceTools
                    .AssignInstanceListener<G9CInstanceListenerTest>
                    (
                        // On assign
                        newInstance =>
                        {
                            newInstance.GetUseCounter();
                            instanceCount++;
                        }, justListenToNewInstance: false
                    );
                Assert.True(
                    G9Assembly.InstanceTools.GetInstancesOfType<G9CInstanceListenerTest>().Count ==
                    instanceCount);
            }

            // Automatic dispose listener after block area 
            GC.Collect();
        }

        [Test]
        [Order(4)]
        public void TestSafeThreadShock()
        {
            var object1 = new G9CInstanceTest();
            var object2 = new G9CThirdClass();

            var h1 = object1.GetType().GetHashCode();
            var h2 = object2.GetType().GetHashCode();

            var count1 = 0;
            var count2 = 0;

            G9Assembly.InstanceTools.AssignInstanceListener(object1.GetType(),
                assignObjectItem => { count1++; },
                unassignObject => { count1--; }, onErrorException => throw onErrorException);

            G9Assembly.InstanceTools.AssignInstanceListener(object2.GetType(),
                assignObjectItem => { count2++; },
                unassignObject => { count2--; }, onErrorException => throw onErrorException);


            G9Assembly.PerformanceTools.MultiThreadShockTest(i =>
            {
                using var newObject1 = new G9CInstanceTest();
                using var newObject2 = new G9CThirdClass();
                var hash1 = newObject1.GetType().GetHashCode();
                var hash2 = newObject2.GetType().GetHashCode();

                Assert.True(hash1 == h1);
                Assert.True(hash2 == h2);
            }, 99_999);

            Assert.True(count1 == 0 && count2 == 0);

            G9Assembly.PerformanceTools.MultiThreadShockTest(i =>
            {
                var newObject1 = new G9CInstanceTest();
                var newObject2 = new G9CThirdClass();
                var hash1 = newObject1.GetType().GetHashCode();
                var hash2 = newObject2.GetType().GetHashCode();

                Assert.True(hash1 == h1);
                Assert.True(hash2 == h2);

                newObject1.Dispose();
                newObject2.Dispose();
            }, 99_999);

            Assert.True(count1 == 0 && count2 == 0);

            G9Assembly.PerformanceTools.MultiThreadShockTest(i =>
            {
                using var newObject1 = new G9CInstanceTest();
                using var newObject2 = new G9CThirdClass();
                var hash1 = newObject1.GetType().GetHashCode();
                var hash2 = newObject2.GetType().GetHashCode();

                Assert.True(hash1 == h1);
                Assert.True(hash2 == h2);

                newObject1.Dispose();
                newObject2.Dispose();
            }, 99_999);

            Assert.True(count1 == 0 && count2 == 0);
        }

        [Test]
        [Order(5)]
        public void TestGetFieldsOfObject()
        {
            // Create objects from class and struct
            var object1 = new G9CObjectMembersTest();
            var object2 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);
            var object3 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);
            var object4 = new G9CObjectMembersTest();
            var object5 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);

            // Get fields
            var fieldsOfObject1 =
                G9Assembly.ObjectAndReflectionTools.GetFieldsOfObject(object1, G9EAccessModifier.Public);
            var fieldsOfObject2 =
                G9Assembly.ObjectAndReflectionTools.GetFieldsOfObject(object2, G9EAccessModifier.NonPublic);
            var fieldsOfObject3 =
                G9Assembly.ObjectAndReflectionTools.GetFieldsOfObject(object3,
                    G9EAccessModifier.Static | G9EAccessModifier.Public);
            var fieldsOfObject4 =
                G9Assembly.ObjectAndReflectionTools.GetFieldsOfObject(object4,
                    G9EAccessModifier.Static | G9EAccessModifier.NonPublic);
            var fieldsOfObject5 = G9Assembly.ObjectAndReflectionTools.GetFieldsOfObject(object5);

            // It has one public field
            Assert.True(fieldsOfObject1.Count == 1);

            // It has three non public fields
            // One field is defined on the body, and two fields are defined as a back field
            Assert.True(fieldsOfObject2.Count == 3);

            // Get Public/Private/static fields
            fieldsOfObject1 = G9Assembly.ObjectAndReflectionTools.GetFieldsOfObject(object1);
            fieldsOfObject2 = G9Assembly.ObjectAndReflectionTools.GetFieldsOfObject(object2);

            // They have two public fields, two private fields and two static field (Two private fields are automated fields(Backing Field) for decimal properties)
            Assert.True(fieldsOfObject1.Count == 6 && fieldsOfObject2.Count == 6);

            // Get and test value from fields
            Assert.True(fieldsOfObject1[0].Name == nameof(G9CObjectMembersTest.StringTest1) &&
                        (string)fieldsOfObject1[0].GetValue() == "A" &&
                        fieldsOfObject1[0].GetValue<string>() == "A");
            Assert.True(fieldsOfObject1[1].Name == "StringTest2" &&
                        (string)fieldsOfObject1[1].GetValue() == "B" &&
                        fieldsOfObject1[1].GetValue<string>() == "B");
            Assert.True(fieldsOfObject2[2].Name.Contains("BackingField") &&
                        (decimal)fieldsOfObject2[2].GetValue() == (decimal)999.999 &&
                        fieldsOfObject2[2].GetValue<decimal>() == (decimal)999.999);
            Assert.True(fieldsOfObject2[3].Name.Contains("BackingField") &&
                        (decimal)fieldsOfObject2[3].GetValue() == (decimal)369.963 &&
                        fieldsOfObject2[3].GetValue<decimal>() == (decimal)369.963);

            // Bad type test (InvalidCastException)
            try
            {
                _ = fieldsOfObject2[3].GetValue<int>();
            }
            catch (Exception ex)
            {
                // Unable to cast object of type 'System.Decimal' to type 'System.Int32'.
                Assert.True(ex is InvalidCastException);
            }

            // Set and test value for fields
            fieldsOfObject1[0].SetValue("Iman Kari");
            Assert.True(fieldsOfObject1[0].Name == nameof(G9CObjectMembersTest.StringTest1) &&
                        (string)fieldsOfObject1[0].GetValue() == "Iman Kari" &&
                        fieldsOfObject1[0].GetValue<string>() == "Iman Kari");

            fieldsOfObject2[3].SetValue((decimal)999.999);
            Assert.True(fieldsOfObject2[3].Name.Contains("BackingField") &&
                        (decimal)fieldsOfObject2[3].GetValue() == (decimal)999.999 &&
                        fieldsOfObject2[3].GetValue<decimal>() == (decimal)999.999);

            // Bad type test (ArgumentException)
            try
            {
                fieldsOfObject2[3].SetValue("Oops!");
            }
            catch (Exception ex)
            {
                // Object of type 'System.String' cannot be converted to type 'System.Decimal'.
                Assert.True(ex is ArgumentException);
            }

            // Set static field value
            fieldsOfObject2[5].SetValue(IPAddress.Parse("192.168.1.1"));
            Assert.True(fieldsOfObject2[5].GetValue<IPAddress>().Equals(IPAddress.Parse("192.168.1.1")));

            // It has one static public field
            Assert.True(fieldsOfObject3.Count == 1 && fieldsOfObject3[0].Name == "StaticIpAddressTest1" &&
                        fieldsOfObject3[0].GetValue<IPAddress>().Equals(IPAddress.Any));

            // It has one private static public field (BackingField)
            Assert.True(fieldsOfObject4.Count == 1 &&
                        fieldsOfObject4[0].Name == "<StaticIpAddressTest2>k__BackingField" &&
                        fieldsOfObject4[0].GetValue<IPAddress>().Equals(IPAddress.None));

            // Access to all fields
            // All fields (4) + Backing Fields (2) are 6
            Assert.True(fieldsOfObject5.Count == 6);

            // Test with custom filter
            // Three fields have "1" in their name
            fieldsOfObject5 = G9Assembly.ObjectAndReflectionTools.GetFieldsOfObject(object5,
                G9EAccessModifier.Everything, s => s.Name.Contains("1"));
            Assert.True(fieldsOfObject5.Count == 3);
        }

        [Test]
        [Order(6)]
        public void TestGetPropertiesOfObject()
        {
            // Create objects from class and struct
            var object1 = new G9CObjectMembersTest();
            var object2 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);
            var object3 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);
            var object4 = new G9CObjectMembersTest();
            var object5 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);

            // Get properties
            var fieldsOfObject1 =
                G9Assembly.ObjectAndReflectionTools.GetPropertiesOfObject(object1, G9EAccessModifier.Public);
            var fieldsOfObject2 =
                G9Assembly.ObjectAndReflectionTools.GetPropertiesOfObject(object2, G9EAccessModifier.NonPublic);
            var fieldsOfObject3 =
                G9Assembly.ObjectAndReflectionTools.GetPropertiesOfObject(object3,
                    G9EAccessModifier.Static | G9EAccessModifier.Public);
            var fieldsOfObject4 =
                G9Assembly.ObjectAndReflectionTools.GetPropertiesOfObject(object4,
                    G9EAccessModifier.Static | G9EAccessModifier.NonPublic);
            var fieldsOfObject5 = G9Assembly.ObjectAndReflectionTools.GetPropertiesOfObject(object5);

            // It has one public property
            Assert.True(fieldsOfObject1.Count == 1);

            // It has one non public property
            Assert.True(fieldsOfObject2.Count == 1);

            // Get Public/Private/static properties
            fieldsOfObject1 = G9Assembly.ObjectAndReflectionTools.GetPropertiesOfObject(object1);
            fieldsOfObject2 = G9Assembly.ObjectAndReflectionTools.GetPropertiesOfObject(object2);

            // They have one public property + one private property + one private static property
            Assert.True(fieldsOfObject1.Count == 3 && fieldsOfObject2.Count == 3);

            // Get and test value from fields
            Assert.True(fieldsOfObject1[0].Name == nameof(G9CObjectMembersTest.DecimalTest1) &&
                        (decimal)fieldsOfObject1[0].GetValue() == 999.999m &&
                        fieldsOfObject1[0].GetValue<decimal>() == 999.999m);
            Assert.True(fieldsOfObject1[1].Name == "DecimalTest2" &&
                        (decimal)fieldsOfObject1[1].GetValue() == 369.963m &&
                        fieldsOfObject1[1].GetValue<decimal>() == 369.963m);
            Assert.True(fieldsOfObject2[2].Name == "StaticIpAddressTest2" &&
                        Equals((IPAddress)fieldsOfObject2[2].GetValue(), IPAddress.None) &&
                        Equals(fieldsOfObject2[2].GetValue<IPAddress>(), IPAddress.None));

            // Bad type test (InvalidCastException)
            try
            {
                _ = fieldsOfObject2[2].GetValue<int>();
            }
            catch (Exception ex)
            {
                // Unable to cast object of type 'System.Decimal' to type 'System.Int32'.
                Assert.True(ex is InvalidCastException);
            }

            // Set and test value for properties
            fieldsOfObject1[0].SetValue(639.963m);
            Assert.True(fieldsOfObject1[0].GetValue<decimal>() == 639.963m);

            fieldsOfObject2[2].SetValue(IPAddress.Broadcast);
            Assert.True(fieldsOfObject2[2].GetValue<IPAddress>().Equals(IPAddress.Broadcast));

            // It don't have public static property
            Assert.True(fieldsOfObject3.Count == 0);

            // It has one private public static property
            Assert.True(fieldsOfObject4.Count == 1 &&
                        fieldsOfObject4[0].Name == "StaticIpAddressTest2" &&
                        fieldsOfObject4[0].GetValue<IPAddress>().Equals(IPAddress.None));

            // Access to all properties
            // it has one public property + one private property + one private static property
            Assert.True(fieldsOfObject5.Count == 3);

            // Test custom filter
            // Just one property has "1" in its name
            fieldsOfObject5 = G9Assembly.ObjectAndReflectionTools.GetPropertiesOfObject(object5,
                G9EAccessModifier.Everything, s => s.Name.Contains("1"));
            Assert.True(fieldsOfObject5.Count == 1);
        }

        [Test]
        [Order(7)]
        public void TestGetMethodsOfObject()
        {
            // Create objects from class and struct
            var object1 = new G9CObjectMembersTest();
            var object2 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);
            var object3 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);
            var object4 = new G9CObjectMembersTest();

            // Get Methods
            var fieldsOfObject1 =
                G9Assembly.ObjectAndReflectionTools.GetMethodsOfObject(object1, G9EAccessModifier.Public);
            var fieldsOfObject2 =
                G9Assembly.ObjectAndReflectionTools.GetMethodsOfObject(object2, G9EAccessModifier.NonPublic);
            var fieldsOfObject3 =
                G9Assembly.ObjectAndReflectionTools.GetMethodsOfObject(object3,
                    G9EAccessModifier.Static | G9EAccessModifier.Public);
            var fieldsOfObject4 =
                G9Assembly.ObjectAndReflectionTools.GetMethodsOfObject(object4,
                    G9EAccessModifier.Static | G9EAccessModifier.NonPublic);

            // Notice
            // 1- Each object has some built-in public methods (GetType() - ToString() - Equals() - GetHashCode())
            // 2- Each object has some built-in private method (MemberwiseClone() - Finalize())
            // 3- Each object with the property can have auto-made methods for the "set" or "get" processes of properties.

            // It has one public method + 4 built-in public methods + 2 auto-made property public methods
            Assert.True(fieldsOfObject1.Count == 7);

            // It has one non public method (internal) + 2 built-in private methods + one auto-made property method (just has getter)
            Assert.True(fieldsOfObject2.Count == 4);

            // Get Public/Private/static methods
            fieldsOfObject1 = G9Assembly.ObjectAndReflectionTools.GetMethodsOfObject(object1);
            fieldsOfObject2 = G9Assembly.ObjectAndReflectionTools.GetMethodsOfObject(object2);

            // They have 13 methods in total (public method + private methods + (internal/protected) methods + public static methods + built-in methods)
            Assert.True(fieldsOfObject1.Count == 13 && fieldsOfObject2.Count == 13);

            // Get and test value from methods
            Assert.True(fieldsOfObject1[5].MethodName == nameof(G9CObjectMembersTest.TestMethod1) &&
                        fieldsOfObject1[5].CallMethodWithResult<int>(6, 3) == 9);

            // Bad argument type test (ArgumentException)
            try
            {
                _ = fieldsOfObject1[5].CallMethodWithResult<int>("6", 3);
            }
            catch (Exception ex)
            {
                // Object of type 'System.String' cannot be converted to type 'System.Int32'.
                Assert.True(ex is ArgumentException);
            }

            // Mismatch argument count test (TargetParameterCountException)
            try
            {
                _ = fieldsOfObject1[5].CallMethodWithResult<int>(3);
            }
            catch (Exception ex)
            {
                // Parameter count mismatch.
                Assert.True(ex is TargetParameterCountException);
            }

            // It don't have public static method
            Assert.True(fieldsOfObject3.Count == 0);

            // It have just two private static (Backing Property setter/getter) method
            Assert.True(fieldsOfObject4.Count == 2);

            // Test custom filter
            // Three methods have "1" in their names (One normal method + 2 backing method for decimal properties)
            fieldsOfObject1 = G9Assembly.ObjectAndReflectionTools.GetMethodsOfObject(object1,
                G9EAccessModifier.Everything, s => s.Name.Contains("1"));
            Assert.True(fieldsOfObject1.Count == 3);
        }

        [Test]
        [Order(8)]
        public void TestGetGenericMethodsOfObject()
        {
            // Create objects from class and struct
            var object1 = new G9CObjectMembersTest();
            var object2 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);
            var object3 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);
            var object4 = new G9CObjectMembersTest();

            // Get Methods
            var fieldsOfObject1 =
                G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfObject(object1, G9EAccessModifier.Public);
            var fieldsOfObject2 =
                G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfObject(object2,
                    G9EAccessModifier.NonPublic);
            var fieldsOfObject3 =
                G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfObject(object3,
                    G9EAccessModifier.Static | G9EAccessModifier.Public);
            var fieldsOfObject4 =
                G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfObject(object4,
                    G9EAccessModifier.Static | G9EAccessModifier.NonPublic);

            // It don't have a public generic method
            Assert.True(fieldsOfObject1.Count == 0);

            // It has one non public generic method (private)
            Assert.True(fieldsOfObject2.Count == 1);

            // Get Public/Private/static methods
            fieldsOfObject1 = G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfObject(object1);
            fieldsOfObject2 = G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfObject(object2);

            // They have one public static generic method as well as one private generic method
            Assert.True(fieldsOfObject1.Count == 2 && fieldsOfObject2.Count == 2);

            // Get and test value from methods
            Assert.True(fieldsOfObject1[0].MethodName == nameof(G9CObjectMembersTest.TestGenericMethod) &&
                        fieldsOfObject1[0]
                            .CallMethodWithResult<G9DtObjectMembersTest>(new[] { typeof(G9DtObjectMembersTest) })
                            .Equals(
                                default(G9DtObjectMembersTest)));

            Assert.True(fieldsOfObject1[1].MethodName == nameof(G9CObjectMembersTest.TestGenericMethod) &&
                        fieldsOfObject1[1].CallMethodWithResult<(int, string, IPAddress)>(
                            new[] { typeof(int), typeof(string), typeof(IPAddress) }, 1, "Iman",
                            IPAddress.Broadcast) ==
                        (1, "Iman", IPAddress.Broadcast));

            // Bad result type test (ArgumentException)
            try
            {
                _ = fieldsOfObject1[0]
                    .CallMethodWithResult<G9DtObjectMembersTest>(new[] { typeof(string) });
            }
            catch (Exception ex)
            {
                // The specified type for the result is incorrect. The specified result type is: 'G9AssemblyManagement_NUnitTest.ObjectMembers.G9DtObjectMembersTest'.
                Assert.True(ex is ArgumentException);
            }

            // Mismatch argument count test (ArgumentException)
            try
            {
                _ = fieldsOfObject1[1]
                    .CallMethodWithResult<(int, string)>(new[] { typeof(int), typeof(string) }, 1, "Iman");
            }
            catch (Exception ex)
            {
                // The type or method has 3 generic parameter(s), but 2 generic argument(s) were provided. A generic argument must be provided for each generic parameter.
                Assert.True(ex is ArgumentException);
            }

            // It has one public static generic method
            Assert.True(fieldsOfObject3.Count == 1);

            // It don't have a private static generic method
            Assert.True(fieldsOfObject4.Count == 0);

            // Test custom filter
            // Just one generic method has three argumments
            fieldsOfObject2 = G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfObject(object2,
                G9EAccessModifier.Everything,
                s => s.GetGenericArguments().Length >= 3);
            Assert.True(fieldsOfObject2.Count == 1);
        }

        [Test]
        [Order(9)]
        public void TestGetAllMembersOfObject()
        {
            // Create objects from class and struct
            var object1 = new G9CObjectMembersTest();
            var object2 = new G9DtObjectMembersTest("A", "B", (decimal)999.999, (decimal)369.963, IPAddress.Any,
                IPAddress.None);

            // Get Methods
            var fieldsOfObject1 = G9Assembly.ObjectAndReflectionTools.GetAllMembersOfObject(object1);
            var fieldsOfObject2 = G9Assembly.ObjectAndReflectionTools.GetAllMembersOfObject(object2);

            // Test count of members
            Assert.True(fieldsOfObject1.Fields.Count == 6 && fieldsOfObject1.Properties.Count == 3 &&
                        fieldsOfObject1.Methods.Count == 13 && fieldsOfObject1.GenericMethods.Count == 2);
            Assert.True(fieldsOfObject2.Fields.Count == 6 && fieldsOfObject2.Properties.Count == 3 &&
                        fieldsOfObject2.Methods.Count == 13 && fieldsOfObject2.GenericMethods.Count == 2);
        }

        [Test]
        [Order(10)]
        public void TestGetMembersFromStaticObject()
        {
            #region Test static member

            // Get fields of type with initialize for accessing to non static members
            var fields = G9Assembly.ObjectAndReflectionTools.GetFieldsOfType(typeof(G9DtStaticMemberType),
                G9EAccessModifier.Everything, null, true);
            // Total fields
            Assert.True(fields.Count == 4);
            // Test static field
            Assert.True(
                fields.First(s => s.Name == nameof(G9DtStaticMemberType.Name))
                    .GetValue<string>() == G9DtStaticMemberType.Name);
            // Test non static field
            Assert.True(
                fields.First(s => s.Name == nameof(G9DtStaticMemberType.Name2))
                    .GetValue<string>() == "G9TM2");

            // Get properties of type with initialize for accessing to non static members
            var properties = G9Assembly.ObjectAndReflectionTools.GetPropertiesOfType(typeof(G9DtStaticMemberType),
                G9EAccessModifier.Everything, null, true);
            // Total properties
            Assert.True(properties.Count == 2);
            // Test static property
            Assert.True(
                properties.First(s => s.Name == nameof(G9DtStaticMemberType.Age))
                    .GetValue<int>() == G9DtStaticMemberType.Age);
            // Test non static property
            Assert.True(
                properties.First(s => s.Name == nameof(G9DtStaticMemberType.Age2))
                    .GetValue<int>() == 99);


            // Get methods of type with initialize for accessing to non static members
            var methods = G9Assembly.ObjectAndReflectionTools.GetMethodsOfType(typeof(G9DtStaticMemberType),
                G9EAccessModifier.Everything, null, true);
            // Total methods
            Assert.True(methods.Count == 12);
            // Test static method
            Assert.True(
                methods.First(s => s.MethodName == nameof(G9DtStaticMemberType.GetNameAsHashCode))
                    .CallMethodWithResult<int>() == G9DtStaticMemberType.Name.GetHashCode());
            // Test non static method
            Assert.True(
                methods.First(s => s.MethodName == nameof(G9DtStaticMemberType.GetName))
                    .CallMethodWithResult<string>() == G9DtStaticMemberType.Name);

            // Get generic methods of type with initialize for accessing to non static members
            var genericMethods = G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfType(
                typeof(G9DtStaticMemberType),
                G9EAccessModifier.Everything, null, true);
            // Total generic methods
            Assert.True(genericMethods.Count == 2);

            // Test static generic method
            Assert.True(
                genericMethods.First(s => s.MethodName == nameof(G9DtStaticMemberType.TestStaticGeneric))
                    .CallMethodWithResult<string>(new[] { typeof(string) }, "G9TM") == "G9TM");
            // Test non static generic method
            Assert.True(
                genericMethods.First(s => s.MethodName == nameof(G9DtStaticMemberType.TestStaticGeneric2))
                    .CallMethodWithResult<string>(new[] { typeof(string) }, "!G9TM!") == "!G9TM!");

            #endregion

            #region Test static class

            // A static class can't have an instance!
            try
            {
                G9Assembly.ObjectAndReflectionTools.GetFieldsOfType(typeof(G9DtStaticType),
                    G9EAccessModifier.Everything, null, true);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.True(ex is MemberAccessException && ex.Message == "Cannot create an abstract class.");
            }

            // Get fields of type
            var staticFields = G9Assembly.ObjectAndReflectionTools.GetFieldsOfType(typeof(G9DtStaticType),
                G9EAccessModifier.Everything);
            // Total fields
            Assert.True(staticFields.Count == 2);
            // Test static field
            Assert.True(
                staticFields.First(s => s.Name == nameof(G9DtStaticType.Name))
                    .GetValue<string>() == G9DtStaticType.Name);

            // Get properties of type
            var staticProperties = G9Assembly.ObjectAndReflectionTools.GetPropertiesOfType(typeof(G9DtStaticType),
                G9EAccessModifier.Everything);
            // Total properties
            Assert.True(staticProperties.Count == 1);
            // Test static property
            Assert.True(
                staticProperties.First(s => s.Name == nameof(G9DtStaticType.Age))
                    .GetValue<int>() == G9DtStaticType.Age);


            // Get methods of type
            var staticMethods = G9Assembly.ObjectAndReflectionTools.GetMethodsOfType(typeof(G9DtStaticType),
                G9EAccessModifier.Everything);
            // Total methods
            Assert.True(staticMethods.Count == 9);
            // Test static method
            Assert.True(
                staticMethods.First(s => s.MethodName == nameof(G9DtStaticType.GetNameAsHashCode))
                    .CallMethodWithResult<int>() == G9DtStaticType.Name.GetHashCode());

            // Get generic methods of type
            var staticGenericMethods = G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfType(
                typeof(G9DtStaticType),
                G9EAccessModifier.Everything);
            // Total generic methods
            Assert.True(staticGenericMethods.Count == 1);

            // Test static generic method
            Assert.True(
                staticGenericMethods.First(s => s.MethodName == nameof(G9DtStaticType.TestStaticGeneric))
                    .CallMethodWithResult<string>(new[] { typeof(string) }, "G9TM") == "G9TM");

            G9Assembly.PerformanceTools.MultiThreadShockTest(i =>
            {
                // Get generic methods of type
                var genericMethodsList = G9Assembly.ObjectAndReflectionTools.GetGenericMethodsOfType(
                    typeof(G9DtStaticType),
                    G9EAccessModifier.Everything);

                // Total generic methods
                Assert.True(genericMethodsList.Count == 1);

                // Test static generic method
                Assert.True(
                    genericMethodsList.First(s => s.MethodName == nameof(G9DtStaticType.TestStaticGeneric))
                        .CallMethodWithResult<string>(new[] { typeof(string) }, $"G9TM{i}") == $"G9TM{i}");
            }, 99_999);

            #endregion
        }

        [Test]
        [Order(11)]
        public void TestCreateInstanceFromType()
        {
            // Create instance from a normal type
            var testObject1 = G9Assembly.InstanceTools.CreateInstanceFromType<G9DtNormalType>();
            testObject1.Name = "Okay";
            Assert.True(testObject1.Name == "Okay");

            // Create instance from a generic type
            var testObject2 =
                (G9DtGenericType<string>)G9Assembly.InstanceTools.CreateInstanceFromType(
                    typeof(G9DtGenericType<string>));
            testObject2.ValueOfType = "G9TM";
            Assert.True(testObject2.ValueOfType == "G9TM");

            // Create instance from a normal type with constructor
            var testObject3 =
                G9Assembly.InstanceTools
                    .CreateInstanceFromTypeWithParameters<G9DtNormalTypeByConstructor>("Hello G9TM");
            Assert.True(testObject3.Name == "Hello G9TM");
            testObject3 =
                (G9DtNormalTypeByConstructor)
                G9Assembly.InstanceTools.CreateInstanceFromTypeWithParameters(
                    typeof(G9DtNormalTypeByConstructor), "Hello G9TM");
            Assert.True(testObject3.Name == "Hello G9TM");

            // Create instance from a generic type with constructor
            var testObject4 = G9Assembly.InstanceTools
                .CreateInstanceFromTypeWithParameters<G9DtGenericTypeByConstructor<string, int, IPAddress>>("G9TM",
                    999, IPAddress.IPv6None);
            Assert.True(testObject4.ObjectType1 == "G9TM" && testObject4.ObjectType2 == 999 &&
                        Equals(testObject4.ObjectType3, IPAddress.IPv6None));
            testObject4 =
                (G9DtGenericTypeByConstructor<string, int, IPAddress>)G9Assembly.InstanceTools
                    .CreateInstanceFromTypeWithParameters(typeof(G9DtGenericTypeByConstructor<string, int, IPAddress>),
                        "G9TM",
                        999, IPAddress.IPv6None);
            Assert.True(testObject4.ObjectType1 == "G9TM" && testObject4.ObjectType2 == 999 &&
                        Equals(testObject4.ObjectType3, IPAddress.IPv6None));

            // Create uninitialized instance from types
            var testObject5A = G9Assembly.InstanceTools.CreateUninitializedInstanceFromType<IPAddress>();
            var testObject5B =
                G9Assembly.InstanceTools.CreateUninitializedInstanceFromType(typeof(IPAddress));
            Assert.AreEqual(testObject5A, testObject5B);
            // Testing initialize "IPAddress" type by the default method
            try
            {
                G9Assembly.InstanceTools.CreateInstanceFromType(typeof(IPAddress));
                Assert.Fail();
            }
            catch (Exception e)
            {
                // No parameterless constructor defined for type 'System.Net.IPAddress'.
                Assert.True(e is MissingMethodException);
            }
        }

        [Test]
        [Order(12)]
        public void TestMergeObjectsValues()
        {
            // Defining three different objects.
            var objectA = new G9CMismatchTypeA();
            var objectB = new G9CMismatchTypeB();
            var objectC = new G9CMismatchTypeC();
            var objectC2 = new G9CMismatchTypeC2();

            // The first test just recognizes the default value.
            Assert.True(objectA.Name == "G9TM" && objectA.Age == 32 &&
                        objectA.ExDateTime == DateTime.Parse("1990-09-01") &&
                        objectA.Name != objectB.Name && objectA.Age != objectB.Age &&
                        objectA.ExDateTime != objectB.ExDateTime);

            // Merging object objectA with object objectB
            G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectA, objectB);

            // Testing the values between objects after merging.
            Assert.True(objectA.Name != "G9TM" && objectA.Age != 32 &&
                        objectA.ExDateTime != DateTime.Parse("1990-09-01") &&
                        objectA.Name == objectB.Name && objectA.Age == objectB.Age &&
                        objectA.ExDateTime == objectB.ExDateTime);

            // Defining new value for the object again.
            objectA = new G9CMismatchTypeA();
            G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectA, objectB);

            // Testing the values between objects after merging again.
            Assert.True(objectA.Name != "G9TM" && objectA.Age != 32 &&
                        objectA.ExDateTime != DateTime.Parse("1990-09-01") &&
                        objectA.Name == objectB.Name && objectA.Age == objectB.Age &&
                        objectA.ExDateTime == objectB.ExDateTime);


            // Merging objectB with objectC
            try
            {
                /*
                By paying attention to mode "enableTryToChangeType" that is set "false":
                    In this case, the objectC has the value "nine" for age; it definitely can't be changed to the int type.
                */
                G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectB, objectC,
                    G9EValueMismatchChecking.PreventMismatchValues);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.True(e.Message == @"The members can't merge their values.
In the first object, the member's name is 'Age' with the type 'System.Int32'.
In the second object, the member's name is 'Age' with the value '109' and the type 'System.String'." &&
                            e.InnerException.GetType() == typeof(ArgumentException) && e.InnerException.Message ==
                            "Object of type 'System.String' cannot be converted to type 'System.Int32'.");
            }

            // Merging objectB with objectC by "AllowMismatchValues" mode.
            G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectB, objectC);

            // Testing the values between objects after merging.
            /*
             * By paying attention to the "G9EValueMismatchChecking.AllowMismatchValues" mode:
             *   The merging process ignores the "Age" member because of mismatching in type.
             *   The merging process ignores the "IpAddress" member because of mismatching in type.
             *   The merging process ignores the "Percent" member because of mismatching in type.
            */
            Assert.True(objectB.Name != "G9TM 2" && objectB.Age == 99 &&
                        objectB.ExDateTime != DateTime.Parse("1999-03-06") &&
                        objectB.IpAddress == IPAddress.IPv6Loopback && objectB.Percent == 99.9f &&
                        objectB.Name == objectC.Name && objectB.Age.ToString() != objectC.Age &&
                        objectB.ExDateTime == objectC.ExDateTime &&
                        objectB.IpAddress.ToString() != objectC.IpAddress &&
                        objectB.Percent.ToString() != objectC.Percent);

            // Defining new value for the object again.
            objectB = new G9CMismatchTypeB();

            // Merging objectB with objectC by trying smart change type.
            G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectB, objectC,
                G9EValueMismatchChecking.PreventMismatchValues, true);

            // Testing the values between objects after merging.
            /*
             * By paying attention to the "G9EValueMismatchChecking.PreventMismatchValues" mode as well as "enableTryToChangeType":
             *   All mismatches are solved with the automatic smart change type process.
            */
            // Testing the values between objects after merging.
            Assert.True(objectB.Name != "G9TM 2" && objectB.Age != 99 &&
                        objectB.ExDateTime != DateTime.Parse("1999-03-06") &&
                        objectB.IpAddress != IPAddress.IPv6Loopback && objectB.Percent != 99.9f &&
                        objectB.Name == objectC.Name && objectB.Age.ToString() == objectC.Age &&
                        objectB.ExDateTime == objectC.ExDateTime &&
                        objectB.IpAddress.ToString() == objectC.IpAddress &&
                        objectB.Percent.ToString() == objectC.Percent);

            // Defining new value for the object again.
            objectB = new G9CMismatchTypeB();
            try
            {
                /*
                 * Merging objectB with objectC by trying smart change type.
                 * In this case, the objectC has the value "nine" for age; it definitely can't be changed to the int type.
                 */
                G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectB, objectC2,
                    G9EValueMismatchChecking.PreventMismatchValues, true);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.True(e.Message == @"The members can't merge their values.
In the first object, the member's name is 'Age' with the type 'System.Int32'.
In the second object, the member's name is 'Age' with the value 'nine' and the type 'System.String'." &&
                            e.InnerException.GetType() == typeof(FormatException) && e.InnerException.Message ==
                            "Input string was not in a correct format.");
            }


            // Merging objectB with objectC by "AllowMismatchValues" mode.
            G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectB, objectC2,
                G9EValueMismatchChecking.AllowMismatchValues, true);

            // Testing the values between objects after merging.
            /*
             * By paying attention to the "G9EValueMismatchChecking.AllowMismatchValues" mode:
             *   The merging process ignores the "Age" member because of mismatching in type.
            */
            Assert.True(objectB.Age == 99 && objectB.Age.ToString() != objectC2.Age &&
                        objectB.Percent != 99.9f && objectB.Percent.ToString() == objectC2.Percent);

            // Defining new value for the object agains.
            objectA = new G9CMismatchTypeA();
            objectB = new G9CMismatchTypeB();

            // Merging objectA with objectB just for all public members.
            G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectA, objectB,
                G9EValueMismatchChecking.PreventMismatchValues);

            // The first test just recognizes the default value.
            Assert.True(objectA.GetTime() == new TimeSpan(9, 9, 9) && objectB.GetTime() == new TimeSpan(3, 6, 9) &&
                        objectA.GetTime() != objectB.GetTime());

            // Merging objectA with objectB for all members (private/protect/public/...).
            G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectA, objectB,
                G9EValueMismatchChecking.PreventMismatchValues, false, G9EAccessModifier.Everything);

            // Testing private member
            Assert.True(objectA.GetTime() != new TimeSpan(9, 9, 9) && objectB.GetTime() == new TimeSpan(3, 6, 9) &&
                        objectA.GetTime() == objectB.GetTime());

            // Defining new value for the object agains.
            objectA = new G9CMismatchTypeA();
            objectB = new G9CMismatchTypeB();

            // Ignore a member with custom filter
            G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectA, objectB,
                G9EValueMismatchChecking.PreventMismatchValues, false, G9EAccessModifier.Everything,
                s => s.Name != "Age");

            Assert.True(objectA.Age != objectB.Age && objectA.ExDateTime == objectB.ExDateTime);

            // Defining new value for the object again.
            objectB = new G9CMismatchTypeB();
            /*
                 * Merging objectB with objectC by trying smart change type.
                 * In this case, the object has the value of "nine" for the "age" member; it definitely can't be changed to the int type, but with the custom process it can be done!
                 */
            Assert.True(objectB.Age == 99 && objectC2.Age == "nine");
            G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(objectB, objectC2,
                G9EValueMismatchChecking.PreventMismatchValues, true, G9EAccessModifier.Public, null,
                (m1, m2) =>
                {
                    // For the "Age" member
                    if (m1.Name == "Age")
                        return m2.GetValue<string>() switch
                        {
                            "nine" => 9,
                            "eight" => 8,
                            // .....
                            _ => 0
                        };

                    // For the other members
                    return m2.GetValue();
                });

            Assert.True(objectB.Age == 9);

            // Thread shock test
            G9Assembly.PerformanceTools.MultiThreadShockTest(i =>
            {
                string stringNumber;
                int intNumber;
                if (i % 2 == 0)
                {
                    stringNumber = "nine";
                    intNumber = 9;
                }
                else
                {
                    stringNumber = "eight";
                    intNumber = 8;
                }

                var newObjectB = new G9CMismatchTypeB();
                var newObjectC2 = new G9CMismatchTypeC2
                {
                    Age = stringNumber
                };

                Assert.True(newObjectB.Age == 99 && newObjectC2.Age == stringNumber);
                G9Assembly.ObjectAndReflectionTools.MergeObjectsValues(newObjectB, newObjectC2,
                    G9EValueMismatchChecking.PreventMismatchValues, true, G9EAccessModifier.Public, null,
                    (m1, m2) =>
                    {
                        // For the "Age" member
                        if (m1.Name == "Age")
                            return m2.GetValue<string>() switch
                            {
                                "nine" => 9,
                                "eight" => 8,
                                // .....
                                _ => 0
                            };

                        // For the other members
                        return m2.GetValue();
                    });

                Assert.True(newObjectB.Age == intNumber);
            }, 99_999);
        }

        [Test]
        [Order(13)]
        public void TestPerformanceMethods()
        {
            // Number Of Repetitions
            const int numberOfRepetitions = 39_999_999;

            // Just Test
            G9Assembly.PerformanceTools.MultiThreadShockTest(i =>
            {
                // Nothing/Just test
            }, numberOfRepetitions);

            // Two sample function for test
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

            // Single-core test
            var result1 = G9Assembly.PerformanceTools.PerformanceTester(StringSum1,
                G9EPerformanceTestMode.SingleCore, numberOfRepetitions);

            Assert.True(result1.AverageExecutionTimeOnMultiCore == null &&
                        result1.AverageExecutionTimeOnSingleCore != null &&
                        result1.TestMode == G9EPerformanceTestMode.SingleCore &&
                        result1.NumberOfRepetitions == numberOfRepetitions);

            Thread.Sleep(99);

            // Multi-core test
            var result2 = G9Assembly.PerformanceTools.PerformanceTester(StringSum1,
                G9EPerformanceTestMode.MultiCore, numberOfRepetitions);
            Assert.True(result2.AverageExecutionTimeOnMultiCore != null &&
                        result2.AverageExecutionTimeOnSingleCore == null &&
                        result2.TestMode == G9EPerformanceTestMode.MultiCore &&
                        result2.NumberOfRepetitions == numberOfRepetitions);

            Thread.Sleep(99);

            // Single-core and multi-core test
            var result3 = G9Assembly.PerformanceTools.PerformanceTester(StringSum1, G9EPerformanceTestMode.Both,
                numberOfRepetitions);

            Assert.True(result3.AverageExecutionTimeOnMultiCore != null &&
                        result3.AverageExecutionTimeOnSingleCore != null &&
                        result3.TestMode == G9EPerformanceTestMode.Both &&
                        result3.NumberOfRepetitions == numberOfRepetitions &&
                        result3.AverageExecutionTimeOnMultiCore < result3.AverageExecutionTimeOnSingleCore);

            Thread.Sleep(99);

            // Comparative Performance Test
            var comparativeResult = G9Assembly.PerformanceTools.ComparativePerformanceTester(
                G9EPerformanceTestMode.Both,
                numberOfRepetitions, StringSum1, StringSum2);

            // It can be weird, but in fact, in C#, the operator of string (string A += "...")  is less speed than (string A = A + "..." ).
            // Note: Specially in multi-thread
            Assert.True(
                comparativeResult.SortedPerformanceSpeedPercentageForSingleCore.Count == 1 &&
                comparativeResult.SortedPerformanceSpeedPercentageForSingleCore[0].ReadableResult
                    .StartsWith("The performance speed of action") &&
                comparativeResult.SortedPerformanceSpeedPercentageForMultiCore.Count == 1 &&
                comparativeResult.SortedPerformanceSpeedPercentageForMultiCore[0].ReadableResult
                    .StartsWith("The performance speed of action") &&
                comparativeResult.MultiCoreFastestProcessIndex == 1 &&
                comparativeResult.PerformanceResults[0].AverageExecutionTimeOnMultiCore >
                comparativeResult.PerformanceResults[1].AverageExecutionTimeOnMultiCore &&
                comparativeResult.PerformanceResults[0].AverageExecutionTimeOnSingleCore >
                comparativeResult.PerformanceResults[1].AverageExecutionTimeOnSingleCore
            );

            // Comparative Performance Test - More that two actions
            var comparativeResultB = G9Assembly.PerformanceTools.ComparativePerformanceTester(
                G9EPerformanceTestMode.Both, 99_999,
                // Test list speed
                new G9DtCustomPerformanceAction("List Speed", () =>
                {
                    var Test = new List<string>();
                    for (var i = 0; i < 99; i++) Test.Add($"Test {i}");
                    Test.IndexOf("Test 69");
                }),
                // Test dictionary
                new G9DtCustomPerformanceAction("Dictionary Speed", () =>
                {
                    var Test = new Dictionary<int, string>();
                    for (var i = 0; i < 99; i++) Test.Add(i, "Test");
                    Test.First(s => s.Key == 69);
                }),
                // Test stack speed
                new G9DtCustomPerformanceAction("Stack Speed", () =>
                {
                    var Test = new Stack<string>();
                    for (var i = 0; i < 99; i++) Test.Push($"Test {i}");
                    Test.First(s => s == "Test 69");
                }));

            Assert.True(comparativeResultB.SortedPerformanceSpeedPercentageForMultiCore.Count == 2 &&
                        // Dictionary is a winner in single process
                        comparativeResultB.SingleCoreFastestProcessIndex == 1);
        }

        [Test]
        [Order(14)]
        public void TestCryptographyMethods()
        {
            const string testText = "My Name Is GAM3R!";

            const string testTextMd5 = "521aff434ec91a49bd80278750daa177";
            const string testTextSha1Hash = "e941e49818ce4ece330bbb924c7293588fa572a1";
            const string testTextSha256Hash = "c549aca4f2ecdcbb06071689701aee11ac0f1d9cfea438fa0f32248c4d0e097f";
            const string testTextSha384Hash =
                "088995b6c6eaa3477ffc75e757c694c786260cb511ba49299f385aa33357635b00a588264ebd585b4c972cd8ad1e3c0f";
            const string testTextSha512Hash =
                "51022fb7f5528c1b71654721d01eaeabefa765028923ab1d3b444206d809014bf92a022f46e274bf38d6a071d02016de967cf51cbd381a130d75a884e9433b04";
            const string testTextCrc32Hash = "ee81e4db";

            #region Hashing Test

            // MD5
            var md5Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.MD5, testText);
            Assert.True(!string.IsNullOrEmpty(md5Hash) && md5Hash == testTextMd5);

            md5Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.MD5, testText, true);
            Assert.True(!string.IsNullOrEmpty(md5Hash) && md5Hash == testTextMd5.ToUpper());

            // SHA1
            var sha1Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.SHA1, testText);
            Assert.True(!string.IsNullOrEmpty(sha1Hash) && sha1Hash == testTextSha1Hash);

            sha1Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.SHA1, testText, true);
            Assert.True(!string.IsNullOrEmpty(sha1Hash) && sha1Hash == testTextSha1Hash.ToUpper());

            // SHA256
            var sha256Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.SHA256, testText);
            Assert.True(!string.IsNullOrEmpty(sha256Hash) && sha256Hash == testTextSha256Hash);

            sha256Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.SHA256, testText, true);
            Assert.True(!string.IsNullOrEmpty(sha256Hash) && sha256Hash == testTextSha256Hash.ToUpper());

            // SHA384
            var sha384Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.SHA384, testText);
            Assert.True(!string.IsNullOrEmpty(sha384Hash) && sha384Hash == testTextSha384Hash);

            sha384Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.SHA384, testText, true);
            Assert.True(!string.IsNullOrEmpty(sha384Hash) && sha384Hash == testTextSha384Hash.ToUpper());

            // SHA512
            var sha512Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.SHA512, testText);
            Assert.True(!string.IsNullOrEmpty(sha512Hash) && sha512Hash == testTextSha512Hash);

            sha512Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.SHA512, testText, true);
            Assert.True(!string.IsNullOrEmpty(sha512Hash) && sha512Hash == testTextSha512Hash.ToUpper());

            // CRC32
            var crc32Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.CRC32, testText);
            Assert.True(!string.IsNullOrEmpty(crc32Hash) && crc32Hash == testTextCrc32Hash);

            crc32Hash = G9Assembly.CryptographyTools.StringToCustomHash(G9EHashAlgorithm.CRC32, testText, true);
            Assert.True(!string.IsNullOrEmpty(crc32Hash) && crc32Hash == testTextCrc32Hash.ToUpper());
            // http://www.sha1-online.com/

            #endregion

            const string standardKey = "eShVmYp3s6v9y$B&";
            const string standardIv = "gUkXp2s5v8x/A?D(";

            #region AES Test

            // AES encrypt, default config
            var aesEncryptionText = G9Assembly.CryptographyTools.AesEncryptString(testText, standardKey, standardIv);
            Assert.True(!string.IsNullOrEmpty(aesEncryptionText));

            // AES decrypt, default config
            var aesDecryptionText =
                G9Assembly.CryptographyTools.AesDecryptString(aesEncryptionText, standardKey, standardIv);
            Assert.True(!string.IsNullOrEmpty(aesDecryptionText) && aesDecryptionText == testText);

            // AES encrypt/decrypt, custom config (CFB/CBC/ECB) (ANSIX923/ISO10126/None/Zeros)
            aesEncryptionText = G9Assembly.CryptographyTools.AesEncryptString(testText, standardKey, standardIv,
                new G9DtAESConfig(PaddingMode.ANSIX923, CipherMode.CFB));
            Assert.True(!string.IsNullOrEmpty(aesEncryptionText));
            aesDecryptionText = G9Assembly.CryptographyTools.AesDecryptString(aesEncryptionText, standardKey,
                standardIv, new G9DtAESConfig(PaddingMode.ANSIX923, CipherMode.CFB));
            Assert.True(!string.IsNullOrEmpty(aesDecryptionText) && aesDecryptionText == testText);

            // AES encrypt/decrypt, custom config
            aesEncryptionText = G9Assembly.CryptographyTools.AesEncryptString(testText, standardKey, standardIv,
                new G9DtAESConfig(PaddingMode.ISO10126));
            Assert.True(!string.IsNullOrEmpty(aesEncryptionText));
            aesDecryptionText = G9Assembly.CryptographyTools.AesDecryptString(aesEncryptionText, standardKey,
                standardIv, new G9DtAESConfig(PaddingMode.ISO10126));
            Assert.True(!string.IsNullOrEmpty(aesDecryptionText) && aesDecryptionText == testText);

            // AES encrypt/decrypt, custom config
            var newText = testText.PadRight(32, '*');
            aesEncryptionText = G9Assembly.CryptographyTools.AesEncryptString(newText, standardKey, standardIv,
                new G9DtAESConfig(PaddingMode.None, CipherMode.ECB));
            Assert.True(!string.IsNullOrEmpty(aesEncryptionText));
            aesDecryptionText = G9Assembly.CryptographyTools.AesDecryptString(aesEncryptionText, standardKey,
                standardIv, new G9DtAESConfig(PaddingMode.None, CipherMode.ECB));
            Assert.True(!string.IsNullOrEmpty(aesDecryptionText) && aesDecryptionText == newText);

            // AES encrypt/decrypt, custom config
            aesEncryptionText = G9Assembly.CryptographyTools.AesEncryptString(testText, standardKey, standardIv,
                new G9DtAESConfig(PaddingMode.Zeros, CipherMode.CFB));
            Assert.True(!string.IsNullOrEmpty(aesEncryptionText));
            aesDecryptionText = G9Assembly.CryptographyTools.AesDecryptString(aesEncryptionText, standardKey,
                standardIv, new G9DtAESConfig(PaddingMode.Zeros, CipherMode.CFB));
            Assert.True(!string.IsNullOrEmpty(aesDecryptionText) && aesDecryptionText == testText);


            // AES encrypt/decrypt, nonstandard keys
            newText = testText.PadLeft(99);
            const string nonstandardKey = "G9";
            const string nonstandardIv = "TM";

            aesEncryptionText = G9Assembly.CryptographyTools.AesEncryptString(newText, nonstandardKey, nonstandardIv,
                new G9DtAESConfig(keySize: 256, blockSize: 128, enableAutoFixKeySize: true));
            Assert.True(!string.IsNullOrEmpty(aesEncryptionText));
            aesDecryptionText =
                G9Assembly.CryptographyTools.AesDecryptString(aesEncryptionText, nonstandardKey, nonstandardIv,
                    new G9DtAESConfig(keySize: 256, blockSize: 128, enableAutoFixKeySize: true));
            Assert.True(!string.IsNullOrEmpty(aesDecryptionText) && aesDecryptionText == newText);

            #endregion
        }

        [Test]
        [Order(15)]
        public void TestNew()
        {
            dynamic x = new G9ObjectData();
            x.gsdf = "asdasd";
            //x["asd"] = "321312";
        }
    }
}