namespace Util
{
    public class Util
    {
        public string second2string(float second)
        {
            //时间转换
            var hour = (int) (second / 3600);
            second %= 3600;
            var minute = (int) (second / 60);
            second %= 60;
            return $"{hour:00}:{minute:00}:{second:00}";
        }
    }
}