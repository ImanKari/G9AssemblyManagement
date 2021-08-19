using System.Linq;
using System.Reflection;
using G9AssemblyManagement;
using G9AssemblyManagement_NUnitTest.DataType;
using G9AssemblyManagement_NUnitTest.Inherit;
using G9AssemblyManagement_NUnitTest.InstanceTest;
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
            var getInheritInterfaceType = G9CAssemblyManagement.GetInheritedTypesFromType<G9ITestType>();
            Assert.True(getInheritInterfaceType.Count == 2);
            Assert.True(getInheritInterfaceType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from abstract generic class
            typesName = new[] {nameof(G9CGenericAbstractClassInheritAbstractTest), nameof(G9CGenericAbstractClassTest)};
            var getInheritGenericAbstractClassType =
                G9CAssemblyManagement.GetInheritedTypesFromType(typeof(G9ATestGenericAbstractClass<,,,>));
            Assert.True(getInheritGenericAbstractClassType.Count == 2);
            Assert.True(getInheritGenericAbstractClassType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from abstract generic class (Without Ignore Interface And AbstractType)
            typesName = new[]
            {
                typeof(G9ATestGenericAbstractClassInheritAbstractClass<,,,,>).Name, nameof(G9CGenericAbstractClassTest),
                nameof(G9CGenericAbstractClassInheritAbstractTest)
            };
            var getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType =
                G9CAssemblyManagement.GetInheritedTypesFromType(typeof(G9ATestGenericAbstractClass<,,,>), false, false);
            Assert.True(getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType.Count == 3);
            Assert.True(
                getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType.All(s =>
                    typesName.Contains(s.Name)));

            // Test get inherited types from generic interface (in custom assembly)
            typesName = new[] {nameof(G9CGenericInterfaceTest), nameof(G9CGenericInterfaceInheritInterfaceTest)};
            var getInheritGenericInterfaceType =
                G9CAssemblyManagement.GetInheritedTypesFromType(typeof(G9ITestGenericInterface<,,,>), true, true,
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
                G9CAssemblyManagement.GetInheritedTypesFromType(typeof(G9ITestGenericInterface<,,,>), false, false,
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
            var firstTestClassInstances1 = G9CAssemblyManagement.GetInstancesOfType<G9CInstanceTest>();
            Assert.True(firstTestClassInstances1.First().GetClassName() == nameof(G9CInstanceTest));
            // The second way
            var firstTestClassInstances2 = G9CAssemblyManagement.GetInstancesOfType(typeof(G9CInstanceTest))
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
            var firstTestStructInstances1 = G9CAssemblyManagement.GetInstancesOfType<G9DtInstanceTest>();
            Assert.True(firstTestStructInstances1.First().GetFirstName() == firstName);
            // The second way
            var firstTestStructInstances2 = G9CAssemblyManagement.GetInstancesOfType(typeof(G9DtInstanceTest))
                .Select(s => (G9DtInstanceTest) s);
            Assert.True(firstTestStructInstances2.First().GetFirstName() == firstName);
            // The third way
            var firstTestStructInstances3 = firstTestStruct.GetInstancesOfType().Select(s => (G9DtInstanceTest) s);
            Assert.True(firstTestStructInstances3.First().GetFirstName() == firstName);
        }
    }
}