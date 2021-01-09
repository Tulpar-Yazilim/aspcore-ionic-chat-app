using System;

namespace ChatApp.Const
{
    public static class TulparValidation
    {
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return (guid == Guid.Empty || guid == null);
        }
        public static bool IsNullOrEmpty(this Guid guid)
        {
            return (guid == Guid.Empty);
        }
    }
}
