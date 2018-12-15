using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace AtRec.Viewer.UI.SpecialControls
{
    [Localizability(LocalizationCategory.Ignore)]
    [ContentProperty("PageContent")]
    public class ModernFramePage : UserControl, IAddChild
    {
        // 公開プロパティ

        public string PageName
        {
            get;
            set;
        }

        public UIElement PageContent
        {
            get;
            set;
        }

        public ModernFramePage()
        {
            this.PageContent = new UIElement();
        }

        void IAddChild.AddChild(object value)
        {
            if (value is UIElement == false)
                throw new InvalidOperationException();
            this.PageContent = (UIElement)value;
        }

        void IAddChild.AddText(string text)
        {
            throw new InvalidOperationException();
        }
    }
}
