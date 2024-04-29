namespace ECSGame.Scripts.TestDB
{
    public class PrefsDB : IDataBase
    {
        private static PrefsDB _prefsDB;
        public static PrefsDB I => _prefsDB ??= new PrefsDB();

        private PrefsDB()
        {
        }

        public void ShowInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}