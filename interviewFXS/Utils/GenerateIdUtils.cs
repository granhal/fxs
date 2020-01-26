using System;
namespace interviewFXS.Utils
{
    public static class GenerateIdUtils
    {

        public static string GenerateId()
        {
            //var now = DateTime.Now;
            //var zeroDate = DateTime.MinValue.AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second).AddMilliseconds(now.Millisecond);
            //int uniqueId = (int)(zeroDate.Ticks / 10000);

            int i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:d}", Math.Abs(i - DateTime.Now.Millisecond));
        }
    }
}
