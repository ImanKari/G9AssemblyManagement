namespace G9AssemblyManagement_NUnitTest.StaticType
{
    public class G9DtStaticMemberType
    {
        #region Non static

        public string Name2 = "G9TM2";

        public int Age2 { set; get; } = 99;

        public string GetName()
        {
            return Name;
        }

        public TType TestStaticGeneric2<TType>(TType input)
        {
            return input;
        }

        #endregion

        #region Static

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

        #endregion
    }
}