// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("fgcADD0UKiiT2AFBlqA7f++pS/Ukp6mmliSnrKQkp6emGf4VWktBY5Ykp4SWq6CvjCDuIFGrp6eno6alxgbSc+8Y/qQUEycynWgLG87sV/uhKmrGCLJ9aRcRB9Lf0/5+y5kuVsTTj9+dGjw1fAUNu1p+MMj0f8Ap+WN7MRiWejeLiWaZnj+UXJKTnCgRnV1jJUc+FuO+SlNM0c80WJkP2zWjbqF6v4HstVmCjcMScYyMqxyeRZkTJ6XD11L9lkXnFDOBURy0XDnhEQkT+81jakLpKrfjHw+ObaMFe8ElGy6r9dC8F12sd3Phk8GdiqKuI6epfQ5z1KWEd7uwICzNo0Uv07iTd/u6sAC+fapi3XMV7vFvO20cRe0zyyOMmdGiDaSlp6an");
        private static int[] order = new int[] { 3,1,3,5,10,13,12,11,9,10,12,12,12,13,14 };
        private static int key = 166;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
