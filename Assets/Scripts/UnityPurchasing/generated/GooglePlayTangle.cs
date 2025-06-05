// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("7S35WMQz1Y8/OAwZtkMgMOXHfNAeiEWKUZSqx55yqaboOVqnp4A3teoOMAWA3vuXPHaHXFjKuOq2oYmFVSwrJxY/AQO48ypqvYsQVMSCYN46tnZIDmwVPciVYXhn+uQfc7Ik8A+Mgo29D4yHjw+MjI0y1T5xYGpI0khQGjO9URygok2ytRS/d7m4twO4XNCRmyuVVoFJ9lg+xdpEEEY3br0PjK+9gIuEpwvFC3qAjIyMiI2OigFB7SOZVkI8Oiz59PjVVeCyBX0IjIJWJVj/jq9ckJsLB+aIbgT4k+/4pPS2MRceVy4mkHFVG+PfVOsCyjoiONDmSEFpwgGcyDQkpUaILlBusjgMjuj8eda9bsw/GKp6N593EsYY4AinsvqJJo+OjI2M");
        private static int[] order = new int[] { 5,9,8,3,11,9,12,8,9,10,10,13,12,13,14 };
        private static int key = 141;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
