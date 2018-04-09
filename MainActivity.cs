using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace LoginApp
{
    [Activity(Label = "LoginApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        EditText et_user;
        EditText et_pass;
        Button btn_login;
        Button btn_users;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_main);
            InitApp();
        }

        private void InitApp()
        {
            DBController.InitDB();
            InitViews();
        }

        private void InitViews()
        {
            et_user = FindViewById<EditText>(Resource.Id.et_user);
            et_pass = FindViewById<EditText>(Resource.Id.et_pass);
            btn_login = FindViewById<Button>(Resource.Id.btn_login);
            btn_users = FindViewById<Button>(Resource.Id.btn_users);

            btn_login.Click += BtnLogin_Click;
            btn_users.Click += BtnUsers_Click;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (DBController.AuthUserLogin(DBController.GetConn(), 
                et_user.Text.ToString(),
                et_pass.Text.ToString())){
                StartActivity(typeof(SuccessActivity));
            }
            else
            {
                DialogLoginFail();
            }
        }

        private void DialogLoginFail()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Login Failed");
            alert.SetMessage("Invalid username or password");
            alert.SetPositiveButton("OK", (senderAlert, args) => {
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void BtnUsers_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(UserListActivity));
        }
    }
}

