using System;
using System.Windows;
namespace JointCharsToOne.WPF
{
    public interface ITextToPathService
    {
        string Text2Path(string strText, string strCulture, bool LtoR, string strTypeFace, int nSize,Thickness masks );
        string GetAllToTogether(string[] paths);
        
    }
}
