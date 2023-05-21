namespace mojoin.Extension
{
    public static class ClaimHelper
    {
        public static int GetUserId(this HttpContext context)
        {
            int userId = 0;
            int.TryParse(context?.User?.Claims?.FirstOrDefault(x => x.Type == "UserId")?.Value, out userId);
            return userId;
        }
    }
}
