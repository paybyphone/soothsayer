using System.Text;

namespace soothsayer.Scripts
{
    public static class UTF8
    {
        public static UTF8Encoding WithoutByteOrderMark = new UTF8Encoding(false);
    }
}