// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("iQ0D16TZfg8u3REaioZnCe+FeRJrj7GEAV96Fr33Bt3ZSzlrNyAIBI4NAww8jg0GDo4NDQyzVL/w4evJ1K2qppe+gII5cqvrPAqR1UUD4V+7N/fJj+2UvEkU4Pnme2We8jOlcW55JXU3sJaf1q+nEfDUmmJe1WqD7zO5jQ9pffhXPO9Nvpkr+7Ye9pNLu6O5UWfJwOhDgB1JtaUkxwmv0WyseNlFslQOvrmNmDfCobFkRv1ROd1REBqqFNcAyHfZv0RbxZHHtu9TydGbsjzQnSEjzDM0lT72ODk2gjyODS48AQoFJopEivsBDQ0NCQwPC4DAbKIY18O9u614dXlU1GEzhPyfCcQL0BUrRh/zKCdpuNsmJgG2NEeZYYkmM3sIpw4PDQwN");
        private static int[] order = new int[] { 7,8,8,3,11,13,11,11,13,13,12,11,12,13,14 };
        private static int key = 12;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
