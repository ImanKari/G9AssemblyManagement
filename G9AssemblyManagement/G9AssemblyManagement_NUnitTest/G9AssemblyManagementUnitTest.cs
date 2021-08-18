using System.Linq;
using System.Reflection;
using G9AssemblyManagement;
using G9AssemblyManagement_NUnitTest.DataType;
using G9AssemblyManagement_NUnitTest.Inherit;
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
            var getInheritClassType = objectItem.GetInheritedTypesOfType();
            Assert.True(getInheritClassType.Count == 1);
            Assert.True(getInheritClassType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from interface type
            typesName = new[] {nameof(G9CInterfaceInheritTest), nameof(G9DtStructInheritTest)};
            var getInheritInterfaceType = G9CAssemblyManagement.GetInheritedTypesOfType<G9ITestType>();
            Assert.True(getInheritInterfaceType.Count == 2);
            Assert.True(getInheritInterfaceType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from abstract generic class
            typesName = new[] {nameof(G9CGenericAbstractClassInheritAbstractTest), nameof(G9CGenericAbstractClassTest)};
            var getInheritGenericAbstractClassType =
                G9CAssemblyManagement.GetInheritedTypesOfType(typeof(G9ATestGenericAbstractClass<,,,>));
            Assert.True(getInheritGenericAbstractClassType.Count == 2);
            Assert.True(getInheritGenericAbstractClassType.All(s => typesName.Contains(s.Name)));

            // Test get inherited types from abstract generic class (Without Ignore Interface And AbstractType)
            typesName = new[]
            {
                typeof(G9ATestGenericAbstractClassInheritAbstractClass<,,,,>).Name, nameof(G9CGenericAbstractClassTest),
                nameof(G9CGenericAbstractClassInheritAbstractTest)
            };
            var getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType =
                G9CAssemblyManagement.GetInheritedTypesOfType(typeof(G9ATestGenericAbstractClass<,,,>), false, false);
            Assert.True(getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType.Count == 3);
            Assert.True(
                getInheritGenericAbstractClassTypeWithoutIgnoreInterfaceAndAbstractType.All(s =>
                    typesName.Contains(s.Name)));

            // Test get inherited types from generic interface (in custom assembly)
            typesName = new[] {nameof(G9CGenericInterfaceTest), nameof(G9CGenericInterfaceInheritInterfaceTest)};
            var getInheritGenericInterfaceType =
                G9CAssemblyManagement.GetInheritedTypesOfType(typeof(G9ITestGenericInterface<,,,>), true, true,
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
                G9CAssemblyManagement.GetInheritedTypesOfType(typeof(G9ITestGenericInterface<,,,>), false, false,
                    Assembly.GetExecutingAssembly());
            Assert.True(getInheritGenericInterfaceTypeWithoutIgnoreInterfaceAndAbstractType.Count == 3);
            Assert.True(getInheritGenericInterfaceTypeWithoutIgnoreInterfaceAndAbstractType.All(s => typesName.Contains(s.Name)));
        }
    }
}