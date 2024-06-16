namespace WebPresentationLayer.Utility
{
    public static class Utility
    {
        public static byte[] ConvertFormFileToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
