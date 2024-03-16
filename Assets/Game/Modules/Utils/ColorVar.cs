using UnityEngine;

namespace Game.Modules.Utils
{
    public static class ColorVar
    {
        public static Color32 WhiteColor = new(243, 238, 245, 255);
        public static Color32 BlackColor = new(44, 46, 51, 255);
        
        public static Color32 Ball1Color = WhiteColor;
        public static Color32 Ball2Color = new(155, 198, 227, 255);
        public static Color32 Ball3Color = new(165, 212, 155, 255);
        public static Color32 Ball4Color = new(220, 212, 146, 255);
        public static Color32 Ball5Color = new(248, 206, 163, 255);
        public static Color32 Ball6Color = new(198, 119, 126, 255);
        public static Color32 Ball7Color = new(190, 153, 176, 255);
        public static Color32 Ball8Color = new(97, 125, 160, 255);
        public static Color32 Ball9Color = new(108, 91, 124, 255);
        public static Color32 Ball10Color = new(74, 36, 39, 255);

        public static Color32 Ball1ColorDark = WhiteColor.MakeDarker();
        public static Color32 Ball2ColorDark = Ball2Color.MakeDarker();
        public static Color32 Ball3ColorDark = Ball3Color.MakeDarker();
        public static Color32 Ball4ColorDark = Ball4Color.MakeDarker();
        public static Color32 Ball5ColorDark = Ball5Color.MakeDarker();
        public static Color32 Ball6ColorDark = Ball6Color.MakeDarker();
        public static Color32 Ball7ColorDark = Ball7Color.MakeDarker();
        public static Color32 Ball8ColorDark = Ball8Color.MakeDarker();
        public static Color32 Ball9ColorDark = Ball9Color.MakeDarker();
        public static Color32 Ball10ColorDark = Ball10Color.MakeDarker();
        
        private static Color32 MakeDarker(this Color32 originalColor, float darkenFactor = 0.7f)
        {
            darkenFactor = Mathf.Clamp01(darkenFactor);

            var r = (byte)(originalColor.r * darkenFactor);
            var g = (byte)(originalColor.g * darkenFactor);
            var b = (byte)(originalColor.b * darkenFactor);

            return new Color32(r, g, b, originalColor.a);
        }
    }
}