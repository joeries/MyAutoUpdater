namespace Demo.Updater
{
    public class UpdaterHelper
    {
        public static IUpdater GetInstance(string updaterWay)
        {
            IUpdater updater = null;
            switch (updaterWay)
            {
                case "MyAuto":
                    updater = new MyAutoUpdater();
                    break;
                default:
                    updater = new DefaultUpdater();
                    break;
            }

            return updater;
        }
    }
}