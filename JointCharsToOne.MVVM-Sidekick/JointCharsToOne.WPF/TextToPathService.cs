using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace JointCharsToOne.WPF
{
    public class TextToPathService : JointCharsToOne.WPF.ITextToPathService
    {
        public string Text2Path(String strText, string strCulture, bool LtoR, string strTypeFace, int nSize, Thickness masks)
        {
            // Set up the Culture
            if (strCulture == "")
                strCulture = "en-us";
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(strCulture);


            // Set up the flow direction
            System.Windows.FlowDirection fd;
            if (LtoR)
                fd = FlowDirection.LeftToRight;
            else
                fd = FlowDirection.RightToLeft;

            // Set up the font family from the parameter
            FontFamily ff = new FontFamily(strTypeFace);

            // Create the new typeface
            System.Windows.Media.Typeface tf = new System.Windows.Media.Typeface(ff,

                FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);


            // Create a formatted text object from the text, 

            // culture, flowdirection, typeface, size and black


            FormattedText t = new FormattedText(strText, ci, fd, tf, nSize,

                System.Windows.Media.Brushes.Black);

            // Build a Geometry out of this
            Geometry g = t.BuildGeometry(new Point(0, 0));

            // Get the Path info from the geometry
            PathGeometry p = g.GetFlattenedPathGeometry();
            var x = nSize - masks.Left - masks.Right;
            var y = nSize - masks.Top - masks.Bottom;
            var size = new Size(x < 0 ? 0 : x, y < 0 ? 0 : y);
            var rectv = new Rect(new Point(masks.Left, masks.Top), size);

            RectangleGeometry rg = new RectangleGeometry(rectv);
            var cg = new CombinedGeometry(p, rg);
            cg.GeometryCombineMode = GeometryCombineMode.Intersect;









            p = cg.GetFlattenedPathGeometry();

            // Return the path info
            return p.ToString();


        }



        public string GetAllToTogether(string[] paths)
        {
            PathGeometry result = new PathGeometry();
            foreach (var item in paths)
            {
                result.AddGeometry (  PathGeometry.Parse(item));

               
            }


            return result.ToString();
        }
    }
}
