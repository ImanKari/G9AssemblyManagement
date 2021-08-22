using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using G9AssemblyManagement;
using G9AssemblyManagement.Helper;
using G9AssemblyManagement_NUnitTest.DataType;
using G9AssemblyManagement_NUnitTest.Inherit;
using G9AssemblyManagement_NUnitTest.InstanceListener;
using G9AssemblyManagement_NUnitTest.InstanceTest;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace G9AssemblyManagement_NUnitTest
{
    public class Tests
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
            var typesName = new[] {nameof(G9CClassInheritTest)};
            var objectItem = new G9CTestType();
            var getInheritClassType = objectItem.GetInheritedTypesFromType();
            Assert.True(getInheritClassType.Count == 1);
            Assert.True(getInheritClassType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from interface type
            typesName = new[] {nameof(G9CInterfaceInheritTest), nameof(G9DtStructInheritTest)};
            var getInheritInterfaceType = G9CAssemblyManagement.Types.GetInheritedTypesFromType<G9ITestType>();
            Assert.True(getInheritInterfaceType.Count == 2);
            Assert.True(getInheritInterfaceType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from abstract generic class
            typesName = new[] {nameof(G9CGenericAbstractClassInheritAbstractTest), nameof(G9CGenericAbstractClassTest)};
            var getInheritGenericAbstractClassType =
                G9CAssemblyManagement.Types.GetInheritedTypesFromType(typeof(G9ATestGenericAbstractClass<,,,>));
            Assert.True(getInheritGenericAbstractClassType.Count == 2);
            Assert.True(getInheritGenericAbstractClassType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from abstract generic class (Without Ignore Interface And AbstractType)
            typesName = new[]
            {
                typeof(G9ATestGenericAbstractClassInheritAbstractClass<,,,,>).Name, nameof(G9CGenericAbstractClassTest),
                nameof(G9CGenericAbstractClassInheritAbstractTest)
            };
            var getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType =
                G9CAssemblyManagement.Types.GetInheritedTypesFromType(typeof(G9ATestGenericAbstractClass<,,,>), false,
                    false);
            Assert.True(getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType.Count == 3);
            Assert.True(
                getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType.All(s =>
                    typesName.Contains(s.Name)));

            // Test get inherited types from generic interface (in custom assembly)
            typesName = new[] {nameof(G9CGenericInterfaceTest), nameof(G9CGenericInterfaceInheritInterfaceTest)};
            var getInheritGenericInterfaceType =
                G9CAssemblyManagement.Types.GetInheritedTypesFromType(typeof(G9ITestGenericInterface<,,,>), true, true,
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
                G9CAssemblyManagement.Types.GetInheritedTypesFromType(typeof(G9ITestGenericInterface<,,,>), false,
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
            var firstTestClassInstances1 = G9CAssemblyManagement.Instances.GetInstancesOfType<G9CInstanceTest>();
            Assert.True(firstTestClassInstances1.First().GetClassName() == nameof(G9CInstanceTest));
            // The second way
            var firstTestClassInstances2 = G9CAssemblyManagement.Instances.GetInstancesOfType(typeof(G9CInstanceTest))
                .Select(s => (G9CInstanceTest) s);
            Assert.True(firstTestClassInstances2.First().GetClassName() == nameof(G9CInstanceTest));
            // The third way
            var firstTestClassInstances3 = firstTestClass.GetInstancesOfType().Select(s => (G9CInstanceTest) s);
            Assert.True(firstTestClassInstances3.First().GetClassName() == nameof(G9CInstanceTest));


            // New instance of class
            var firstName = "Iman";
            var firstTestStruct = new G9DtInstanceTest(firstName);

            // Get instances of type - three way
            // The first way
            var firstTestStructInstances1 = G9CAssemblyManagement.Instances.GetInstancesOfType<G9DtInstanceTest>();
            Assert.True(firstTestStructInstances1.First().GetFirstName() == firstName);
            // The second way
            var firstTestStructInstances2 = G9CAssemblyManagement.Instances.GetInstancesOfType(typeof(G9DtInstanceTest))
                .Select(s => (G9DtInstanceTest) s);
            Assert.True(firstTestStructInstances2.First().GetFirstName() == firstName);
            // The third way
            var firstTestStructInstances3 = firstTestStruct.GetInstancesOfType().Select(s => (G9DtInstanceTest) s);
            Assert.True(firstTestStructInstances3.First().GetFirstName() == firstName);


            // New instance of custom type
            var arrayValue = new[] {"firstTest", "secondTest", "thirdTest"};
            var firstTestCustomType = new Trait(arrayValue[0], arrayValue[0]);
            var secondTestCustomType = new Trait(arrayValue[1], arrayValue[1]);
            var thirdTestCustomType = new Trait(arrayValue[2], arrayValue[2]);
            // Assign instances - Used for classes not implemented by us
            G9CAssemblyManagement.Instances.AssignInstanceOfType(firstTestCustomType);
            G9CAssemblyManagement.Instances.AssignInstanceOfType(secondTestCustomType);
            G9CAssemblyManagement.Instances.AssignInstanceOfType(thirdTestCustomType);
            // Get instances of custom type - three way
            // The first way
            var firstTestCustomTypeInstances1 = G9CAssemblyManagement.Instances.GetInstancesOfType<Trait>();
            Assert.True(firstTestCustomTypeInstances1.Count == 3);
            // The second way
            var firstTestCustomTypeInstances2 = G9CAssemblyManagement.Instances.GetInstancesOfType(typeof(Trait))
                .Select(s => (Trait) s);
            Assert.True(firstTestCustomTypeInstances2.Count() == 3);
            // The third way
            var firstTestCustomTypeInstances3 = firstTestCustomType.GetInstancesOfType().Select(s => (Trait) s);
            Assert.True(firstTestCustomTypeInstances3.Count() == 3);
            // Test values
            Assert.True(firstTestCustomTypeInstances1.All(s => arrayValue.Contains(s.Value)));

            // Test Unassign
            G9CAssemblyManagement.Instances.UnassignInstanceOfType(thirdTestCustomType);
            G9CAssemblyManagement.Instances.UnassignInstanceOfType(secondTestCustomType);
            firstTestCustomTypeInstances1 = G9CAssemblyManagement.Instances.GetInstancesOfType<Trait>();
            Assert.True(firstTestCustomTypeInstances1.Count == 1);

            // Test automatic unassign (Notice: Worked just for types inherited from the abstract class "G9AClassInitializer")
            // New instance of class
            IList<G9CMultiInstanceTest> instances;
            var firstClass = new G9CMultiInstanceTest();
            var secondClass = new G9CMultiInstanceTest();
            using (var thirdClass = new G9CMultiInstanceTest())
            {
                Assert.True(thirdClass.GetClassName() == nameof(G9CMultiInstanceTest));
                instances = G9CAssemblyManagement.Instances.GetInstancesOfType<G9CMultiInstanceTest>();
                Assert.True(instances.Count == 3);
            }

            // Unassign automatic after block using
            instances = G9CAssemblyManagement.Instances.GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 2);
            // Unassign automatic with dispose
            secondClass.Dispose();
            instances = G9CAssemblyManagement.Instances.GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 1);
            firstClass.Dispose();
            instances = G9CAssemblyManagement.Instances.GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 0);
            instances = G9CAssemblyManagement.Instances.GetInstancesOfType<G9CMultiInstanceTest>();
            Assert.True(instances.Count == 0);
        }

        [Test]
        [Order(2)]
        public void TestInstanceListener()
        {
            var exceptionMessage = "Just for test, receive it in 'On receive exception'";
            // Save count of receive new instance
            var instanceCount = 0;
            var instanceListener = G9CAssemblyManagement.Instances.AssignInstanceListener<G9CInstanceListenerTest>
            (
                // On assign
                newInstance =>
                {
                    Assert.True(newInstance.GetUseCounter() == 1);
                    instanceCount++;
                },
                // On unassign
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
            var arrayClass = new object[]
            {
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest()
            };
            Assert.True(instanceCount == 3);

            // Stop listener test
            instanceListener.StopListener();
            var arrayClass2 = new object[]
            {
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest()
            };
            Assert.True(instanceCount == 3);

            // Resume listener test
            instanceListener.ResumeListener();
            var arrayClass3 = new object[]
            {
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest(),
                new G9CInstanceListenerTest()
            };
            Assert.True(instanceCount == 6);

            // Dispose listener test
            instanceListener.Dispose();
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
                var instanceListener2 = G9CAssemblyManagement.Instances.AssignInstanceListener<G9CInstanceListenerTest>
                (
                    // On assign
                    newInstance =>
                    {
                        newInstance.GetUseCounter();
                        instanceCount++;
                    }, justListenToNewInstance: false
                );
                Assert.True(G9CAssemblyManagement.Instances.GetInstancesOfType<G9CInstanceListenerTest>().Count ==
                            instanceCount);
            }

            // Automatic dispose listener after block area 
            GC.Collect();
        }
    }
}