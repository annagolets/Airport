namespace Airport.Application.Data
{
    public class Initializer
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();
        }
    }
}
