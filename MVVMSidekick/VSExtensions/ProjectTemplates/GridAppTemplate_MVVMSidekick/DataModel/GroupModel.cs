using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using $safeprojectname$.Data;



namespace $safeprojectname$.Data
{

    [DataContract]
    public class GroupModel : ViewModelBase<GroupModel>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        [DataMember]
        public IEnumerable<SampleDataGroup> Groups
        {
            get
            {

                return m_GroupsLocator(this).Value;
            }
            set
            {

                m_GroupsLocator(this).SetValueAndTryNotify(value);
            }
        }

        #region Property IEnumerable<SampleDataGroup> Groups Setup
        protected Property<IEnumerable<SampleDataGroup>> m_Groups =
          new Property<IEnumerable<SampleDataGroup>> { LocatorFunc = m_GroupsLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<IEnumerable<SampleDataGroup>>> m_GroupsLocator =
            RegisterContainerLocator<IEnumerable<SampleDataGroup>>(
                "Groups",
                model =>
                {
                    model.m_Groups =
                        model.m_Groups
                        ??
                        new Property<IEnumerable<SampleDataGroup>> { LocatorFunc = m_GroupsLocator };
                    return model.m_Groups.Container =
                        model.m_Groups.Container
                        ??
                        new ValueContainer<IEnumerable<SampleDataGroup>>("Groups", model);
                });
        #endregion


        
        public IEnumerable<SampleDataItem > Items
        {
            get { return m_ItemsLocator(this).Value; }
            set { m_ItemsLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property IEnumerable<SampleDataItem > Items Setup
        protected Property<IEnumerable<SampleDataItem >> m_Items =
          new Property<IEnumerable<SampleDataItem >> { LocatorFunc = m_ItemsLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<IEnumerable<SampleDataItem >>> m_ItemsLocator =
            RegisterContainerLocator<IEnumerable<SampleDataItem >>(
                "Items",
                model =>
                {
                    model.m_Items =
                        model.m_Items
                        ??
                        new Property<IEnumerable<SampleDataItem >> { LocatorFunc = m_ItemsLocator };
                    return model.m_Items.Container =
                        model.m_Items.Container
                        ??
                        new ValueContainer<IEnumerable<SampleDataItem >>("Items", model);
                });
        #endregion

        
        public SampleDataGroup  Group
        {
            get { return m_GroupLocator(this).Value; }
            set { m_GroupLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property SampleDataGroup  Group Setup
        protected Property<SampleDataGroup > m_Group =
          new Property<SampleDataGroup > { LocatorFunc = m_GroupLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<SampleDataGroup >> m_GroupLocator =
            RegisterContainerLocator<SampleDataGroup >(
                "Group",
                model =>
                {
                    model.m_Group =
                        model.m_Group
                        ??
                        new Property<SampleDataGroup > { LocatorFunc = m_GroupLocator };
                    return model.m_Group.Container =
                        model.m_Group.Container
                        ??
                        new ValueContainer<SampleDataGroup >("Group", model);
                });
        #endregion

    }

}
