namespace G9AssemblyManagement_NUnitTest.StaticType
{
    public static class G9DtStaticType
    {
        public static string Name = "G9TM";

        public static int Age { set; get; } = 32;

        public static int GetNameAsHashCode()
        {
            return Name.GetHashCode();
        }

        public static TType TestStaticGeneric<TType>(TType input)
        {
            return input;
        }
    }
}