using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;

namespace LoginApp
{
    [Activity(Label = "UserListActivity")]
    public class UserListActivity : Activity
    {
        private List<string> usernames;
        ListView lv_users;
        private TextView tv_add;
        private ImageView iv_back;

        protected override void OnCreate(Bundle b)
        {
            base.OnCreate(b);
            SetContentView(Resource.Layout.layout_users_list);
            Init();
        }

        private void Init()
        {
            lv_users = FindViewById<ListView>(Resource.Id.lv_users);
            setUserNames();
            tv_add = FindViewById<TextView>(Resource.Id.tv_add);
            iv_back = FindViewById<ImageView>(Resource.Id.iv_back);
            tv_add.Click += BtnAdd_Click;
            iv_back.Click += delegate
            {
                Finish();
            };
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AddUserActivity));
        }

        private void setUserNames()
        {
            usernames = DBController.GetAllUsernames();
            lv_users.Adapter = new ArrayAdapter<string>(this, Resource.Layout.item_user, usernames);
        }

        protected override void OnResume()
        {
            if (usernames != null && lv_users.Adapter != null)
            {
                setUserNames();
            }
            base.OnResume();
        }
    }
}