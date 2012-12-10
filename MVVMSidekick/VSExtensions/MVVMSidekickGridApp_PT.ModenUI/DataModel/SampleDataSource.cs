using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;

using MVVMSidekick.ViewModels;
// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace $safeprojectname$.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon<TBindable> : BindableBase<TBindable> where TBindable : SampleDataCommon<TBindable> 
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.Image = new BitmapImage(new Uri(SampleDataCommon<TBindable>._baseUri, imagePath)); 
        }

        
        public String UniqueId
        {
            get { return m_UniqueIdLocator(this).Value; }
            set { m_UniqueIdLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property String UniqueId Setup
        protected Property<String> m_UniqueId =
          new Property<String> { LocatorFunc = m_UniqueIdLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<String>> m_UniqueIdLocator =
            RegisterContainerLocator<String>(
                "UniqueId",
                model =>
                {
                    model.m_UniqueId =
                        model.m_UniqueId
                        ??
                        new Property<String> { LocatorFunc = m_UniqueIdLocator };
                    return model.m_UniqueId.Container =
                        model.m_UniqueId.Container
                        ??
                        new ValueContainer<String>("UniqueId", model);
                });
        #endregion


        
        public string Title
        {
            get { return m_TitleLocator(this).Value; }
            set { m_TitleLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property string Title Setup
        protected Property<string> m_Title =
          new Property<string> { LocatorFunc = m_TitleLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<string>> m_TitleLocator =
            RegisterContainerLocator<string>(
                "Title",
                model =>
                {
                    model.m_Title =
                        model.m_Title
                        ??
                        new Property<string> { LocatorFunc = m_TitleLocator };
                    return model.m_Title.Container =
                        model.m_Title.Container
                        ??
                        new ValueContainer<string>("Title", model);
                });
        #endregion

        
        public string Subtitle
        {
            get { return m_SubtitleLocator(this).Value; }
            set { m_SubtitleLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property string Subtitle Setup
        protected Property<string> m_Subtitle =
          new Property<string> { LocatorFunc = m_SubtitleLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<string>> m_SubtitleLocator =
            RegisterContainerLocator<string>(
                "Subtitle",
                model =>
                {
                    model.m_Subtitle =
                        model.m_Subtitle
                        ??
                        new Property<string> { LocatorFunc = m_SubtitleLocator };
                    return model.m_Subtitle.Container =
                        model.m_Subtitle.Container
                        ??
                        new ValueContainer<string>("Subtitle", model);
                });
        #endregion


        
        public string Description
        {
            get { return m_DescriptionLocator(this).Value; }
            set { m_DescriptionLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property string Description Setup
        protected Property<string> m_Description =
          new Property<string> { LocatorFunc = m_DescriptionLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<string>> m_DescriptionLocator =
            RegisterContainerLocator<string>(
                "Description",
                model =>
                {
                    model.m_Description =
                        model.m_Description
                        ??
                        new Property<string> { LocatorFunc = m_DescriptionLocator };
                    return model.m_Description.Container =
                        model.m_Description.Container
                        ??
                        new ValueContainer<string>("Description", model);
                });
        #endregion




        
        public ImageSource Image
        {
            get { return m_ImageLocator(this).Value; }
            set { m_ImageLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property ImageSource Image Setup
        protected Property<ImageSource> m_Image =
          new Property<ImageSource> { LocatorFunc = m_ImageLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<ImageSource>> m_ImageLocator =
            RegisterContainerLocator<ImageSource>(
                "Image",
                model =>
                {
                    model.m_Image =
                        model.m_Image
                        ??
                        new Property<ImageSource> { LocatorFunc = m_ImageLocator };
                    return model.m_Image.Container =
                        model.m_Image.Container
                        ??
                        new ValueContainer<ImageSource>("Image", model);
                });
        #endregion



     

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon<SampleDataItem>
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this.Content = content;
            this.Group = group;
        }

        
        public string  Content
        {
            get { return m_ContentLocator(this).Value; }
            set { m_ContentLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property string  Content Setup
        protected Property<string > m_Content =
          new Property<string > { LocatorFunc = m_ContentLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<string >> m_ContentLocator =
            RegisterContainerLocator<string >(
                "Content",
                model =>
                {
                    model.m_Content =
                        model.m_Content
                        ??
                        new Property<string > { LocatorFunc = m_ContentLocator };
                    return model.m_Content.Container =
                        model.m_Content.Container
                        ??
                        new ValueContainer<string >("Content", model);
                });
        #endregion


        
        public SampleDataGroup Group
        {
            get { return m_GroupLocator(this).Value; }
            set { m_GroupLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property SampleDataGroup Group Setup
        protected Property<SampleDataGroup> m_Group =
          new Property<SampleDataGroup> { LocatorFunc = m_GroupLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<SampleDataGroup>> m_GroupLocator =
            RegisterContainerLocator<SampleDataGroup>(
                "Group",
                model =>
                {
                    model.m_Group =
                        model.m_Group
                        ??
                        new Property<SampleDataGroup> { LocatorFunc = m_GroupLocator };
                    return model.m_Group.Container =
                        model.m_Group.Container
                        ??
                        new ValueContainer<SampleDataGroup>("Group", model);
                });
        #endregion

    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon<SampleDataGroup>
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex,Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<SampleDataItem> _topItem = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> TopItems
        {
            get {return this._topItem; }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// SampleDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
            String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat");

            var group1 = new SampleDataGroup("Group-1",
                    "Group Title: 1",
                    "Group Subtitle: 1",
                    "Assets/DarkGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                    "Item Title: 4",
                    "Item Subtitle: 4",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                    "Item Title: 5",
                    "Item Subtitle: 5",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group1));
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                    "Group Title: 2",
                    "Group Subtitle: 2",
                    "Assets/LightGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group2));
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group2));
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group2));
            this.AllGroups.Add(group2);

            var group3 = new SampleDataGroup("Group-3",
                    "Group Title: 3",
                    "Group Subtitle: 3",
                    "Assets/MediumGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group3.Items.Add(new SampleDataItem("Group-3-Item-1",
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-2",
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-3",
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-4",
                    "Item Title: 4",
                    "Item Subtitle: 4",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-5",
                    "Item Title: 5",
                    "Item Subtitle: 5",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-6",
                    "Item Title: 6",
                    "Item Subtitle: 6",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-7",
                    "Item Title: 7",
                    "Item Subtitle: 7",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group3));
            this.AllGroups.Add(group3);

            var group4 = new SampleDataGroup("Group-4",
                    "Group Title: 4",
                    "Group Subtitle: 4",
                    "Assets/LightGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group4.Items.Add(new SampleDataItem("Group-4-Item-1",
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-2",
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-3",
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-4",
                    "Item Title: 4",
                    "Item Subtitle: 4",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-5",
                    "Item Title: 5",
                    "Item Subtitle: 5",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-6",
                    "Item Title: 6",
                    "Item Subtitle: 6",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group4));
            this.AllGroups.Add(group4);

            var group5 = new SampleDataGroup("Group-5",
                    "Group Title: 5",
                    "Group Subtitle: 5",
                    "Assets/MediumGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group5.Items.Add(new SampleDataItem("Group-5-Item-1",
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group5));
            group5.Items.Add(new SampleDataItem("Group-5-Item-2",
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group5));
            group5.Items.Add(new SampleDataItem("Group-5-Item-3",
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group5));
            group5.Items.Add(new SampleDataItem("Group-5-Item-4",
                    "Item Title: 4",
                    "Item Subtitle: 4",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group5));
            this.AllGroups.Add(group5);

            var group6 = new SampleDataGroup("Group-6",
                    "Group Title: 6",
                    "Group Subtitle: 6",
                    "Assets/DarkGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group6.Items.Add(new SampleDataItem("Group-6-Item-1",
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-2",
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-3",
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-4",
                    "Item Title: 4",
                    "Item Subtitle: 4",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-5",
                    "Item Title: 5",
                    "Item Subtitle: 5",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-6",
                    "Item Title: 6",
                    "Item Subtitle: 6",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-7",
                    "Item Title: 7",
                    "Item Subtitle: 7",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-8",
                    "Item Title: 8",
                    "Item Subtitle: 8",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group6));
            this.AllGroups.Add(group6);
        }
    }
}
