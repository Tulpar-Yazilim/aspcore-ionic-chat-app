namespace ChatApp.Const
{
    public static class Messages
    {
        #region General Messages
        public static string Ok = "Ok";
        public static string Error = "Error";
        public static string NoRecord = "NoRecord";
        public static string Unauthorized = "Unauthorized";
        public static string UsernameTaken = "UsernameTaken";

        public static string ProjectUrlName = "https://localhost:55685/";
        public static string AtLeastOneRecordMustBe = "AtLeastOneRecordMustBe";

        public static string WebOneSignalAppID = "******";
        public static string WebOneSignalApiKey = "******";
        #endregion

        #region Db Config
        public static DevelopmentMode RunTimeProjectType = DevelopmentMode.Local;
        public static string DatabaseLocalConnectionStr = "User ID=postgres;Password=;Server=localhost;Port=5432;Database=TulparYazilimDb";
        public static string DatabaseTestConnectionStr = "User ID=postgres;Password=;Server=localhost;Port=5432;Database=TulparYazilimDb";
        public static string DatabaseProdConnectionStr = "User ID=postgres;Password=;Server=localhost;Port=5432;Database=TulparYazilimDb";
        #endregion
    }


    public enum DevelopmentMode
    {
        Local = 1,
        Development = 2,
        Production = 3
    }
}
