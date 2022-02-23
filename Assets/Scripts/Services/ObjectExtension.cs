namespace Tools.Util.Extensions
{
    public static class ObjectUtility
    {
        public static string GetTypeWithNamespace(this object obj)
        {
            var type = obj.GetType();
            
            if (type.Namespace == null)
            {
                return type.ToString();
            }
            
            // NOTE: there might be better fix when interpreting the typestring, instead of this solution?
            if (type.Namespace != "UnityEngine")
            {
                return type.ToString();
            }

            return $"{type}, {type.Namespace}";
        }
    }
}