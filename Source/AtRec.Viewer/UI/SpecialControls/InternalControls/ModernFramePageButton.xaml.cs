using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AtRec.Viewer.UI.SpecialControls.InternalControls
{
    /// <summary>
    /// ModernFramePageButton.xaml の相互作用ロジック
    /// </summary>
    public partial class ModernFramePageButton : UserControl
    {
        // 公開プロパティ

        public string ButtonText
        {
            get => this.MainButton.Content.ToString();
            set => this.MainButton.Content = value;
        }

        public Brush RightSpaceBrush
        {
            get => this.RightBox.Background;
            set => this.RightBox.Background = value;
        }

        public int IndexNumber
        {
            get; set;
        }

        public ICommand Command
        {
            get => this.MainButton.Command;
            set => this.MainButton.Command = value;
        }


        // コンストラクタ

        public ModernFramePageButton()
        {
            InitializeComponent();
        }
    }
}
