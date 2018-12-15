using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

using Prism.Commands;

using AtRec.Viewer.UI.SpecialControls.InternalControls;

namespace AtRec.Viewer.UI.SpecialControls
{
    [Localizability(LocalizationCategory.Ignore)]
    [ContentProperty(nameof(Pages))]
    public class ModernFrame : UserControl, IAddChild
    {
        // 非公開フィールド
        private ModernFramePageCollection _pages;
        private int _currentPage;

        private StackPanel _leftMenuStackPanel;
        private Grid _contentPanel;
        

        // 公開プロパティ

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ModernFramePageCollection Pages
        {
            get => this._pages;
        }

        public int CurrentPage
        {
            get => this._currentPage;
            set => this._setCurrentPage(value);
        }


        // コンストラクタ

        /// <summary>
        /// <see cref="ModernFrame"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ModernFrame()
        {
            this._pages = new ModernFramePageCollection();
            this._pages.CollectionChanged += _pages_CollectionChanged;
            this._currentPage = 0;
            this._initializeContainer();
        }


        // 非公開フィールド

        private void _pages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this._leftMenuStackPanel.Children.Clear();

            //.Where(item => item.Parent == this)
            foreach (var info in this._pages.Where(item => item is ModernFramePage).Select((item, index) => new { pageName = item.PageName, pageIndex = index }))
            {
                var button = new ModernFramePageButton() { ButtonText = info.pageName, IndexNumber = info.pageIndex };
                button.Command = new DelegateCommand(() => { this._setCurrentPage(info.pageIndex); });
                this._leftMenuStackPanel.Children.Add(button);
            }

            if (this._leftMenuStackPanel.Children.Count != 0)
                this._setCurrentPage(0);
        }

        private void _initializeContainer()
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            var leftMenuScrollViewer = new ScrollViewer();
            leftMenuScrollViewer.Background = Brushes.Silver;
            leftMenuScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            leftMenuScrollViewer.SetValue(Grid.ColumnProperty, 0);

            var leftMenuStackPanel = new StackPanel() { Orientation = Orientation.Vertical };
            leftMenuScrollViewer.Content = leftMenuStackPanel;
            this._leftMenuStackPanel = leftMenuStackPanel;

            var contentPanel = new Grid();
            contentPanel.SetValue(Grid.ColumnProperty, 1);
            this._contentPanel = contentPanel;

            grid.Children.Add(leftMenuScrollViewer);
            grid.Children.Add(contentPanel);

            this.Content = grid;
        }

        private void _setCurrentPage(int page)
        {
            this._currentPage = page;

            this._contentPanel.Children.Clear();
            this._contentPanel.Children.Add(this.Pages[page].PageContent);
        }


        // 公開メソッド

        void IAddChild.AddChild(object value)
        {
            if (value is ModernFramePage == false)
                throw new InvalidOperationException();

            this._pages.Add((ModernFramePage)value);
        }

        void IAddChild.AddText(string text)
        {
            throw new InvalidOperationException();
        }
    }
}
