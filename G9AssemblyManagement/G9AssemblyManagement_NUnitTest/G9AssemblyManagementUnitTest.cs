using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using G9AssemblyManagement;
using G9AssemblyManagement.Enums;
using G9AssemblyManagement.Helper;
using G9AssemblyManagement_NUnitTest.DataType;
using G9AssemblyManagement_NUnitTest.Inherit;
using G9AssemblyManagement_NUnitTest.InstanceListener;
using G9AssemblyManagement_NUnitTest.InstanceTest;
using G9AssemblyManagement_NUnitTest.ObjectMembers;
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
            var getInheritClassType = objectItem.G9GetInheritedTypesFromType();
            Assert.True(getInheritClassType.Count == 1);
            Assert.True(getInheritClassType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from interface type
            typesName = new[] { nameof(G9CInterfaceInheritTest), nameof(G9DtStructInheritTest) };
            var getInheritInterfaceType = G9CAssemblyManagement.TypeHandlers.G9GetInheritedTypesFromType<G9ITestType>();
            Assert.True(getInheritInterfaceType.Count == 2);
            Assert.True(getInheritInterfaceType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from abstract generic class
            typesName = new[]
                { nameof(G9CGenericAbstractClassInheritAbstractTest), nameof(G9CGenericAbstractClassTest) };
            var getInheritGenericAbstractClassType =
                G9CAssemblyManagement.TypeHandlers.G9GetInheritedTypesFromType(
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
                G9CAssemblyManagement.TypeHandlers.G9GetInheritedTypesFromType(typeof(G9ATestGenericAbstractClass<,,,>),
                    false,
                    false);
            Assert.True(getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType.Count == 3);
            Assert.True(
                getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType.All(s =>
                    typesName.Contains(s.Name)));

            // Test get inherited types from generic interface (in custom assembly)
            typesName = new[] { nameof(G9CGenericInterfaceTest), nameof(G9CGenericInterfaceInheritInterfaceTest) };
            var getInheritGenericInterfaceType =
                G9CAssemblyManagement.TypeHandlers.G9GetInheritedTypesFromType(typeof(G9ITestGenericInterface<,,,>),
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
                G9CAssemblyManagement.TypeHandlers.G9GetInheritedTypesFromType(typeof(G9ITestGenericInterface<,,,>),
                    false,
                    false,
                    Assembly.GetExecutingAssembly());
            Assert.True(getInheritGenericInterfaceTypeWithoutIgnoreInterfaceAndAbstractType.Count == 3);
            Assert.True(
                getInheritGenericInterfaceTypeWithoutIgnoreInterfaceAndAbstractType.All(s =>
                    typesName.Contains(s.Name)));
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
                G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType<G9CInstanceTest>();
            Assert.True(firstTestClassInstances1.First().GetClassName() == nameof(G9CInstanceTest));
            // The second way
            var firstTestClassInstances2 = G9CAssemblyManagement.InstanceHandlers
                .G9GetInstancesOfType(typeof(G9CInstanceTest))
                .Select(s => (G9CInstanceTest)s);
            Assert.True(firstTestClassInstances2.First().GetClassName() == nameof(G9CInstanceTest));
            // The third way
            var firstTestClassInstances3 = firstTestClass.G9GetInstancesOfType().Select(s => (G9CInstanceTest)s);
            Assert.True(firstTestClassInstances3.First().GetClassName() == nameof(G9CInstanceTest));


            // New instance of class
            const string firstName = "Iman";
            var firstTestStruct = new G9DtInstanceTest(firstName);

            // Get instances of type - three way
            // The first way
            var firstTestStructInstances1 =
                G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType<G9DtInstanceTest>();
            Assert.True(firstTestStructInstances1.First().GetFirstName() == firstName);
            // The second way
            var firstTestStructInstances2 = G9CAssemblyManagement.InstanceHandlers
                .G9GetInstancesOfType(typeof(G9DtInstanceTest))
                .Select(s => (G9DtInstanceTest)s);
            Assert.True(firstTestStructInstances2.First().GetFirstName() == firstName);
            // The third way
            var firstTestStructInstances3 = firstTestStruct.G9GetInstancesOfType().Select(s => (G9DtInstanceTest)s);
            Assert.True(firstTestStructInstances3.First().GetFirstName() == firstName);


            // New instance of custom type
            var arrayValue = new[] { "firstTest", "secondTest", "thirdTest" };
            var firstTestCustomType = new Trait(arrayValue[0], arrayValue[0]);
            var secondTestCustomType = new Trait(arrayValue[1], arrayValue[1]);
            var thirdTestCustomType = new Trait(arrayValue[2], arrayValue[2]);
            // Assign instances - Used for classes not implemented by us
            G9CAssemblyManagement.InstanceHandlers.G9AssignInstanceOfType(firstTestCustomType);
            G9CAssemblyManagement.InstanceHandlers.G9AssignInstanceOfType(secondTestCustomType);
            G9CAssemblyManagement.InstanceHandlers.G9AssignInstanceOfType(thirdTestCustomType);
            // Get instances of custom type - three way
            // The first way
            var firstTestCustomTypeInstances1 = G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType<Trait>();
            Assert.True(firstTestCustomTypeInstances1.Count == 3);
            // The second way
            var firstTestCustomTypeInstances2 = G9CAssemblyManagement.InstanceHandlers
                .G9GetInstancesOfType(typeof(Trait))
                .Select(s => (Trait)s);
            Assert.True(firstTestCustomTypeInstances2.Count() == 3);
            // The third way
            var firstTestCustomTypeInstances3 = firstTestCustomType.G9GetInstancesOfType().Select(s => (Trait)s);
            Assert.True(firstTestCustomTypeInstances3.Count() == 3);
            // Test values
            Assert.True(firstTestCustomTypeInstances1.All(s => arrayValue.Contains(s.Value)));

            // Test Unassigning
            G9CAssemblyManagement.InstanceHandlers.G9UnassignInstanceOfType(thirdTestCustomType);
            G9CAssemblyManagement.InstanceHandlers.G9UnassignInstanceOfType(secondTestCustomType);
            firstTestCustomTypeInstances1 = G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType<Trait>();
            Assert.True(firstTestCustomTypeInstances1.Count == 1);

            // Test automatic Unassigning (Notice: Worked just for types inherited from the abstract class "G9AClassInitializer")
            // New instance of class
            IList<G9CMultiInstanceTest> instances;
            var firstClass = new G9CMultiInstanceTest();
            var secondClass = new G9CMultiInstanceTest();
            using (var thirdClass = new G9CMultiInstanceTest())
            {
                Assert.True(thirdClass.GetClassName() == nameof(G9CMultiInstanceTest));
                instances = G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType<G9CMultiInstanceTest>();
                Assert.True(instances.Count == 3);
            }

            // Automatic unassigning after block using
            instances = G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 2);
            // Automatic Unassigning with dispose
            secondClass.Dispose();
            instances = G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 1);
            firstClass.Dispose();
            instances = G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 0);
            instances = G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType<G9CMultiInstanceTest>();
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
                G9CAssemblyManagement.InstanceHandlers.G9AssignInstanceListener<G9CInstanceListenerTest>
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
                var instanceListener2 = G9CAssemblyManagement.InstanceHandlers
                    .G9AssignInstanceListener<G9CInstanceListenerTest>
                    (
                        // On assign
                        newInstance =>
                        {
                            newInstance.GetUseCounter();
                            instanceCount++;
                        }, justListenToNewInstance: false
                    );
                Assert.True(
                    G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType<G9CInstanceListenerTest>().Count ==
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

            object1.G9AssignInstanceListener(
                assignObjectItem => { count1++; },
                unassignObject => { count1--; }, onErrorException => throw onErrorException);

            object2.G9AssignInstanceListener(
                assignObjectItem => { count2++; },
                unassignObject => { count2--; }, onErrorException => throw onErrorException);


            _ = Parallel.For(0, 99_999, index =>
            {
                using var newObject1 = new G9CInstanceTest();
                using var newObject2 = new G9CThirdClass();
                var hash1 = newObject1.GetType().GetHashCode();
                var hash2 = newObject2.GetType().GetHashCode();

                Assert.True(hash1 == h1);
                Assert.True(hash2 == h2);
            });

            Assert.True(count1 == 0 && count2 == 0);

            _ = Parallel.For(0, 99_999, index =>
            {
                var newObject1 = new G9CInstanceTest();
                var newObject2 = new G9CThirdClass();
                var hash1 = newObject1.GetType().GetHashCode();
                var hash2 = newObject2.GetType().GetHashCode();

                Assert.True(hash1 == h1);
                Assert.True(hash2 == h2);

                newObject1.Dispose();
                newObject2.Dispose();
            });

            Assert.True(count1 == 0 && count2 == 0);

            _ = Parallel.For(0, 999_999, index =>
            {
                using var newObject1 = new G9CInstanceTest();
                using var newObject2 = new G9CThirdClass();
                var hash1 = newObject1.GetType().GetHashCode();
                var hash2 = newObject2.GetType().GetHashCode();

                Assert.True(hash1 == h1);
                Assert.True(hash2 == h2);

                newObject1.Dispose();
                newObject2.Dispose();
            });

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
            var fieldsOfObject1 = object1.G9GetFieldsOfObject(G9EAccessModifier.Public);
            var fieldsOfObject2 = object2.G9GetFieldsOfObject(G9EAccessModifier.NonPublic);
            var fieldsOfObject3 = object3.G9GetFieldsOfObject(G9EAccessModifier.Static | G9EAccessModifier.Public);
            var fieldsOfObject4 = object4.G9GetFieldsOfObject(G9EAccessModifier.Static | G9EAccessModifier.NonPublic);
            var fieldsOfObject5 = object5.G9GetFieldsOfObject();

            // It has one public field
            Assert.True(fieldsOfObject1.Count == 1);

            // It has three non public fields
            // One field is defined on the body, and two fields are defined as a back field
            Assert.True(fieldsOfObject2.Count == 3);

            // Get Public/Private/static fields
            fieldsOfObject1 = object1.G9GetFieldsOfObject();
            fieldsOfObject2 = object2.G9GetFieldsOfObject();

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
            fieldsOfObject5 = object5.G9GetFieldsOfObject(G9EAccessModifier.Everything, s => s.Name.Contains("1"));
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
            var fieldsOfObject1 = object1.G9GetPropertiesOfObject(G9EAccessModifier.Public);
            var fieldsOfObject2 = object2.G9GetPropertiesOfObject(G9EAccessModifier.NonPublic);
            var fieldsOfObject3 = object3.G9GetPropertiesOfObject(G9EAccessModifier.Static | G9EAccessModifier.Public);
            var fieldsOfObject4 =
                object4.G9GetPropertiesOfObject(G9EAccessModifier.Static | G9EAccessModifier.NonPublic);
            var fieldsOfObject5 = object5.G9GetPropertiesOfObject();

            // It has one public property
            Assert.True(fieldsOfObject1.Count == 1);

            // It has one non public property
            Assert.True(fieldsOfObject2.Count == 1);

            // Get Public/Private/static properties
            fieldsOfObject1 = object1.G9GetPropertiesOfObject();
            fieldsOfObject2 = object2.G9GetPropertiesOfObject();

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
            fieldsOfObject5 = object5.G9GetPropertiesOfObject(G9EAccessModifier.Everything, s => s.Name.Contains("1"));
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
            var fieldsOfObject1 = object1.G9GetMethodsOfObject(G9EAccessModifier.Public);
            var fieldsOfObject2 = object2.G9GetMethodsOfObject(G9EAccessModifier.NonPublic);
            var fieldsOfObject3 = object3.G9GetMethodsOfObject(G9EAccessModifier.Static | G9EAccessModifier.Public);
            var fieldsOfObject4 = object4.G9GetMethodsOfObject(G9EAccessModifier.Static | G9EAccessModifier.NonPublic);

            // Notice
            // 1- Each object has some built-in public methods (GetType() - ToString() - Equals() - GetHashCode())
            // 2- Each object has some built-in private method (MemberwiseClone() - Finalize())
            // 3- Each object with the property can have auto-made methods for the "set" or "get" processes of properties.

            // It has one public method + 4 built-in public methods + 2 auto-made property public methods
            Assert.True(fieldsOfObject1.Count == 7);

            // It has one non public method (internal) + 2 built-in private methods + one auto-made property method (just has getter)
            Assert.True(fieldsOfObject2.Count == 4);

            // Get Public/Private/static methods
            fieldsOfObject1 = object1.G9GetMethodsOfObject();
            fieldsOfObject2 = object2.G9GetMethodsOfObject();

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
            fieldsOfObject1 = object1.G9GetMethodsOfObject(G9EAccessModifier.Everything, s => s.Name.Contains("1"));
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
            var fieldsOfObject1 = object1.G9GetGenericMethodsOfObject(G9EAccessModifier.Public);
            var fieldsOfObject2 = object2.G9GetGenericMethodsOfObject(G9EAccessModifier.NonPublic);
            var fieldsOfObject3 =
                object3.G9GetGenericMethodsOfObject(G9EAccessModifier.Static | G9EAccessModifier.Public);
            var fieldsOfObject4 =
                object4.G9GetGenericMethodsOfObject(G9EAccessModifier.Static | G9EAccessModifier.NonPublic);

            // It don't have a public generic method
            Assert.True(fieldsOfObject1.Count == 0);

            // It has one non public generic method (private)
            Assert.True(fieldsOfObject2.Count == 1);

            // Get Public/Private/static methods
            fieldsOfObject1 = object1.G9GetGenericMethodsOfObject();
            fieldsOfObject2 = object2.G9GetGenericMethodsOfObject();

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
            fieldsOfObject2 = object2.G9GetGenericMethodsOfObject(G9EAccessModifier.Everything,
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
            var fieldsOfObject1 = object1.G9GetAllMembersOfObject();
            var fieldsOfObject2 = object2.G9GetAllMembersOfObject();

            // Test count of members
            Assert.True(fieldsOfObject1.Fields.Count == 6 && fieldsOfObject1.Properties.Count == 3 &&
                        fieldsOfObject1.Methods.Count == 13 && fieldsOfObject1.GenericMethods.Count == 2);
            Assert.True(fieldsOfObject2.Fields.Count == 6 && fieldsOfObject2.Properties.Count == 3 &&
                        fieldsOfObject2.Methods.Count == 13 && fieldsOfObject2.GenericMethods.Count == 2);
        }

        [Test]
        [Order(10)]
        public void TestCreateInstanceFromType()
        {
            // Create instance from a normal type
            var testObject1 = G9CAssemblyManagement.InstanceHandlers.G9CreateInstanceFromType<G9DtNormalType>();
            testObject1.Name = "Okay";
            Assert.True(testObject1.Name == "Okay");

            // Create instance from a generic type
            var testObject2 =
                typeof(G9DtGenericType<string>)
                    .G9CreateInstanceFromType<G9DtGenericType<string>>();
            testObject2.ValueOfType = "G9TM";
            Assert.True(testObject2.ValueOfType == "G9TM");

            // Create instance from a normal type with constructor
            var testObject3 =
                typeof(G9DtNormalTypeByConstructor).G9CreateInstanceFromTypeWithParameters<G9DtNormalTypeByConstructor>(
                    "Hello G9TM");
            Assert.True(testObject3.Name == "Hello G9TM");

            // Create instance from a generic type with constructor
            var testObject4 = typeof(G9DtGenericTypeByConstructor<string, int, IPAddress>)
                .G9CreateInstanceFromTypeWithParameters<G9DtGenericTypeByConstructor<string, int, IPAddress>>("G9TM",
                    999, IPAddress.IPv6None);
            Assert.True(testObject4.ObjectType1 == "G9TM" && testObject4.ObjectType2 == 999 &&
                        Equals(testObject4.ObjectType3, IPAddress.IPv6None));
        }
    }
}